using Fulcrum.Runtime;
using Tests.Unit.Commands.Validation;
using Xunit;

namespace Tests.Unit.Commands
{
	public class CommandSchemaGeneratorTests
	{
		[Fact]
		public void Generate_schema_for_bool()
		{
			var generator = new CommandSchemaGenerator();

			var schema = generator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.True(schema.Properties.ContainsKey("thisIsBoolean"));
		}

		[Fact]
		public void Generate_schema_for_int()
		{
			var generator = new CommandSchemaGenerator();

			var schema = generator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.True(schema.Properties.ContainsKey("anIntegerValue"));
		}

		[Fact]
		public void Generate_schema_for_string()
		{
			var generator = new CommandSchemaGenerator();

			var schema = generator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.True(schema.Properties.ContainsKey("someStringProperty"));
		}

		[Fact]
		public void Schema_includes_all_properties()
		{
			var generator = new CommandSchemaGenerator();

			var schema = generator.GenerateSchema(typeof(SchemaTestingCommand));

			Assert.Equal(3, schema.Properties.Count);
		}
	}
}
