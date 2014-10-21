using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fulcrum.Core
{
	public class CommandValidationResult
	{
		public CommandValidationResult(bool isCommandValid, IList<CommandValidationMessage> validationMessages)
		{
			ValidationMessages = new ReadOnlyCollection<CommandValidationMessage>(validationMessages);
			IsCommandValid = isCommandValid;
		}

		public CommandValidationResult(bool isCommandValid) :
			this(isCommandValid, new List<CommandValidationMessage>())
		{
		}

		public bool IsCommandValid { get; private set; }

		public IReadOnlyList<CommandValidationMessage> ValidationMessages { get; private set; }
	}
}
