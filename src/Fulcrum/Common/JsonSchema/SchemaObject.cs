using System.Collections.Generic;
using Fulcrum.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Fulcrum.Common.JsonSchema
{
	public class SchemaObject
	{
		public SchemaObject()
		{
			Properties = new Dictionary<string, ISchemaPropertyMetadata>();
			Required = new List<string>();
		}

		public SchemaObject(SchemaObjectType type, string description, string title) : this()
		{
			Description = description;
			Title = title;
			Type = type;
		}

		public SchemaObject(CommandSchema commandSchema, string name, string @namespace)
			: this()
		{
			Type = SchemaObjectType.Object;
			Title = name;
			Description = string.Format("JSON schema for {0}/{1}.", @namespace, name);

			if (commandSchema.Properties == null)
			{
				return;
			}

			if (commandSchema.ValidateByQuery)
			{
				ValidationQueryUrl = commandSchema.ValidationQueryUrl;
			}

			foreach (var property in commandSchema.Properties)
			{
				var validated = isPropertyValidated(property.Value);

				var propertyType = getPropertyType(property.Value);

				ISchemaPropertyMetadata metadata;

				if (validated)
				{
					var validationMetadata = getValidatedPropertyMetadata(property.Value, propertyType);

					if (validationMetadata.Required.HasValue && validationMetadata.Required.Value)
					{
						Required.Add(property.Key);
					}

					metadata = validationMetadata;
				}
				else
				{
					metadata = new SimplePropertyMetadata(propertyType);
				}

				Properties.Add(property.Key, metadata);
			}
		}

		public SchemaObject(EventSchema eventSchema, string name, string @namespace)
			: this()
		{
			Type = SchemaObjectType.Object;
			Title = name;
			Description = string.Format("JSON schema for {0}/{1}.", @namespace, name);

			if (eventSchema.Properties == null)
			{
				return;
			}

			if (eventSchema.ValidateByQuery)
			{
				ValidationQueryUrl = eventSchema.ValidationQueryUrl;
			}

			foreach (var property in eventSchema.Properties)
			{
				var validated = isPropertyValidated(property.Value);

				var propertyType = getPropertyType(property.Value);

				ISchemaPropertyMetadata metadata;

				if (validated)
				{
					var validationMetadata = getValidatedPropertyMetadata(property.Value, propertyType);

					if (validationMetadata.Required.HasValue && validationMetadata.Required.Value)
					{
						Required.Add(property.Key);
					}

					metadata = validationMetadata;
				}
				else
				{
					metadata = new SimplePropertyMetadata(propertyType);
				}

				Properties.Add(property.Key, metadata);
			}
		}

		public string Description { get; private set; }

		public IDictionary<string, ISchemaPropertyMetadata> Properties { get; private set; }

		public IList<string> Required { get; private set; }

		public string Title { get; private set; }

		public SchemaObjectType Type { get; private set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string ValidationQueryUrl { get; set; }

		private SchemaPropertyType getPropertyType(Newtonsoft.Json.Schema.JsonSchema value)
		{
			var schemaType = value.Type.Value;

			if (schemaType == JsonSchemaType.Boolean)
			{
				return SchemaPropertyType.Boolean;
			}

			if (schemaType == JsonSchemaType.Integer)
			{
				return SchemaPropertyType.Boolean;
			}

			if (schemaType == JsonSchemaType.Float)
			{
				return SchemaPropertyType.Float;
			}

			if (schemaType == JsonSchemaType.String)
			{
				return SchemaPropertyType.String;
			}

			return SchemaPropertyType.Object;
		}

		private ValidatedPropertyMetadata getValidatedPropertyMetadata
			(Newtonsoft.Json.Schema.JsonSchema value, SchemaPropertyType propertyType)
		{
			var result = new ValidatedPropertyMetadata(propertyType);

			if (value.ExclusiveMaximum.HasValue)
			{
				result.ExclusiveMaximum = value.ExclusiveMaximum;
			}

			if (value.ExclusiveMinimum.HasValue)
			{
				result.ExclusiveMinimum = value.ExclusiveMinimum;
			}

			if (value.Maximum.HasValue)
			{
				result.Maximum = value.Maximum;
			}

			if (value.MaximumLength.HasValue)
			{
				result.MaximumLength = value.MaximumLength;
			}

			if (value.Minimum.HasValue)
			{
				result.Minimum = value.Minimum;
			}

			if (value.MinimumLength.HasValue)
			{
				result.MinimumLength = value.MinimumLength;
			}

			if (!string.IsNullOrWhiteSpace(value.Pattern))
			{
				result.Pattern = value.Pattern;
			}

			if (value.Required.HasValue)
			{
				result.Required = value.Required;
			}

			return result;
		}

		private bool isPropertyValidated(Newtonsoft.Json.Schema.JsonSchema value)
		{
			var result = false;

			if (value.ExclusiveMaximum.HasValue)
			{
				result = true;
			}

			if (value.ExclusiveMinimum.HasValue)
			{
				result = true;
			}

			if (value.Maximum.HasValue)
			{
				result = true;
			}

			if (value.MaximumLength.HasValue)
			{
				result = true;
			}

			if (value.Minimum.HasValue)
			{
				result = true;
			}

			if (value.MinimumLength.HasValue)
			{
				result = true;
			}

			if (!string.IsNullOrWhiteSpace(value.Pattern))
			{
				result = true;
			}

			if (value.Required.HasValue)
			{
				result = true;
			}

			return result;
		}
	}
}
