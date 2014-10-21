namespace Fulcrum.Runtime
{
	public class QueryDescriptor
	{
		public QueryDescriptor(string name, string @namespace)
		{
			Namespace = @namespace;
			Name = name;
		}

		public string Name { get; private set; }

		public string Namespace { get; private set; }
	}
}