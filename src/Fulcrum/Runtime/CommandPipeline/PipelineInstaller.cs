using System.Collections.Generic;
using System.Reflection;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fulcrum.Core;

namespace Fulcrum.Runtime.CommandPipeline
{
	public static class PipelineInstaller
	{
		public static IList<ICommandHandler> InstallHandlers
			(IWindsorContainer targetContainer, params Assembly[] assembliesToScan)
		{
			targetContainer.BeginScope();

			IList<ICommandHandler> installedHandlers = new List<ICommandHandler>();

			if (assembliesToScan.Length == 0)
			{
				assembliesToScan = new[] { Assembly.GetExecutingAssembly() };
			}

			foreach (var assembly in assembliesToScan)
			{
				targetContainer.Register(
					Classes.FromAssembly(assembly)
						.BasedOn(typeof(ICommandHandler))
						.LifestylePerWebRequest()
						.WithServiceAllInterfaces()
						.WithServiceSelf());
			}

			var handlers = targetContainer.ResolveAll<ICommandHandler>();

			foreach (var handler in handlers)
			{
				installedHandlers.Add(handler);
			}

			targetContainer.Release(handlers);

			return installedHandlers;
		}

        public static IList<IEventHandler> InstallEventHandlers
            (IWindsorContainer targetContainer, params Assembly[] assembliesToScan)
        {
            targetContainer.BeginScope();

            IList<IEventHandler> installedHandlers = new List<IEventHandler>();

            if (assembliesToScan.Length == 0)
            {
                assembliesToScan = new[] { Assembly.GetExecutingAssembly() };
            }

            foreach (var assembly in assembliesToScan)
            {
                targetContainer.Register(
                    Classes.FromAssembly(assembly)
                        .BasedOn(typeof(IEventHandler))
                        .LifestyleTransient()
                        .WithServiceAllInterfaces()
                        .WithServiceSelf());
            }

            var handlers = targetContainer.ResolveAll<IEventHandler>();

            foreach (var handler in handlers)
            {
                installedHandlers.Add(handler);
            }

            targetContainer.Release(handlers);

            return installedHandlers;
        }
	}

}
