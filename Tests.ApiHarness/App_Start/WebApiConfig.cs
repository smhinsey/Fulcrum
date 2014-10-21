using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Tests.ApiHarness
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Formatters.JsonFormatter.SerializerSettings =
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Include,
					Formatting = Formatting.None,
					ContractResolver = new CamelCasePropertyNamesContractResolver(),
					Converters = new List<JsonConverter>
					{
						new StringEnumConverter()
					}
				};
		}
	}
}
