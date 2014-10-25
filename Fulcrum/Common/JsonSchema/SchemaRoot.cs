using System.Collections.Generic;

namespace Fulcrum.Common.JsonSchema
{
	public class SchemaObject
	{
		public SchemaObject(SchemaObjectType type, string description)
		{
			Description = description;
			Type = type;

			Properties = new Dictionary<string, ISchemaProperty>();
			Required = new List<string>();
		}

		public string Description { get; private set; }

		public IDictionary<string, ISchemaProperty> Properties { get; private set; }

		public IList<string> Required { get; private set; }

		public SchemaObjectType Type { get; private set; }
	}
}
