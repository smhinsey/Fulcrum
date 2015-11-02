namespace Fulcrum.Common.JsonSchema
{
	public enum SchemaPropertyType
	{
		// TODO: figure out how to get these to serialize as lower case in JSON so they can be correctly cased
		boolean,

		integer,

		@float,

		@string,

		@object,
	}
}
