using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Fulcrum.Core;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class CommandPublicationRecord : ICommandPublicationRecord
	{
		public CommandPublicationRecord()
		{
		}

		public CommandPublicationRecord(ICommand command)
		{
			Status = CommandPublicationStatus.Unpublished;
			Id = command.PublicationRecordId;
			PortableCommand = new PortableCommand(command, CommandSchemaGenerator.GenerateSchema(command.GetType()));
			Created = DateTime.UtcNow;
			QueryReferences = new EditableList<ParameterizedQueryReference>();
		}

		public DateTime Created { get; private set; }

		public string ErrorDetails { get; set; }

		public string ErrorHeadline { get; set; }

		public PortableCommand PortableCommand { get; private set; }

		public IList<ParameterizedQueryReference> QueryReferences { get; set; }

		public CommandPublicationStatus Status { get; set; }

		public DateTime? Updated { get; set; }

		public Guid Id { get; private set; }
	}
}
