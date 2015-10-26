namespace Fulcrum.Core
{
	public class CommandDescriptor
	{
		public CommandDescriptor(string name, string @namespace)
		{
			Namespace = @namespace;
			Name = name;
		}

		public string Name { get; private set; }

		public string Namespace { get; private set; }
	}
}
