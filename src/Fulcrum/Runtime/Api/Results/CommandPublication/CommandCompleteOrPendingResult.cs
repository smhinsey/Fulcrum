using System;
using System.Collections.Generic;
using System.Linq;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class CommandCompleteOrPendingResult
	{
		public CommandCompleteOrPendingResult(ICommandPublicationRecord record)
		{
			Created = record.Created;
			Id = record.Id;
			Status = record.Status;
			Updated = record.Updated;
			CommandName = record.PortableCommand.ClrTypeName;
			QueryReferences = record.QueryReferences;

			// TODO: this should probably be subsumed by QueryReferences, meaning the registry should
			// have an IQuery front end
			Links = new List<JsonLink>()
			{
				new JsonLink(string.Format("/api/commands/publication-registry/{0}", Id), "publication-record"),
			};

			foreach (var reference in QueryReferences)
			{
				if(reference.Parameters == null)
				{
					reference.Parameters = new List<QueryReferenceParameter>();
				}

				var queryUrl = string.Format("/api/queries/{0}/results?id={1}",
					reference.QueryName, string.Join("&amp;", reference.Parameters.Any() ?
					reference.Parameters.Select(x => x.Name + "=" + x.Value).ToArray() : new[] {""} ));

				Links.Add(new JsonLink(queryUrl, "query-reference"));
			}
		}

		public string CommandName { get; set; }

		public DateTime Created { get; private set; }

		public Guid Id { get; private set; }

		public IList<JsonLink> Links { get; private set; }

		public IList<ParameterizedQueryReference> QueryReferences { get; private set; }

		public CommandPublicationStatus Status { get; private set; }

		public DateTime? Updated { get; private set; }
	}
}
