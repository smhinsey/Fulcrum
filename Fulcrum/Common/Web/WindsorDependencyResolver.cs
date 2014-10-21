using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

// based on http://cangencer.wordpress.com/2012/12/22/integrating-asp-net-web-api-with-castle-windsor/
namespace Fulcrum.Common.Web
{
	public class WindsorDependencyResolver : IDependencyResolver
	{
		private readonly IKernel _container;

		public WindsorDependencyResolver(IKernel container)
		{
			_container = container;
		}

		public IDependencyScope BeginScope()
		{
			return new WindsorDependencyScope(_container);
		}

		public void Dispose()
		{
		}

		public object GetService(Type serviceType)
		{
			return _container.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _container.ResolveAll(serviceType).Cast<object>();
		}
	}
}
