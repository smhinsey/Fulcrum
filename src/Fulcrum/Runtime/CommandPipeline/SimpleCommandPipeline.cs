using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Elmah;
using Fulcrum.Common;
using Fulcrum.Core;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class SimpleCommandPipeline : ICommandPipeline, ILoggingSource
	{
		private readonly IWindsorContainer _container;

		private readonly CommandPipelineDbContext _db;

		private readonly IList<ICommandHandler> _installedHandlers;

		public SimpleCommandPipeline(IWindsorContainer container, IList<ICommandHandler> installedHandlers,
			CommandPipelineDbContext db)
		{
			_container = container;
			_installedHandlers = installedHandlers;
			_db = db;
		}

		public ICommandPublicationRecord Inquire(Guid publicationId)
		{
			var recordQuery = (from dbRecord in _db.CommandPublicationRecords
			                   where dbRecord.Id == publicationId
			                   select dbRecord);

			recordQuery = recordQuery.Include(r => r.QueryReferences);

			return recordQuery.SingleOrDefault();
		}

		public ICommandPublicationRecord MarkAsComplete(Guid publicationId)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = CommandPublicationStatus.Complete;
			record.Updated = DateTime.UtcNow;

			_db.SaveChanges();

			return record;
		}

		public ICommandPublicationRecord MarkAsFailed(Guid publicationId, string errorHeadline, Exception exception)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = CommandPublicationStatus.Failed;
			record.Updated = DateTime.UtcNow;

			record.ErrorHeadline = errorHeadline;
			record.ErrorDetails = exception.ToString();

			_db.SaveChanges();

			return record;
		}

		public ICommandPublicationRecord MarkAsProcessing(Guid publicationId)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = CommandPublicationStatus.Processing;
			record.Updated = DateTime.UtcNow;

			_db.SaveChanges();

			return record;
		}

		public ICommandPublicationRecord MarkAsWaitingOnJob(Guid publicationId)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = CommandPublicationStatus.WaitingOnJob;
			record.Updated = DateTime.UtcNow;

			_db.SaveChanges();

			return record;
		}

		public ICommandPublicationRecord Publish(ICommand command)
		{
			command.AssignPublicationRecordId(Guid.NewGuid());

			// TODO: add support for encrypted fields to support things like passwords, SSNs, credit cards, etc.
			// TODO: move this to CommandPublicationRegistry
			var publicationRecord = new CommandPublicationRecord(command);

			_db.CommandPublicationRecords.Add(publicationRecord);

			_db.SaveChanges();

			// Should this be done via strategy? We'll need a separate set of interfaces
			// for an async pipeline. What other types of executors are realistic? A pooled
			// executor would likely require a custom ICommandPipeline implementation in order
			// to manage its own lifecycle, so it couldn't be done this way. Could anything?

			// Should we really allow more than one handler per command?
			foreach (var installedHandler in _installedHandlers)
			{
				const string HandleMethodName = "Handle";

				var potentialHandlerMethods = installedHandler.GetType().GetMethods();

				var handlerMethods = potentialHandlerMethods
					.Where(m => m.Name == HandleMethodName);

				var configuredHandlerForThisCommand = handlerMethods
					.Any(m => m.GetParameters().ToList().Any(p => p.ParameterType == command.GetType()));

				if (!configuredHandlerForThisCommand)
				{
					continue;
				}

				MarkAsProcessing(publicationRecord.Id);

				try
				{
					var resolvedHandler = _container.Resolve(installedHandler.GetType());

					var handlerType = resolvedHandler.GetType();

					var handler = handlerType.GetMethod(HandleMethodName, new[] { command.GetType() });

					// TODO: async could go here
					handler.Invoke(resolvedHandler, new object[] { command });

					var latestRecord = safelyFetchRecord(publicationRecord.Id);

					// don't complete commands which are waiting on jobs
					if (latestRecord.Status != CommandPublicationStatus.WaitingOnJob)
					{
						publicationRecord = (CommandPublicationRecord)MarkAsComplete(publicationRecord.Id);
					}
					else
					{
						publicationRecord = latestRecord;
					}

					// this call is unnecessary as Windsor ignores manual releases
					// of PerWebRequest lifestyle components
					//_container.Release(resolvedHandler);

					var disposable = resolvedHandler as IDisposable;

					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				catch (Exception ex)
				{
					// the outer exception is always going to be TargetInvocationException
					var appException = ex.InnerException;

					// TODO: derive a better headline from the particular exception
					publicationRecord = (CommandPublicationRecord)MarkAsFailed(publicationRecord.Id, appException.Message, appException);

					if (HttpContext.Current != null)
					{
						var fromCurrentContext = ErrorSignal.FromCurrentContext();
						fromCurrentContext.Raise(appException);
					}
				}
			}

			this.LogInfo("Published command {0}, view details at {1}commands/publication-registry/{2}.",
				command.GetType().Name, ConfigurationSettings.AppSettings["ApiBaseUrl"], publicationRecord.Id);

			return safelyFetchRecord(publicationRecord.Id);
		}

		// TODO: delegate all usages to CommandPublicationRegistry
		private CommandPublicationRecord safelyFetchRecord(Guid publicationId)
		{
			var recordQuery = (from dbRecord in _db.CommandPublicationRecords
			                   where dbRecord.Id == publicationId
			                   select dbRecord);

			recordQuery = recordQuery.Include(r => r.QueryReferences);

			var record = recordQuery.SingleOrDefault();

			if (record == null)
			{
				throw new Exception(string.Format("Unable to locate record {0}.", publicationId));
			}
			return record;
		}
	}
}
