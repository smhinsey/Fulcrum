namespace Fulcrum.Common.JsonSchema
{
	public class JsonLink
	{
		public JsonLink(string href, string rel)
		{
			Rel = rel;
			Href = href;
		}

		public string Href { get; private set; }

		public string Rel { get; private set; }
	}
}
