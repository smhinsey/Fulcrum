using System;
using System.Collections.Generic;
using System.Linq;
using Fulcrum.Core;
using Fulcrum.Runtime.CommandPipeline;

namespace Fulcrum.Runtime.EventPipeline
{
	public class EventPublicationRegistry
	{
		private readonly CommandPipelineDbContext _db;

		public EventPublicationRegistry(CommandPipelineDbContext db)
		{
			_db = db;
		}

		public IEventPublicationRecord AssociateQueryReference(Guid publicationId, IdentifierQueryReference reference)
		{
			var record = safelyFetchRecord(publicationId);

			record.Updated = DateTime.UtcNow;

			if (record.QueryReferences == null)
			{
				record.QueryReferences = new List<IdentifierQueryReference>();
			}

			reference.Id = Guid.NewGuid();

			record.QueryReferences.Add(reference);

			_db.SaveChanges();

			return record;
		}

		private EventPublicationRecord safelyFetchRecord(Guid publicationId)
		{
			var recordQuery = (from dbRecord in _db.EventPublicationRecords
			                   where dbRecord.Id == publicationId
			                   select dbRecord);

			var record = recordQuery.SingleOrDefault();

			if (record == null)
			{
				throw new Exception(string.Format("Unable to locate record {0}.", publicationId));
			}
			return record;
		}
	}
}
