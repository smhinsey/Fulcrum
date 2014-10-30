using System;
using System.Collections.Generic;
using Fulcrum.Core;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class CommandPublicationRecord : ICommandPublicationRecord
	{
		public CommandPublicationRecord(ICommand command)
		{
			Status = CommandPublicationStatus.Unpublished;
			Id = command.PublicationRecordId;
			PortableCommand = new PortableCommand(command, CommandSchemaGenerator.GenerateSchema(command.GetType()));
			RelatedModelIds = new Dictionary<string, Guid>();
			Created = DateTime.UtcNow;
		}

		public DateTime Created { get; private set; }

		public string ErrorDetails { get; set; }

		public string ErrorHeadline { get; set; }

		public DateTime? Updated { get; set; }

		public Guid Id { get; private set; }

		public PortableCommand PortableCommand { get; private set; }

		// TODO: figure out what this really needs to look like
		public IDictionary<string, Guid> RelatedModelIds { get; private set; }

		public CommandPublicationStatus Status { get; set; }
	}
}
