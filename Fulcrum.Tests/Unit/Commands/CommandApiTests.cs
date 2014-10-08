using System.Reflection;
using Fulcrum.Runtime;
using Tests.Unit.Commands.DescribeTheseCommands;
using Xunit;

namespace Tests.Unit.Commands
{
	public class CommandApiTests
	{
		[Fact]
		public void Locate_commands_in_assembly_via_namespace()
		{
			var extractor = new CommandLocator();

			var commands = extractor.FindCommands(Assembly.GetExecutingAssembly(),
				"Tests.Unit.Commands.LocateTheseCommands");

			Assert.Equal(3, commands.Count);
		}

		[Fact]
		public void Generate_schema_for_basic_command()
		{
			var generator = new CommandSchemaGenerator();

			var schema = generator.GenerateSchema(typeof(DescribeThisBasicCommand));

			Assert.Equal(3, schema.Properties.Count);
			Assert.True(schema.Properties.ContainsKey("someStringProperty"));
			Assert.True(schema.Properties.ContainsKey("anIntegerValue"));
			Assert.True(schema.Properties.ContainsKey("thisIsBoolean"));
		}

		[Fact]
		public void Generate_schema_for_validated_command()
		{
			var generator = new CommandSchemaGenerator();

			var schema = generator.GenerateSchema(typeof(DescribeThisValidatedCommand));

			Assert.Equal(3, schema.Properties.Count);
			Assert.True(schema.Properties.ContainsKey("requiredFirstName"));
			Assert.True(schema.Properties.ContainsKey("requiredAgeWithMinAndMax"));
			Assert.True(schema.Properties.ContainsKey("emailWithPattern"));

			Assert.True(schema.Properties["requiredFirstName"].Required.HasValue);
			Assert.True(schema.Properties["requiredFirstName"].Required.Value);

			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Required.HasValue);
			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Required.Value);

			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Minimum == 18);
			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Maximum == 100);

			Assert.True(schema.Properties["emailWithPattern"].Pattern == ".@");
		}
	}
}
