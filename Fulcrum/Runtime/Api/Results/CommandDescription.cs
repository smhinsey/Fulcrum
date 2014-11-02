using System;
using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results
{
	public class CommandDescription : CommandDescriptor
	{
		public CommandDescription(Type commandType, bool includeDetails) : base(commandType.Name, commandType.Namespace)
		{
			EmptyModel = Activator.CreateInstance(commandType);

			var commandSchema = CommandSchemaGenerator.GenerateSchema(commandType);

			Schema = new SchemaObject(commandSchema, Name, Namespace);
			
			Links = new List<JsonLink>()
			{
				new JsonLink(string.Format("commands/{0}/{1}/publish", Namespace, Name), "publication")
			};

			if (includeDetails)
			{
				Links.Add(new JsonLink(string.Format("commands/{0}/{1}", Namespace, Name), "details"));
			}
		}

		public List<JsonLink> Links { get; private set; }

		public object EmptyModel { get; private set; }

		public SchemaObject Schema { get; private set; }
	}
}
