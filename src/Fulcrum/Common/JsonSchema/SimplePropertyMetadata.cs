namespace Fulcrum.Common.JsonSchema
{
	public class SimplePropertyMetadata : ISchemaPropertyMetadata
	{
		public SimplePropertyMetadata(SchemaPropertyType type)
		{
			Type = type;
		}

		public SchemaPropertyType Type { get; private set; }
	}
}