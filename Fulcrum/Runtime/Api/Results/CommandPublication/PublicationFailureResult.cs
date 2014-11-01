using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class PublicationFailureResult : PublicationImmediateResult
	{
		public PublicationFailureResult(ICommandPublicationRecord record) : base(record)
		{
			ErrorDetails = record.ErrorDetails;
			ErrorHeadline = record.ErrorHeadline;
		}

		public string ErrorDetails { get; private set; }

		public string ErrorHeadline { get; private set; }
	}
}