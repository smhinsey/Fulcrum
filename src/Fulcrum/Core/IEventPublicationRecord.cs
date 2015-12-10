using System;
using System.Collections.Generic;
using Fulcrum.Common.Registry;

namespace Fulcrum.Core
{
	public interface IEventPublicationRecord : IRecord
	{
		DateTime Created { get; }

		string ErrorDetails { get; set; }

		string ErrorHeadline { get; set; }

		PortableEvent PortableEvent { get; }

		IList<ParameterizedQueryReference> QueryReferences { get; set; }

		EventPublicationStatus Status { get; set; }

		DateTime? Updated { get; set; }
	}
}
