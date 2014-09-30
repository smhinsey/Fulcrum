using System;
using System.Collections.Generic;

namespace Fulcrum.Common.Registry
{
	public interface IRegistry<TRecord>
		where TRecord : IRecord
	{
		IList<TRecord> FindAll();

		TRecord FindById(Guid id);

		void Save(TRecord record);
	}
}
