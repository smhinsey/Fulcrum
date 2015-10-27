using System;
using System.Security.Claims;
using System.Threading;
using Fulcrum.Common.Claims;
using log4net;

namespace Fulcrum.Common
{
	/// <summary>
	///   Extension methods used by implementors of ILoggingSource to perform logging related operations.
	/// </summary>
	// Method naming doesn't follow normal standards in order to group the related extension methods.
	public static class Log4NetLoggingSourceExtensions
	{
		private static Guid? LogIdOverride { get; set; }

		/// <summary>
		///   Used by applications which need to set their session log ids directly, rather than via claims.
		/// </summary>
		/// <returns></returns>
		public static void LogClearSessionId(this ILoggingSource source)
		{
			LogIdOverride = new Guid?();
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="formatParameters"> String formatting parameters used to render the message. </param>
		public static void LogDebug(this ILoggingSource source,
			string message, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsDebugEnabled)
			{
				logger.Debug(string.Format(message, formatParameters));
			}
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer without setting a thread context variable.
		///   Useful for initialization code or any code which executes prior to a session being created.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="formatParameters"> String formatting parameters used to render the message. </param>
		public static void LogDebugWithoutContext(this ILoggingSource source,
			string message, params object[] formatParameters)
		{
			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsDebugEnabled)
			{
				logger.Debug(string.Format(message, formatParameters));
			}
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="exception"> The exception associated with the log message. </param>
		/// <param name="formatParameters"> String formatting parameters used to render the message. </param>
		public static void LogError(this ILoggingSource source,
			string message, Exception exception, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsErrorEnabled)
			{
				logger.Error(string.Format(message, formatParameters), exception);
			}
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="formatParameters"> String formatting parameters used to render the message. </param>
		public static void LogError(this ILoggingSource source,
			string message, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsErrorEnabled)
			{
				logger.Error(string.Format(message, formatParameters));
			}
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="exception"> The exception associated with the log message. </param>
		/// <param name="formatParameters">String formatting parameters used to render the message.</param>
		public static void LogFatal(this ILoggingSource source,
			string message, Exception exception, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsFatalEnabled)
			{
				logger.Fatal(string.Format(message, formatParameters), exception);
			}
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="formatParameters">String formatting parameters used to render the message.</param>
		public static void LogFatal(this ILoggingSource source,
			string message, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsFatalEnabled)
			{
				logger.Fatal(string.Format(message, formatParameters));
			}
		}

		/// <summary>
		///   Returns the current session id, if one has been set.
		/// </summary>
		/// <returns></returns>
		public static Guid? LogGetSessionId(this ILoggingSource source)
		{
			return LogIdOverride;
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="formatParameters"> String formatting parameters used to render the message. </param>
		public static void LogInfo(this ILoggingSource source,
			string message, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsInfoEnabled)
			{
				if (formatParameters != null && formatParameters.Length > 0)
				{
					logger.Info(string.Format(message, formatParameters));
				}
				else
				{
					logger.Info(message);
				}
			}
		}

		/// <summary>
		///   Used by applications which need to set their session log ids directly, rather than via claims.
		/// </summary>
		/// <returns></returns>
		public static Guid LogSetSessionId(this ILoggingSource source)
		{
			LogIdOverride = Guid.NewGuid();

			return LogIdOverride.Value;
		}

		/// <summary>
		///   Used by applications which need to set their session log ids directly, rather than via claims.
		/// </summary>
		/// <returns></returns>
		public static Guid LogSetSessionId(this ILoggingSource source, Guid sessionId)
		{
			LogIdOverride = sessionId;

			return LogIdOverride.Value;
		}

		/// <summary>
		///   Writes a debug message to the configured log4net buffer.
		/// </summary>
		/// <param name="source"> An ILoggingSource implementation. </param>
		/// <param name="message"> The message to be written to the log. </param>
		/// <param name="formatParameters"> String formatting parameters used to render the message. </param>
		public static void LogWarn(this ILoggingSource source,
			string message, params object[] formatParameters)
		{
			setSessionLogContext();

			var logger = LogManager.GetLogger(source.GetType());

			if (logger.IsWarnEnabled)
			{
				logger.Warn(string.Format(message, formatParameters));
			}
		}

		private static void setSessionLogContext()
		{
			var sessionLogId = LogIdOverride ?? ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).GetSessionLogId();

			LogicalThreadContext.Properties["SessionLogId"] = sessionLogId;
		}
	}
}
