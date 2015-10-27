using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Core;
using Newtonsoft.Json.Schema;

namespace Fulcrum.Runtime
{
	public static class CommandSchemaGenerator
	{
		public static CommandSchema GenerateSchema(Type command)
		{
			var commandSchema = new CommandSchema();

			var propertyTypeTable = new Dictionary<Type, Action<PropertyInfo>>
			{
				{ typeof(string), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.String }) },
				{ typeof(int), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.Integer }) },
				{ typeof(float), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.Float }) },
				{ typeof(bool), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.Boolean }) }
			};

			foreach (var property in command.GetProperties())
			{
				var propertyType = property.PropertyType;

				if (propertyTypeTable.ContainsKey(propertyType))
				{
					propertyTypeTable[propertyType](property);
				}
				else
				{
					// TODO: Is object the best type? Should this be an exception instead?
					addPropertyToSchema(commandSchema, property, new JsonSchema { Type = JsonSchemaType.Object });
				}
			}

			addQueryValidation(commandSchema, command);

			return commandSchema;
		}

		private static void addPropertyToSchema(JsonSchema commandSchema, PropertyInfo property, JsonSchema propertySchema)
		{
			if (checkForBlacklistedName(property.Name))
			{
				return;
			}

			if (commandSchema.Properties == null)
			{
				commandSchema.Properties = new Dictionary<string, JsonSchema>();
			}

			addPropertyValidationToSchema(property, propertySchema);

			commandSchema.Properties.Add(formatPropertyName(property.Name), propertySchema);
		}

		private static void addPropertyValidationToSchema(PropertyInfo property, JsonSchema propertySchema)
		{
			var attributes = property.GetCustomAttributes(true);

			var validationTypeTable = new Dictionary<Type, Action<object>>
			{
				{
					typeof(RequiredAttribute), attr => propertySchema.Required = true
				},
				{
					typeof(RangeAttribute), attr =>
					                        {
						                        var range = (RangeAttribute)attr;

						                        propertySchema.Minimum = double.Parse(range.Minimum.ToString());
						                        propertySchema.Maximum = double.Parse(range.Maximum.ToString());
					                        }
				},
				{
					typeof(RegularExpressionAttribute), attr =>
					                                    {
						                                    var regex = (RegularExpressionAttribute)attr;

						                                    propertySchema.Pattern = regex.Pattern;
					                                    }
				}
			};

			foreach (var attr in attributes.Where(attr => validationTypeTable.ContainsKey(attr.GetType())))
			{
				validationTypeTable[attr.GetType()](attr);
			}
		}

		private static void addQueryValidation(CommandSchema commandSchema, Type commandType)
		{
			var queryValidationAttribute = commandType.GetAttribute<QueryValidationAttribute>();

			if (queryValidationAttribute == null)
			{
				return;
			}

			var descriptor = queryValidationAttribute.Descriptor;

			commandSchema.ValidateByQuery = true;
			// NOTE: we need a globally-safe way of referring to URLs
			commandSchema.ValidationQueryUrl = "/queries/" + descriptor.Namespace + "/" + descriptor.QueryObject + "/validate";
		}

		private static bool checkForBlacklistedName(string name)
		{
			return name == "PublicationRecordId";
		}

		private static string formatPropertyName(string name)
		{
			return Char.ToLowerInvariant(name[0]) + name.Substring(1);
		}
	}
}
