using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fulcrum.Core;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class CommandPublicationRegistry
	{
		private readonly CommandPipelineDbContext _db;

		public CommandPublicationRegistry(CommandPipelineDbContext db)
		{
			_db = db;
		}

		public ICommandPublicationRecord AssociateQueryReference(Guid publicationId, ParameterizedQueryReference reference)
		{
			var record = safelyFetchRecord(publicationId);

			record.Updated = DateTime.UtcNow;

			if (record.QueryReferences == null)
			{
				record.QueryReferences = new List<ParameterizedQueryReference>();
			}

			reference.Id = Guid.NewGuid();

			record.QueryReferences.Add(reference);

			_db.SaveChanges();

			return record;
		}

		private CommandPublicationRecord safelyFetchRecord(Guid publicationId)
		{
			var query = (from dbRecord in _db.CommandPublicationRecords
			                   where dbRecord.Id == publicationId
			                   select dbRecord);

			query = query.Include(r => r.QueryReferences);
			query = query.Include(r => r.QueryReferences.Select(q=>q.Parameters));

			var record = query.SingleOrDefault();

			if (record == null)
			{
				throw new Exception(string.Format("Unable to locate record {0}.", publicationId));
			}
			return record;
		}
	}
}
