using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class CommandFailedResult : CommandCompleteOrPendingResult
	{
		public CommandFailedResult(ICommandPublicationRecord record) : base(record)
		{
			ErrorDetails = record.ErrorDetails;
			ErrorHeadline = record.ErrorHeadline;
		}

		public string ErrorDetails { get; private set; }

		public string ErrorHeadline { get; private set; }
	}
}
