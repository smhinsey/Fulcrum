using Newtonsoft.Json.Schema;

namespace Fulcrum.Runtime.Api.Resources
{
	public class QueryDescription : QueryDescriptor
	{
		public QueryDescription(string parentNamespace, string name, JsonSchema schema) :
			base(parentNamespace, name)
		{
			Schema = schema;
		}

		public JsonSchema Schema { get; private set; }
	}
}
