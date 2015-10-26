using Fulcrum.Core;

namespace UnitTests.Commands.Validation
{
	public class SchemaTestingCommand : DefaultCommand
	{
		public int AnIntegerValue { get; set; }

		public string SomeStringProperty { get; set; }

		public bool ThisIsBoolean { get; set; }
	}
}
