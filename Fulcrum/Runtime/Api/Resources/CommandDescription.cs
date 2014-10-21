using System;
using Fulcrum.Core;
using Newtonsoft.Json.Schema;

namespace Fulcrum.Runtime.Api.Resources
{
	public class CommandDescription : CommandDescriptor
	{
		public CommandDescription(Type commandType) : base(commandType.Name, commandType.Namespace)
		{
			Schema = CommandSchemaGenerator.GenerateSchema(commandType);
			CommandModel = Activator.CreateInstance(commandType);
		}

		public JsonSchema Schema { get; private set; }

		public object CommandModel { get; private set; }
	}
}
