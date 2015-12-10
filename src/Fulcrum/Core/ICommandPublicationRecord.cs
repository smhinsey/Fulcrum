using System;
using System.Collections.Generic;
using Fulcrum.Common.Registry;

namespace Fulcrum.Core
{
	public interface ICommandPublicationRecord : IRecord
	{
		DateTime Created { get; }

		string ErrorDetails { get; set; }

		string ErrorHeadline { get; set; }

		PortableCommand PortableCommand { get; }

		IList<ParameterizedQueryReference> QueryReferences { get; set; }

		CommandPublicationStatus Status { get; set; }

		DateTime? Updated { get; set; }
	}
}
