using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

// based on http://cangencer.wordpress.com/2012/12/22/integrating-asp-net-web-api-with-castle-windsor/

namespace Fulcrum.Common.Web
{
	public class WindsorDependencyScope : IDependencyScope
	{
		private readonly IKernel _container;

		private readonly IDisposable _scope;

		public WindsorDependencyScope(IKernel container)
		{
			_container = container;
			_scope = container.BeginScope();
		}

		public void Dispose()
		{
			_scope.Dispose();
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
