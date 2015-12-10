using System;
using System.Collections.Generic;

namespace Fulcrum.Core
{
	/// <summary>
	///   As a command is processed, a handlers can create references to queries
	///   which will return the read models it has created.
	/// </summary>
	public class ParameterizedQueryReference
	{
		public IList<QueryReferenceParameter> Parameters { get; set; }

		public string QueryName { get; set; }

		public Guid Id { get; set; }
	}
}
