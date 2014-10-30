using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

		public SimpleCommandPipeline(IWindsorContainer container)
		{
			_container = container;
			_installedHandlers = new List<ICommandHandler>();

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

		public void InstallHandlers(IWindsorContainer targetContainer, params Assembly[] assembliesToScan)
		{
			if (assembliesToScan.Length == 0)
			{
				assembliesToScan = new[] { Assembly.GetExecutingAssembly() };
			}

			foreach (var assembly in assembliesToScan)
			{
				targetContainer.Register(
					Classes.FromAssembly(assembly)
						.BasedOn(typeof(ICommandHandler))
						.WithServiceAllInterfaces()
						.WithServiceSelf());
			}

			var handlers = _container.ResolveAll<ICommandHandler>();

			foreach (var handler in handlers)
			{
				_installedHandlers.Add(handler);
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

			var result = new CommandPublicationRecord(command);

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
					try
					{
						var resolvedHandler = _container.Resolve(installedHandler.GetType());

						var handlerType = resolvedHandler.GetType();

						var handler = handlerType.GetMethod("Handle", new[] { command.GetType() });

						// TODO: invoke on a task, wait for the results, and update the record appropriately
						handler.Invoke(resolvedHandler, new object[] { command });
					}
					catch (Exception ex)
					{
						MarkAsFailed(result.Id, "Unrecoverable command processing error", ex);
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
