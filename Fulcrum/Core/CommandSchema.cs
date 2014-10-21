using Newtonsoft.Json.Schema;

namespace Fulcrum.Core
{
	public class CommandSchema : JsonSchema
	{
		public bool ValidateByQuery { get; set; }

		public string ValidationQueryUrl { get; set; }
	}
}
