namespace Fulcrum.Core
{
	public class CommandValidationMessage
	{
		public CommandValidationMessage(string message, string propertyName)
		{
			PropertyName = propertyName;
			Message = message;
		}

		public string Message { get; private set; }

		public string PropertyName { get; private set; }
	}
}