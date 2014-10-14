using System;
using System.Collections.Generic;
using System.Reflection;
using Fulcrum.Runtime;
using Tests.Unit.Commands.LocateTheseCommands;
using Xunit;

namespace Tests.Unit.Commands
{
	public class CommandLocationTests
	{
		[Fact]
		public void Locate_commands_in_assembly_via_namespace()
		{
			var extractor = new CommandLocator();

			var commands = extractor.FindCommands(Assembly.GetExecutingAssembly(),
				"Tests.Unit.Commands.LocateTheseCommands");

			var expectedCommands = new List<Type>()
			{
				typeof(AnotherCommandToLocate),
				typeof(LocateAnotherCommand),
				typeof(LocateOneCommand),
			};

			Assert.Equal(expectedCommands, commands);
		}
	}
}
