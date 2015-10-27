using System;

namespace Fulcrum.Core
{
	/// <summary>
	///   An event pipeline implementation is responsible for
	/// </summary>
	public interface IEventPipeline
	{
		/// <summary>
		///   Prevent the publication of events.
		/// </summary>
		void DisablePublication();

		/// <summary>
		///   Allow the publication of events.
		/// </summary>
		void EnablePublication();

		/// <summary>
		///   Returns the current publication record.
		/// </summary>
		/// <param name="publicationId"></param>
		/// <returns></returns>
		IEventPublicationRecord Inquire(Guid publicationId);

		IEventPublicationRecord MarkAsComplete(Guid publicationId);

		IEventPublicationRecord MarkAsFailed(Guid publicationId, string errorHeadline, Exception exception);

		IEventPublicationRecord MarkAsProcessing(Guid publicationId);

		IEventPublicationRecord MarkAsWaitingOnJob(Guid publicationId);

		IEventPublicationRecord Publish(IEvent ev);
	}
}
