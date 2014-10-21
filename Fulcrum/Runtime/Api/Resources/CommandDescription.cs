using Newtonsoft.Json.Schema;

namespace Fulcrum.Runtime.Api.Resources
{
	public class CommandDescription : CommandDescriptor
	{
		public CommandDescription(string parentNamespace, string name, JsonSchema schema) :
			base(parentNamespace, name)
		{
			Schema = schema;
		}

		public JsonSchema Schema { get; private set; }
	}
}
