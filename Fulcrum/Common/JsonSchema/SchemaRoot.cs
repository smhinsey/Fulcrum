using System.Collections.Generic;

namespace Fulcrum.Common.JsonSchema
{
	public class SchemaObject
	{
		public SchemaObject(SchemaObjectType type, string description, string title)
		{
			Description = description;
			Title = title;
			Type = type;

			Properties = new Dictionary<string, ISchemaPropertyMetadata>();
			Required = new List<string>();
		}

		public string Description { get; private set; }

		public IDictionary<string, ISchemaPropertyMetadata> Properties { get; private set; }

		public IList<string> Required { get; private set; }

		public string Title { get; private set; }

		public SchemaObjectType Type { get; private set; }
	}
}
