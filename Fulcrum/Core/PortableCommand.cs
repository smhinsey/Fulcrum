using Newtonsoft.Json;

namespace Fulcrum.Core
{
	/// <summary>
	///   A portable representation of a command, where portable means that the command
	///   and its schema are encoded in JSON.
	/// </summary>
	public class PortableCommand
	{
		public PortableCommand(ICommand command, CommandSchema commandSchema)
		{
			ClrAssemblyName = command.GetType().Assembly.FullName;
			ClrTypeName = command.GetType().FullName;
			CommandJson = JsonConvert.SerializeObject(command);
			CommandJsonSchema = commandSchema.ToString();
		}

		public string ClrAssemblyName { get; private set; }

		public string ClrTypeName { get; private set; }

		public string CommandJson { get; private set; }

		public string CommandJsonSchema { get; private set; }
	}
}
