namespace Fulcrum.Common.JsonSchema
{
	public class UnvalidatedProperty : ISchemaProperty
	{
		public UnvalidatedProperty(SchemaPropertyType type)
		{
			Type = type;
		}

		public SchemaPropertyType Type { get; private set; }
	}
}