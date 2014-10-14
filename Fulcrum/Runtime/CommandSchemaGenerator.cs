using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Schema;

namespace Fulcrum.Runtime
{
	public class CommandSchemaGenerator
	{
		public JsonSchema GenerateSchema(Type command)
		{
			var commandSchema = new JsonSchema();

			foreach (var property in command.GetProperties())
			{
				var propertyTypeTable = new Dictionary<Type, Action<PropertyInfo>>
				{
					{ typeof(string), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.String }) },
					{ typeof(int), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.String }) },
					{ typeof(float), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.String }) },
					{ typeof(bool), prop => addPropertyToSchema(commandSchema, prop, new JsonSchema { Type = JsonSchemaType.String }) }
				};

				var propertyType = property.PropertyType;

				if (propertyTypeTable.ContainsKey(propertyType))
				{
					propertyTypeTable[propertyType](property);
				}
				else
				{
					addPropertyToSchema(commandSchema, property, new JsonSchema { Type = JsonSchemaType.None });
				}
			}

			return commandSchema;
		}

		private void addPropertyToSchema(JsonSchema commandSchema, PropertyInfo property, JsonSchema propertySchema)
		{
			if (commandSchema.Properties == null)
			{
				commandSchema.Properties = new Dictionary<string, JsonSchema>();
			}

			addPropertyValidationToSchema(property, propertySchema);

			commandSchema.Properties.Add(formatPropertyName(property.Name), propertySchema);
		}

		private void addPropertyValidationToSchema(PropertyInfo property, JsonSchema propertySchema)
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

		private string formatPropertyName(string name)
		{
			return Char.ToLowerInvariant(name[0]) + name.Substring(1);
		}
	}
}
