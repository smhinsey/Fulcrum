using System;

namespace Fulcrum.Core
{
	public abstract class Error : Exception
	{
		protected Error(ICommand relatedCommand)
		{
			RelatedCommand = relatedCommand;
		}

		public ICommand RelatedCommand { get; private set; }
	}
}
