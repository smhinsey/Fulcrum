using System;

namespace Fulcrum.Core
{
	/// <summary>
	///   A command pipeline implementation is responsible for
	/// </summary>
	public interface ICommandPipeline
	{
		/// <summary>
		///   Returns the current publication record.
		/// </summary>
		/// <param name="publicationId"></param>
		/// <returns></returns>
		ICommandPublicationRecord Inquire(Guid publicationId);

		ICommandPublicationRecord MarkAsComplete(Guid publicationId);

		ICommandPublicationRecord MarkAsFailed(Guid publicationId, string errorHeadline, Exception exception);

		ICommandPublicationRecord MarkAsProcessing(Guid publicationId);

		ICommandPublicationRecord MarkAsWaitingOnJob(Guid publicationId);

		ICommandPublicationRecord Publish(ICommand command);
	}
}
