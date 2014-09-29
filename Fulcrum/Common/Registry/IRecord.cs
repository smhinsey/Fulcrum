using System;

namespace Fulcrum.Common.Registry
{
	public interface IRecord
	{
		bool Active { get; }

		Guid Id { get; }
	}
}
