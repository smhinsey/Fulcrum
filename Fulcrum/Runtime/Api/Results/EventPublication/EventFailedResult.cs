using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.EventPublication
{
	public class EventFailedResult : EventCompleteOrPendingResult
	{
		public EventFailedResult(IEventPublicationRecord record) : base(record)
		{
			ErrorDetails = record.ErrorDetails;
			ErrorHeadline = record.ErrorHeadline;
		}

		public string ErrorDetails { get; private set; }

		public string ErrorHeadline { get; private set; }
	}
}