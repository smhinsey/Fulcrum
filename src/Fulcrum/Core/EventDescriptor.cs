namespace Fulcrum.Core
{
	public class EventDescriptor
	{
		public EventDescriptor(string name, string @namespace)
		{
			Namespace = @namespace;
			Name = name;
		}

		public string Name { get; private set; }

		public string Namespace { get; private set; }
	}
}
