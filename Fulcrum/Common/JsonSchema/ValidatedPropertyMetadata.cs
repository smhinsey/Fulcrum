using Newtonsoft.Json;

namespace Fulcrum.Common.JsonSchema
{
	public class ValidatedPropertyMetadata : ISchemaPropertyMetadata
	{
		public ValidatedPropertyMetadata(SchemaPropertyType type)
		{
			Type = type;
		}

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? ExclusiveMaximum { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? ExclusiveMinimum { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? Maximum { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? MaximumLength { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? Minimum { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? MinimumLength { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Pattern { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? Required { get; set; }

		public SchemaPropertyType Type { get; private set; }
	}
}
