using System;
using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;
using Newtonsoft.Json;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class DetailedPublicationRecordResult
	{
		public DetailedPublicationRecordResult(ICommandPublicationRecord record)
		{
			Created = record.Created;
			Id = record.Id;
			Status = record.Status;
			Updated = record.Updated;
			CommandName = record.PortableCommand.ClrTypeName;
			Links = new List<JsonLink>()
			{
				new JsonLink("/api/commands/", "home")
			};
			OriginalCommand = JsonConvert.DeserializeObject(record.PortableCommand.CommandJson);
			CommandSchema = JsonConvert.DeserializeObject(record.PortableCommand.CommandJsonSchema);
			ErrorDetails = record.ErrorDetails;
			ErrorHeadline = record.ErrorHeadline;

			foreach (var reference in record.QueryReferences)
			{
				var queryUrl = string.Format("/api/queries/{0}/results?id={1}", 
					reference.QueryName, reference.QueryParameter);

				Links.Add(new JsonLink(queryUrl,"query-reference"));
			}
		}

		public string CommandName { get; set; }

		public object CommandSchema { get; private set; }

		public DateTime Created { get; private set; }

		public Guid Id { get; private set; }

		public IList<JsonLink> Links { get; private set; }

		public object OriginalCommand { get; private set; }

		public CommandPublicationStatus Status { get; private set; }

		public DateTime? Updated { get; private set; }

		public string ErrorDetails { get; private set; }

		public string ErrorHeadline { get; private set; }
	}
}
