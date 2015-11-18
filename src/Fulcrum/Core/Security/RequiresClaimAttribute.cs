using System;

namespace Fulcrum.Core.Security
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class RequiresClaimAttribute : Attribute
	{
		public string Type { get; set; }

		public string Value { get; set; }

		public RequiresClaimAttribute(string type, string value)
		{
			Type = type;
			Value = value;
		}
	}
}