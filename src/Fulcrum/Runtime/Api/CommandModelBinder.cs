using System;
using System.Collections;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Fulcrum.Common;

namespace Fulcrum.Runtime.Api
{
	// TODO: this is in need of a major rewrite. 
	// best case, this can just delegate to existing code within MVC or Web API, but at the very least
	// we should introduce something like a type dictionary dispatcher to reduce code duplication, etc.
	public class CommandModelBinder : DefaultModelBinder, ILoggingSource
	{
		private const int MaxRecursionDepth = 25;

		public CommandModelBinder()
		{
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var serviceLocator = GlobalConfiguration.Configuration.DependencyResolver.BeginScope();

			var commandLocator = serviceLocator.GetService(typeof(ICommandLocator)) as ICommandLocator;

			var inNamespaceResult = bindingContext.ValueProvider.GetValue("inNamespace");
			var nameResult = bindingContext.ValueProvider.GetValue("commandName");

			if (inNamespaceResult != null && nameResult != null)
			{
				var commandNamespace = inNamespaceResult.AttemptedValue;
				var commandName = nameResult.AttemptedValue;

				var commandType = commandLocator.Find(commandName, commandNamespace);

				if (commandType == null)
				{
					var msg = string.Format("Unable to locate command {0}.{1}", commandNamespace, commandName);
					throw new Exception(msg);
				}

				var command = Activator.CreateInstance(commandType);

				var commandProperties = command.GetType().GetProperties();

				foreach (var property in commandProperties)
				{
					if (property.CanWrite)
					{
						var value = bindingContext.ValueProvider.GetValue(property.Name);

						if (value == null)
						{
							if (property.PropertyType.IsArray)
							{
								bindArrayProperty(bindingContext, property, command, 0);
							}
							else
							{
								var errorMessage = string.Format("Property '{0}' missing from command.", property.Name);

								bindingContext.ModelState.AddModelError(property.Name, errorMessage);
							}
						}
						else
						{
							try
							{
								if (property.PropertyType.IsEnum)
								{
									property.SetValue(command, Enum.Parse(property.PropertyType, value.RawValue.ToString()));
								}
								else if (property.PropertyType == typeof(DateTime))
								{
									property.SetValue(command, DateTime.Parse(value.RawValue.ToString()));
								}
								else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
								{
									var rawValue = value.RawValue;

									var input = "";

									if (rawValue != null)
									{
										input = rawValue.ToString();
									}

									var parsedGuid = !string.IsNullOrWhiteSpace(input) ? Guid.Parse(input) : new Guid?();

									property.SetValue(command, parsedGuid);
								}
								else if (property.PropertyType == typeof(DateTime?))
								{
									object val = null;

									if (value.RawValue != null)
									{
										val = DateTime.Parse(value.RawValue.ToString());
									}

									property.SetValue(command, val);
								}
								else if (property.PropertyType == typeof(Decimal) || property.PropertyType == typeof(Decimal?))
								{
									if (value.RawValue == null || string.IsNullOrEmpty(value.RawValue.ToString()))
									{
										property.SetValue(command, new decimal?());
									}
									else
									{
										property.SetValue(command, Decimal.Parse(value.RawValue.ToString()));
									}
								}
								else
								{
									property.SetValue(command, value.RawValue);
								}

								//var typeDescriptor = TypeDescriptor.GetProperties(commandType);

								//var propertyDescriptor = typeDescriptor.Find(property.Name, true);

								//var val = GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, new DefaultModelBinder());

								//SetProperty(controllerContext, bindingContext, propertyDescriptor, val);
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

		private void bindArrayProperty
			(ModelBindingContext bindingContext, PropertyInfo property, object bindingTarget,
				int currentDepth, string parentPath = "")
		{
			var elementType = property.PropertyType.GetElementType();
			var values = new ArrayList();
			var lookingForMore = true;
			var currentIndex = 0;

			while (lookingForMore)
			{
				if (property.PropertyType.IsClass
				    && elementType != typeof(string)
				    && elementType != typeof(int)
				    && elementType != typeof(bool)
				    && elementType != typeof(DateTime)
					)
				{
					var instance = Activator.CreateInstance(elementType);

					var instanceProps = instance.GetType().GetProperties();

					var missingKeyCount = 1;
					var propCount = instanceProps.Length;

					foreach (var prop in instanceProps)
					{
						var name = string.Format("{0}[{1}].{2}",
							property.Name, currentIndex, prop.Name);

						if (!string.IsNullOrWhiteSpace(parentPath))
						{
							name = string.Format("{0}[{1}].{2}", parentPath, currentIndex, prop.Name);
						}

						//this.LogDebug("Trying to bind property {0}.", name);

						if (prop.CanWrite && !prop.PropertyType.IsArray)
						{
							//this.LogDebug("Property is writable and not an array.");

							var localValue = bindingContext.ValueProvider.GetValue(name);

							var keyExists = bindingContext.ValueProvider.ContainsPrefix(name);

							if (keyExists && localValue != null)
							{
								if (prop.PropertyType == typeof(int?))
								{
									if (!string.IsNullOrEmpty(localValue.AttemptedValue))
									{
										var coercedValue = int.Parse(localValue.AttemptedValue);

										prop.SetValue(instance, coercedValue);
									}
								}
								else if (prop.PropertyType == typeof(decimal?))
								{
									if (!string.IsNullOrEmpty(localValue.AttemptedValue))
									{
										var coercedValue = decimal.Parse(localValue.AttemptedValue);

										prop.SetValue(instance, coercedValue);
									}
								}
								else if (prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(Guid?))
								{
									if (!string.IsNullOrWhiteSpace(localValue.AttemptedValue))
									{
										var coercedValue = Guid.Parse(localValue.AttemptedValue);

										prop.SetValue(instance, coercedValue);
									}
								}
								else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
								{
									if (!string.IsNullOrWhiteSpace(localValue.AttemptedValue))
									{
										var coercedValue = DateTime.Parse(localValue.AttemptedValue);

										prop.SetValue(instance, coercedValue);
									}
								}
								else
								{
									var coercedValue = Convert.ChangeType
										(localValue.AttemptedValue, prop.PropertyType);

									prop.SetValue(instance, coercedValue);
								}
							}
							else
							{
								if (!keyExists)
								{
									missingKeyCount++;
								}

								if (propCount + 1 == missingKeyCount)
								{
									lookingForMore = false;
								}
							}
						}
						else if (prop.PropertyType.IsArray)
						{
							//this.LogDebug("Property is array.");

							bindArrayProperty(bindingContext, prop, instance, currentDepth++, name);
						}
					}

					if (propCount == missingKeyCount)
					{
						// don't add the value if all keys are missing
						break;
					}

					values.Add(instance);

					if (lookingForMore)
					{
						currentIndex++;
					}
				}
				else
				{
					var name = string.Format("{0}[{1}]", property.Name, currentIndex);

					if (!string.IsNullOrEmpty(parentPath))
					{
						var fixedParentPath = Char.ToLowerInvariant(parentPath[0]) + parentPath.Substring(1);

						if (!string.IsNullOrWhiteSpace(fixedParentPath))
						{
							name = string.Format("{0}[{1}]", fixedParentPath, currentIndex);
						}
					}

					//this.LogDebug("Trying to bind array property {0}.", name);

					var arrayValue = bindingContext.ValueProvider.GetValue(name);

					currentIndex++;

					//this.LogDebug("arrayValue: {0}.", arrayValue);

					if (arrayValue == null || string.IsNullOrEmpty(arrayValue.AttemptedValue))
					{
						lookingForMore = false;

						break;
					}

					values.Add(Convert.ChangeType(arrayValue.AttemptedValue, elementType));
				}
			}

			if (values.Count > 0)
			{
				try
				{
					property.SetValue(bindingTarget, values.ToArray(elementType));
				}
				catch (Exception e)
				{
					var errorMessage = string.Format("Error setting command array property '{0}':{1}",
						property.Name, e.Message);

					bindingContext.ModelState.AddModelError(property.Name, errorMessage);
				}
			}
		}
	}
}
