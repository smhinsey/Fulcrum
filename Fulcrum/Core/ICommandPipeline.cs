using System;
using System.Reflection;
using Castle.Windsor;

namespace Fulcrum.Core
{
	/// <summary>
	///   A command pipeline implementation is responsible for
	/// </summary>
	public interface ICommandPipeline
	{
		/// <summary>
		///   Prevent the publication of commands.
		/// </summary>
		void DisablePublication();

		/// <summary>
		///   Allow the publication of commands.
		/// </summary>
		void EnablePublication();

		/// <summary>
		///   Returns the current publication record.
		/// </summary>
		/// <param name="publicationId"></param>
		/// <returns></returns>
		ICommandPublicationRecord Inquire(Guid publicationId);

		/// <summary>
		///   Using custom configuration if necessary, load a specified set of command handlers
		///   into the target container such that the handlers can be executed.
		/// </summary>
		void InstallHandlers(IWindsorContainer targetContainer, params Assembly[] assembliesToScan);

		ICommandPublicationRecord MarkAsComplete(Guid publicationId);

		ICommandPublicationRecord MarkAsFailed(Guid publicationId, string errorHeadline, Exception exception);

		ICommandPublicationRecord MarkAsProcessing(Guid publicationId);

		ICommandPublicationRecord Publish(ICommand command);
	}
}
