using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Fulcrum.Core;
using Newtonsoft.Json.Schema;

namespace Fulcrum.Runtime
{
	public static class QuerySchemaGenerator
	{
		public static QuerySchema GenerateSchema(MethodInfo queryMethod)
		{
			var commandSchema = new QuerySchema();

			var propertyTypeTable = new Dictionary<Type, Action<ParameterInfo>>
			{
				{ typeof(string), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.String }) },
				{ typeof(int), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.Integer }) },
				{ typeof(float), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.Float }) },
				{ typeof(bool), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.Boolean }) }
			};

			foreach (var param in queryMethod.GetParameters())
			{
				var propertyType = param.ParameterType;

				if (propertyTypeTable.ContainsKey(propertyType))
				{
					propertyTypeTable[propertyType](param);
				}
				else
				{
					// TODO: Is object the best type? Should this be an exception instead?
					addPropertyToSchema(commandSchema, param, new JsonSchema { Type = JsonSchemaType.Object });
				}
			}

			return commandSchema;
		}

		private static void addPropertyToSchema(JsonSchema commandSchema, ParameterInfo property, JsonSchema propertySchema)
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

		private static void addPropertyValidationToSchema(ParameterInfo property, JsonSchema propertySchema)
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
