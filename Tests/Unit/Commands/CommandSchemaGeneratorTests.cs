using Fulcrum.Runtime;
using Tests.Unit.Commands.Validation;
using Xunit;

namespace Tests.Unit.Commands
{
	// TODO: we should probably assert more than just the name on all of these
	public class CommandSchemaGeneratorTests
	{
		[Fact]
		public void Generate_schema_for_bool()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.True(schema.Properties.ContainsKey("thisIsBoolean"));
		}

		[Fact]
		public void Generate_schema_for_int()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.True(schema.Properties.ContainsKey("anIntegerValue"));
		}

		[Fact]
		public void Generate_schema_for_string()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.True(schema.Properties.ContainsKey("someStringProperty"));
		}

		[Fact]
		public void Schema_includes_all_properties()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.Equal(3, schema.Properties.Count);
		}
	}
}
