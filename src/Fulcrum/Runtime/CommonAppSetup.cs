using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fulcrum.Core;
using Fulcrum.Core.Ddd;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.CommandPipeline;
using Fulcrum.Runtime.EventPipeline;

namespace Fulcrum.Runtime
{
	namespace TiltedGlobe.Runtime
	{
		public class CommonAppSetup
		{
			public static void ConfigureCommandsAndHandlers(IWindsorContainer container)
			{
				var commandType = typeof(ICommand);
				var allCommands = AppDomain.CurrentDomain.GetAssemblies()
				                           .SelectMany(s => s.GetTypes())
				                           .Where(p => commandType.IsAssignableFrom(p)
				                                       && p.IsClass
				                                       && !p.IsAbstract
				                                       && !p.IsInterface);

				CommandLocator commandLocator = null;

				foreach (var command in allCommands)
				{
					if (commandLocator == null)
					{
						commandLocator = new CommandLocator
							(command.Assembly, command.Namespace);
					}
					else
					{
						commandLocator.AddCommandSource
							(command.Assembly, command.Namespace);
					}
				}

				container.Register(
					Component.For<ICommandLocator>()
					         .UsingFactoryMethod(() => commandLocator)
					);

				container.Register(Component.For<CommandPublicationRegistry>()
				                            .ImplementedBy<CommandPublicationRegistry>());

				ModelBinders.Binders.Add(typeof(ICommand), new CommandModelBinder());

				var handlerType = typeof(ICommandHandler);
				var allHandlers = AppDomain.CurrentDomain.GetAssemblies()
				                           .SelectMany(s => s.GetTypes())
				                           .Where(p => handlerType.IsAssignableFrom(p) && p.IsClass);

				var handler = allHandlers.FirstOrDefault();

				if (handler != null)
				{
					var handlers = PipelineInstaller.InstallHandlers(container, handler.Assembly);

					container.Register(Component.For<IList<ICommandHandler>>()
					                            .Instance(handlers)
					                            .LifestyleSingleton());

					container.Register(Component.For<ICommandPipeline>()
					                            .ImplementedBy<SimpleCommandPipeline>()
					                            .LifestylePerWebRequest());
				}
				else
				{
					// TODO: log this?
				}
			}

			public static void ConfigureContainer<TDbContext, TAppSettings>(IWindsorContainer container)
				where TDbContext : DbContext where TAppSettings : AppSettings, new()
			{
				container.Register(
					Component.For<TDbContext>()
					         .LifestylePerWebRequest());

				container.Register(
					Component.For<TAppSettings>()
					         .Instance(new TAppSettings())
					         .LifestyleSingleton());

				container.Register(
					Component.For<CommandPipelineDbContext>()
					         .LifestylePerWebRequest());

				container.Register(Classes.FromAssemblyInThisApplication()
				                          .BasedOn<IQuery>()
				                          .LifestylePerWebRequest().WithServiceSelf().WithServiceAllInterfaces());

				container.Register(Classes.FromAssemblyInThisApplication()
				                          .BasedOn<IRepository>()
				                          .LifestylePerWebRequest().WithServiceSelf().WithServiceAllInterfaces());

				container.Register(Classes.FromAssemblyInThisApplication()
				                          .BasedOn<IDomainService>()
				                          .LifestylePerWebRequest().WithServiceSelf().WithServiceAllInterfaces());

				container.Register(Classes.FromAssemblyInThisApplication()
				                          .BasedOn<ICommonService>()
				                          .LifestylePerWebRequest().WithServiceSelf().WithServiceAllInterfaces());

				container.Register(Classes.FromAssemblyInThisApplication()
				                          .BasedOn<IBackgroundTask>()
				                          .LifestylePerWebRequest().WithServiceSelf().WithServiceAllInterfaces());

				container.Register(Component.For<IWindsorContainer>()
				                            .Instance(container));
			}

			public static void ConfigureEventsAndHandlers(IWindsorContainer container)
			{
				container.Register(Component.For<EventPublicationRegistry>()
				                            .ImplementedBy<EventPublicationRegistry>()
				                            .LifeStyle.Transient);

				IList<IEventHandler> handlers = new List<IEventHandler>();

				//handlers = PipelineInstaller.InstallEventHandlers(container, typeof(MyEvent).Assembly);

				container.Register(
					Component.For<IList<IEventHandler>>()
					         .Instance(handlers)
					);

				container.Register(Component.For<IEventPipeline>()
				                            .ImplementedBy<SimpleEventPipeline>()
				                            .LifeStyle.Transient);
			}

			public static void ConfigureQueries(IWindsorContainer container)
			{
				var queryType = typeof(IQuery);
				var allQueries = AppDomain.CurrentDomain.GetAssemblies()
				                          .SelectMany(s => s.GetTypes())
				                          .Where(p => queryType.IsAssignableFrom(p)
				                                      && p.IsClass
				                                      && !p.IsAbstract
				                                      && !p.IsInterface);

				QueryLocator queryLocator = null;

				foreach (var command in allQueries)
				{
					if (queryLocator == null)
					{
						queryLocator = new QueryLocator
							(command.Assembly, command.Namespace);
					}
					else
					{
						queryLocator.AddQuerySource
							(command.Assembly, command.Namespace);
					}
				}

				container.Register(
					Component.For<IQueryLocator>()
					         .UsingFactoryMethod(() => queryLocator)
					         .LifestylePerWebRequest()
					);
				;
			}

			public static void ScheduleRecurringTasks()
			{
				//http://docs.hangfire.io/en/latest/background-methods/performing-recurrent-tasks.html

				// clear exiting scheduled jobs prior to rescheduling to
				// avoid having multiple simultaneous instances
				//RecurringJob.RemoveIfExists("MyJob");

				//RecurringJob.AddOrUpdate<MyJob>
				//					("MyJob", i => i.Run(), Cron.Daily);
			}
		}
	}
}
