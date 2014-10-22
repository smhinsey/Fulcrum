using System;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Resources
{
	public class CommandDescription : CommandDescriptor
	{
		public CommandDescription(Type commandType) : base(commandType.Name, commandType.Namespace)
		{
			Schema = CommandSchemaGenerator.GenerateSchema(commandType);
			CommandModel = Activator.CreateInstance(commandType);
		}

		public object CommandModel { get; private set; }

		public CommandSchema Schema { get; private set; }
	}
}
