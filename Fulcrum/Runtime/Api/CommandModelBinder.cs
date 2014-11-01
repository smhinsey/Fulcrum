using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Fulcrum.Runtime.Api
{
	public class CommandModelBinder : IModelBinder
	{
		public CommandModelBinder()
		{
		}

		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			var serviceLocator = GlobalConfiguration.Configuration.DependencyResolver.BeginScope();

			var commandLocator = serviceLocator.GetService(typeof(ICommandLocator)) as ICommandLocator;

			var inNamespaceResult = bindingContext.ValueProvider.GetValue("inNamespace");
			var nameResult = bindingContext.ValueProvider.GetValue("name");

			if (inNamespaceResult != null && nameResult != null)
			{
				var commandNamespace = inNamespaceResult.AttemptedValue;
				var commandName = nameResult.AttemptedValue;

				var commandType = commandLocator.FindInNamespace(commandName, commandNamespace);

				if (commandType != null)
				{
					var command = Activator.CreateInstance(commandType);

					bindingContext.Model = command;

					return true;
				}
			}
			return false;
		}
	}
}
