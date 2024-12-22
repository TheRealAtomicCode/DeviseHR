using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DateModifier
    {
        public static DateOnly SetYearTo1900(DateOnly date)
        {
            return new DateOnly(1900, date.Month, date.Day);
        }

        public static DateOnly ConvertStringToDateOnly(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime dateTime))
            {
                DateOnly dateOnly = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                return dateOnly;
            }

            throw new ArgumentException("Please provise a valid date format: 'yyyy-mm-dd'.");
        }

        public static TimeOnly ConvertStringToTimeOnly(string timeString)
        {
            if (TimeSpan.TryParse(timeString, out TimeSpan timeSpan))
            {
                TimeOnly timeOnly = new TimeOnly(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                return timeOnly;
            }

            throw new ArgumentException("Please provide a valid time format: 'hh:mm:ss'.");
        }

        public static int GetDaysBetween(DateOnly startDate, DateOnly endDate)
        {
            int days = endDate.DayNumber - startDate.DayNumber;

            if (days < 0) throw new ArgumentException("End date cannot be earlier than Start date.");

            return days;
        }

        public static DateOnly GetLeaveYearStartDate(DateOnly newContractStartDate, DateOnly annualLeaveStartDateFrom70s)
        {
            int year = newContractStartDate.Year;
            int month = annualLeaveStartDateFrom70s.Month;
            int day = annualLeaveStartDateFrom70s.Day;

            DateOnly leaveYearStartDate = new DateOnly(year, month, day);
            return leaveYearStartDate;
        }

        public static int GetNumberOfDaysInYear(DateOnly date)
        {
            bool isLeapYear = DateTime.IsLeapYear(date.Year);
            int daysInYear = isLeapYear ? 366 : 365;
            return daysInYear;
        }


    }
}


