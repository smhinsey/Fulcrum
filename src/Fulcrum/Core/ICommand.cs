using System;

namespace Fulcrum.Core
{
	/// <summary>
	///   Represents a discrete, logical operation which modifies the state of the system which
	///   handles it.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		///   When a command has been published, it is assigned a publication record
		///   with a unique identifier. This identifier is returned to the publisher
		///   and made available to the command's handler so that the two can communicate
		///   out-of-band via the command pipeline inquiry system.
		/// </summary>
		Guid PublicationRecordId { get; }

		// NOTE: A better design is possible here
		// Andromeda provides an example of doing this the "right" way, however, that approach
		// entails more complexity and beahvior. Despite being somewhat cheesy, this approach
		// is simple and sufficient.
		void AssignPublicationRecordId(Guid id);
	}
}
