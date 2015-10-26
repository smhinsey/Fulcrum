using System;
using System.Linq;
using System.Security.Claims;

namespace Fulcrum.Common.Claims
{
	public static class ClaimsIdentityExtensions
	{
		public static Guid GetSessionLogId(this ClaimsIdentity identity)
		{
			var sessionLog = identity.Claims.SingleOrDefault(c => c.Type == "SessionLogId");

			if (sessionLog == null)
			{
				return Guid.Empty;
			}

			return Guid.Parse(sessionLog.Value);
		}
	}
}
