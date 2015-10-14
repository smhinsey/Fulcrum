using System;
using System.Collections.Generic;

namespace Fulcrum.Common
{
	public static class DateTimeExtensions
	{
		public static DateTime FindWeekStartDate(this DateTime date, DayOfWeek startOfWeek)
		{
			var diff = date.DayOfWeek - startOfWeek;
			
			if (diff < 0)
			{
				diff += 7;
			}

			return date.AddDays(-1 * diff).Date;
		}

		public static DateTime RoundUp(this DateTime dateTime, TimeSpan duration)
		{
			var delta = (duration.Ticks - (dateTime.Ticks % duration.Ticks)) % duration.Ticks;

			return new DateTime(dateTime.Ticks + delta, dateTime.Kind);
		}

		public static DateTime RoundDown(this DateTime dateTime, TimeSpan duration)
		{
			var delta = dateTime.Ticks % duration.Ticks;

			return new DateTime(dateTime.Ticks - delta, dateTime.Kind);
		}

		public static DateTime RoundToNearest(this DateTime dateTime, TimeSpan duration)
		{
			var delta = dateTime.Ticks % duration.Ticks;

			var roundUp = delta > duration.Ticks / 2;

			return roundUp ? dateTime.RoundUp(duration) : dateTime.RoundDown(duration);
		}

		public static IEnumerable<DateTime> GetWeekDays(this DateTime dateFrom)
		{
			return new List<DateTime>
			{
				dateFrom,
				dateFrom.AddDays(1),
				dateFrom.AddDays(2),
				dateFrom.AddDays(3),
				dateFrom.AddDays(4),
				dateFrom.AddDays(5),
				dateFrom.AddDays(6),
			};
		}
	}
}