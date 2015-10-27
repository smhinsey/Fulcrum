namespace Fulcrum.Core
{
	public enum CommandPublicationStatus
	{
		Failed = -1,

		Unpublished = 0,

		Processing = 1,

		Complete = 2,

		WaitingOnJob = 3,
	}
}
