using System;
using System.Collections.Generic;
using Fulcrum.Runtime;
using Tests.Unit.Commands.Location;
using Tests.Unit.Commands.Location.Additional;
using Xunit;

namespace Tests.Unit.Commands
{
	public class CommandLocationTests
	{
		[Fact]
		public void Find_command_in_namespace()
		{
			var commandType = typeof(LocateThisCommand);

			var extractor = new CommandLocator();

			extractor.AddCommandSource(commandType.Assembly, commandType.Namespace);

			var locatedType = extractor.Find(commandType.Name, commandType.Namespace);

			Assert.Equal(commandType, locatedType);
		}

		[Fact]
		public void List_all_commands_in_assembly_via_multiple_namespaces()
		{
			var firstCommandType = typeof(LocateThisCommand);
			var secondCommandType = typeof(AnAdditionalCommandToLocate);

			var extractor = new CommandLocator(firstCommandType.Assembly, firstCommandType.Namespace);

			extractor.AddCommandSource(secondCommandType.Assembly, secondCommandType.Namespace);

			var commands = extractor.ListAllCommands();

			var expectedCommands = new List<Type>()
			{
				typeof(AnotherCommandToLocate),
				typeof(LocateAnotherCommand),
				typeof(LocateThisCommand),
				typeof(AnAdditionalCommandToLocate),
			};

			Assert.Equal(expectedCommands, commands);
		}

		[Fact]
		public void List_all_commands_in_assembly_via_namespace_and_constructor()
		{
			var commandType = typeof(LocateThisCommand);

			var extractor = new CommandLocator(commandType.Assembly, commandType.Namespace);

			var commands = extractor.ListAllCommands();

			var expectedCommands = new List<Type>()
			{
				typeof(AnotherCommandToLocate),
				typeof(LocateAnotherCommand),
				typeof(LocateThisCommand),
			};

			Assert.Equal(expectedCommands, commands);
		}

		[Fact]
		public void List_all_commands_in_assembly_via_namespace_and_method()
		{
			var commandType = typeof(LocateThisCommand);

			var extractor = new CommandLocator();

			extractor.AddCommandSource(commandType.Assembly, commandType.Namespace);

			var commands = extractor.ListAllCommands();

			var expectedCommands = new List<Type>()
			{
				typeof(AnotherCommandToLocate),
				typeof(LocateAnotherCommand),
				typeof(LocateThisCommand),
			};

			Assert.Equal(expectedCommands, commands);
		}
	}
}
