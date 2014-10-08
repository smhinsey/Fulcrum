using Fulcrum.Core;

namespace Tests.Unit.Commands.DescribeTheseCommands
{
	public class DescribeThisBasicCommand : ICommand
	{
		public int AnIntegerValue { get; set; }

		public string SomeStringProperty { get; set; }

		public bool ThisIsBoolean { get; set; }
	}
}
