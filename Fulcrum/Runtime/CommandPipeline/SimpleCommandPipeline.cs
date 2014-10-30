using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fulcrum.Core;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class SimpleCommandPipeline : ICommandPipeline
	{
		private readonly IWindsorContainer _container;

		private readonly IList<ICommandHandler> _installedHandlers;

		private bool _enabled;

		public SimpleCommandPipeline(IWindsorContainer container, IList<ICommandHandler> installedHandlers)
		{
			_container = container;
			_installedHandlers = installedHandlers;

			_container.Register(Component.For<SimpleCommandPipeline>().Instance(this));
		}

		public void DisablePublication()
		{
			_enabled = false;
		}

		public void EnablePublication()
		{
			_enabled = true;
		}

		public ICommandPublicationRecord Inquire(Guid publicationId)
		{
			using (var db = new CommandPipelineDbContext())
			{
				var recordQuery = (from dbRecord in db.CommandPublicationRecords
					where dbRecord.Id == publicationId
					select dbRecord);

				return recordQuery.SingleOrDefault();
			}
		}

		public ICommandPublicationRecord MarkAsComplete(Guid publicationId)
		{
			using (var db = new CommandPipelineDbContext())
			{
				var record = safelyFetchRecord(publicationId, db);

				record.Status = CommandPublicationStatus.Complete;
				record.Updated = DateTime.UtcNow;

				db.SaveChanges();

				return record;
			}
		}

		public ICommandPublicationRecord MarkAsFailed(Guid publicationId, string errorHeadline, Exception exception)
		{
			using (var db = new CommandPipelineDbContext())
			{
				var record = safelyFetchRecord(publicationId, db);

				record.Status = CommandPublicationStatus.Failed;
				record.Updated = DateTime.UtcNow;

				record.ErrorHeadline = errorHeadline;
				record.ErrorDetails = exception.ToString();

				db.SaveChanges();

				return record;
			}
		}

		public ICommandPublicationRecord MarkAsProcessing(Guid publicationId)
		{
			using (var db = new CommandPipelineDbContext())
			{
				var record = safelyFetchRecord(publicationId, db);

				record.Status = CommandPublicationStatus.Processing;
				record.Updated = DateTime.UtcNow;

				db.SaveChanges();

				return record;
			}
		}

		public ICommandPublicationRecord Publish(ICommand command)
		{
			if (!_enabled)
			{
				throw new Exception("Publication disabled");
			}

			command.AssignPublicationRecordId(Guid.NewGuid());

			var result = new CommandPublicationRecord(command);

			using (var db = new CommandPipelineDbContext())
			{
				db.CommandPublicationRecords.Add(result);

				db.SaveChanges();
			}

			// Should this be done via strategy? We'll need a separate set of interfaces
			// for an async pipeline. What other types of executors are realistic? A pooled
			// executor would likely require a custom ICommandPipeline implementation in order
			// to manage its own lifecycle, so it couldn't be done this way. Could anything?

			// TODO: consider one-handler-per-command
			foreach (var installedHandler in _installedHandlers)
			{
				// TODO: determine if referenceHandler can handle command
				if (true)
				{
					MarkAsProcessing(result.Id);

					try
					{
						var resolvedHandler = _container.Resolve(installedHandler.GetType());

						var handlerType = resolvedHandler.GetType();

						var handler = handlerType.GetMethod("Handle", new[] { command.GetType() });

						// TODO: invoke on a task, wait for the results, and update the record appropriately
						handler.Invoke(resolvedHandler, new object[] { command });

						//MarkAsComplete(result.Id);
					}
					catch (Exception ex)
					{
						// the outer exception is always going to be TargetInvocationException

						MarkAsFailed(result.Id, "Unrecoverable command processing error", ex.InnerException);

						throw ex.InnerException;
					}
				}
			}

			return result;
		}

		private CommandPublicationRecord safelyFetchRecord(Guid publicationId, CommandPipelineDbContext db)
		{
			var recordQuery = (from dbRecord in db.CommandPublicationRecords
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
