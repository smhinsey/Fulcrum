using System;
using System.Web.Http;
using System.Web.Mvc;

namespace Fulcrum.Runtime.Api
{
	public class CommandModelBinder : DefaultModelBinder
	{
		public CommandModelBinder()
		{
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
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

				var command = Activator.CreateInstance(commandType);

				var commandProperties = command.GetType().GetProperties();

				foreach (var property in commandProperties)
				{
					if (property.CanWrite)
					{
						var value = bindingContext.ValueProvider.GetValue(property.Name);

						if (value == null)
						{
							var errorMessage = string.Format("Property '{0}' missing from command.", property.Name);

							bindingContext.ModelState.AddModelError(property.Name, errorMessage);
						}
						else
						{
							try
							{
								property.SetValue(command, value.RawValue);
							}
							catch (Exception e)
							{
								var errorMessage = string.Format("Error setting command property '{0}':{1}", 
									property.Name, e.Message);

								bindingContext.ModelState.AddModelError(property.Name, errorMessage);
							}
						}
					}
				}

				return command;
			}
			return null;
		}
	}
}
