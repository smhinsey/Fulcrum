using System;

namespace Fulcrum.Core
{
	/// <summary>
	///   As a command is processed, a handlers can create references to queries
	///   which will return the read models it has created.
	/// </summary>
	public class IdentifierQueryReference
	{
		public Guid Id { get; set; }

		public string QueryName { get; set; }

		public string QueryParameter { get; set; }
	}
}
