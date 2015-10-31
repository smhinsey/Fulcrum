using System;
using System.Configuration;
using Fulcrum.Common;

namespace Fulcrum.Runtime
{
	public class AppSettings : ILoggingSource
	{
		public AppSettings()
		{
			ApiBasePath = SafeLoadString("ApiBasePath");
			AppRootNamespace = SafeLoadString("AppRootNamespace");
			ReleaseLabel = SafeLoadString("ReleaseLabel");

			EnableAuthentication = SafeLoadBool("EnableAuthentication");
			EnableAuthorization = SafeLoadBool("EnableAuthorization");

			var version = GetType().Assembly.GetName().Version;

			Version = string.Format("{0}-{1}.{2}.{3}.{4}",
				version.Major, version.MajorRevision, version.Minor, version.MinorRevision,
				version.Build);
		}

		public string ApiBasePath { get; set; }

		public string AppRootNamespace { get; set; }

		public bool EnableAuthentication { get; set; }

		public bool EnableAuthorization { get; set; }

		public string ReleaseLabel { get; set; }

		public string Version { get; set; }

		protected bool SafeLoadBool(string key)
		{
			var rawValue = SafeLoadString(key);

			bool extractedValue;

			var parsed = bool.TryParse(rawValue, out extractedValue);

			if (parsed)
			{
				return extractedValue;
			}

			var message = string.Format("appSetting {0} is a boolean but its value doesn't parse. Value: {1}",
				key, rawValue);

			this.LogWarn(message);

			throw new Exception(message);
		}

		protected string SafeLoadString(string key)
		{
			var value = ConfigurationManager.AppSettings[key];

			if (string.IsNullOrEmpty(value))
			{
				var message = string.Format("Expected to find an appSetting for key {0} but couldn't.",
					key);

				this.LogWarn(message);

				throw new Exception(message);
			}

			this.LogDebug("Loaded appSetting {0}, value: {1}",
				key, value);

			return value;
		}
	}
}
