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

		IList<IdentifierQueryReference> QueryReferences { get; set; }

        PortableEvent PortableEvent { get; }

		EventPublicationStatus Status { get; set; }

		DateTime? Updated { get; set; }
	}
}
