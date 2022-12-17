using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCBHarmonization.Helper
{
    public class HolidayWeekend
    {
        private static readonly HashSet<DateTime> Holidays = new HashSet<DateTime>();

        private static bool IsHoliday(DateTime date)
        {
            return Holidays.Contains(date);
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }


        private static DateTime GetNextWorkingDay(DateTime date)
        {
            do
            {
                date = date.AddDays(-1);
            } while (IsHoliday(date) || IsWeekend(date));
            return date;
        }

        public DateTime DetermineNextWorkingDay()
        {
            string today = DateTime.Now.ToString();
            Holidays.Add(new DateTime(DateTime.Now.Year, 1, 1));
            Holidays.Add(new DateTime(DateTime.Now.Year, 05, 1));
            Holidays.Add(new DateTime(DateTime.Now.Year, 06, 12));
            Holidays.Add(new DateTime(DateTime.Now.Year, 10, 01));
            Holidays.Add(new DateTime(DateTime.Now.Year, 12, 25));
            Holidays.Add(new DateTime(DateTime.Now.Year, 12, 26));
            //var dt = GetNextWorkingDay(DateTime.Parse(@"2015-10-31"));
            var dt = GetNextWorkingDay(DateTime.Parse($"{today}"));
            return dt.Date;
        }

    }
}