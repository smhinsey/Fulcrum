using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Castle.Windsor;
using Elmah;
using Fulcrum.Common;
using Fulcrum.Core;
using Fulcrum.Runtime.CommandPipeline;

namespace Fulcrum.Runtime.EventPipeline
{
	public class SimpleEventPipeline : IEventPipeline, ILoggingSource
	{
		private readonly IWindsorContainer _container;

		private readonly CommandPipelineDbContext _db;

		private readonly IList<IEventHandler> _installedHandlers;

		private bool _enabled;

		public SimpleEventPipeline(IWindsorContainer container, IList<IEventHandler> installedHandlers,
			CommandPipelineDbContext db)
		{
			_container = container;
			_installedHandlers = installedHandlers;
			_db = db;
		}

		public void DisablePublication()
		{
			_enabled = false;
		}

		public void EnablePublication()
		{
			_enabled = true;
		}

		public IEventPublicationRecord Inquire(Guid publicationId)
		{
			var recordQuery = (from dbRecord in _db.EventPublicationRecords
			                   where dbRecord.Id == publicationId
			                   select dbRecord);

			recordQuery = recordQuery.Include(r => r.QueryReferences);

			return recordQuery.SingleOrDefault();
		}

		public IEventPublicationRecord MarkAsComplete(Guid publicationId)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = EventPublicationStatus.Complete;
			record.Updated = DateTime.UtcNow;

			_db.SaveChanges();

			return record;
		}

		public IEventPublicationRecord MarkAsFailed(Guid publicationId, string errorHeadline, Exception exception)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = EventPublicationStatus.Failed;
			record.Updated = DateTime.UtcNow;

			record.ErrorHeadline = errorHeadline;
			record.ErrorDetails = exception.ToString();

			_db.SaveChanges();

			return record;
		}

		public IEventPublicationRecord MarkAsProcessing(Guid publicationId)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = EventPublicationStatus.Processing;
			record.Updated = DateTime.UtcNow;

			_db.SaveChanges();

			return record;
		}

		public IEventPublicationRecord MarkAsWaitingOnJob(Guid publicationId)
		{
			var record = safelyFetchRecord(publicationId);

			record.Status = EventPublicationStatus.WaitingOnJob;
			record.Updated = DateTime.UtcNow;

			_db.SaveChanges();

			return record;
		}

		public IEventPublicationRecord Publish(IEvent ev)
		{
			ev.AssignPublicationRecordId(Guid.NewGuid());

			var publicationRecord = new EventPublicationRecord(ev);

			_db.EventPublicationRecords.Add(publicationRecord);

			_db.SaveChanges();

			// Should this be done via strategy? We'll need a separate set of interfaces
			// for an async pipeline. What other types of executors are realistic? A pooled
			// executor would likely require a custom IEventPipeline implementation in order
			// to manage its own lifecycle, so it couldn't be done this way. Could anything?

			// Should we really allow more than one handler per event?

			List<Task> tasks = new List<Task>();

			foreach (var installedHandler in _installedHandlers)
			{
				const string HandleMethodName = "Handle";

				var potentialHandlerMethods = installedHandler.GetType().GetMethods();

				var handlerMethods = potentialHandlerMethods
					.Where(m => m.Name == HandleMethodName);

				var configuredHandlerForThisEvent = handlerMethods
					.Any(m => m.GetParameters().ToList().Any(p => p.ParameterType == ev.GetType()));

				if (!configuredHandlerForThisEvent)
				{
					continue;
				}

				MarkAsProcessing(publicationRecord.Id);

				try
				{
					var resolvedHandler = _container.Resolve(installedHandler.GetType());

					var handlerType = resolvedHandler.GetType();

					var handler = handlerType.GetMethod(HandleMethodName, new[] { ev.GetType() });

					var logSessionId = this.LogGetSessionId();

					var task = Task.Run(() =>
					                 {

														 if (logSessionId.HasValue)
														 {
															 this.LogSetSessionId(logSessionId.Value);
														 }

						                 try
						                 {
							                 handler.Invoke(resolvedHandler, new object[] { ev });
						                 }
						                 finally
						                 {
															 if (logSessionId.HasValue)
															 {
																 this.LogClearSessionId();
															 }
						                 }
				
					                 });

					tasks.Add(task);

					var latestRecord = safelyFetchRecord(publicationRecord.Id);

					// don't complete event which are waiting on jobs
					if (latestRecord.Status != EventPublicationStatus.WaitingOnJob)
					{
						publicationRecord = (EventPublicationRecord)MarkAsComplete(publicationRecord.Id);
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
					publicationRecord = (EventPublicationRecord)MarkAsFailed(publicationRecord.Id, appException.Message, appException);

					if (HttpContext.Current != null)
					{
						var fromCurrentContext = ErrorSignal.FromCurrentContext();
						fromCurrentContext.Raise(appException);
					}
					else
					{
						throw;
					}
				}
			}

			// wait for the tasks to finish and log their status
			Task.WaitAll(tasks.ToArray());

			foreach (Task t in tasks)
			{
				this.LogInfo("Task {0} Status: {1}, publication-registry/{2}", t.Id, t.Status, publicationRecord.Id);
			}

			this.LogInfo("Published event {0}, view details at {1}events/publication-registry/{2}.",
				ev.GetType().Name, ConfigurationSettings.AppSettings["ApiBaseUrl"], publicationRecord.Id);

			return safelyFetchRecord(publicationRecord.Id);
		}

		// TODO: delegate all usages to EventPublicationRegistry
		private EventPublicationRecord safelyFetchRecord(Guid publicationId)
		{
			var recordQuery = (from dbRecord in _db.EventPublicationRecords
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
