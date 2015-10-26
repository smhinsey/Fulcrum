using Fulcrum.Runtime;
using UnitTests.Unit.Commands.Validation;
using Xunit;

namespace UnitTests.Unit.Commands
{
	public class CommandSchemaValidationTests
	{
		[Fact]
		public void Pattern()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaValidationCommand));

			Assert.True(schema.Properties["emailWithPattern"].Pattern == ".@");
		}

		[Fact]
		public void Query()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaValidationCommand));

			var queryUrl = "/queries/Tests.Unit.Commands.Validation/ValidationQuery/validate";

			Assert.Equal(true, schema.ValidateByQuery);
			Assert.Equal(queryUrl, schema.ValidationQueryUrl);
		}

		[Fact]
		public void Required_with_min_and_max_values()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaValidationCommand));

			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Required.HasValue);
			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Required.Value);

			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Minimum == 18);
			Assert.True(schema.Properties["requiredAgeWithMinAndMax"].Maximum == 100);
		}

		[Fact]
		public void Simpled_required_field()
		{
			var schema = CommandSchemaGenerator.GenerateSchema(typeof(SchemaValidationCommand));

			Assert.True(schema.Properties["requiredFirstName"].Required.HasValue);
			Assert.True(schema.Properties["requiredFirstName"].Required.Value);
		}
	}
}
