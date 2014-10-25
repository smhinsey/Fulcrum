namespace Fulcrum.Common.JsonSchema
{
	public class ValidatedProperty : ISchemaProperty
	{
		public ValidatedProperty(SchemaPropertyType type)
		{
			Type = type;
		}

		public SchemaPropertyType Type { get; private set; }

		// TODO: add all json schema validation options, query validation, and ShouldSerialize{Member} methods
	}
}