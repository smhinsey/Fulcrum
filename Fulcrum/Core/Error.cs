using System;

namespace Fulcrum.Core
{
	public abstract class Error : Exception
	{
		public ICommand RelatedCommand { get; private set; }

		protected Error(ICommand relatedCommand)
		{
			RelatedCommand = relatedCommand;
		}
	}
}