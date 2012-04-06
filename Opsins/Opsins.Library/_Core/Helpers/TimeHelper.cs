using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Opsins
{
    /// <summary>
    /// Enum to represent the time as a part of the day.
    /// </summary>
    public enum StartTimeOfDay
    {
        /// <summary>
        /// All day.
        /// </summary>
        All = 0,

        /// <summary>
        /// Morning part.
        /// </summary>
        Morning,

        /// <summary>
        /// Afternoon part.
        /// </summary>
        Afternoon,

        /// <summary>
        /// Evening part.
        /// </summary>
        Evening
    };

    /// <summary>
    /// Time parse result.
    /// </summary>
    public class TimeParseResult
    {
        /// <summary>
        /// True if the parse was valid.
        /// </summary>
        public readonly bool IsValid;


        /// <summary>
        /// Validation error.
        /// </summary>
        public readonly string Error;


        /// <summary>
        /// Start of period.
        /// </summary>
        public readonly TimeSpan Start;


        /// <summary>
        /// End of period.
        /// </summary>
        public readonly TimeSpan End;


        /// <summary>
        /// Constructor to initialize the results.
        /// </summary>
        /// <param name="valid">Validation result.</param>
        /// <param name="error">Error string.</param>
        /// <param name="start">Start of period.</param>
        /// <param name="end">End of period.</param>
        public TimeParseResult(bool valid, string error, TimeSpan start, TimeSpan end)
        {
            IsValid = valid;
            Error = error;
            Start = start;
            End = end;
        }


        /// <summary>
        /// Get the start time as a datetime.
        /// </summary>
        public DateTime StartTimeAsDate
        {
            get
            {
                if (Start == TimeSpan.MinValue)
                    return TimeParserConstants.MinDate;

                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Start.Hours, Start.Minutes, Start.Seconds);
            }
        }


        /// <summary>
        /// Get the end time as a datetime.
        /// </summary>
        public DateTime EndTimeAsDate
        {
            get
            {
                if (End == TimeSpan.MaxValue)
                    return TimeParserConstants.MaxDate;

                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, End.Hours, End.Minutes, End.Seconds);
            }
        }
    }



    /// <summary>
    /// constants used by the time parser.
    /// </summary>
    public class TimeParserConstants
    {
        /// <summary>
        /// Am string.
        /// </summary>
        public const string Am = "am";

        /// <summary>
        /// Am string with periods a.m.
        /// </summary>
        public const string AmWithPeriods = "a.m.";

        /// <summary>
        /// Pm string.
        /// </summary>
        public const string Pm = "pm";

        /// <summary>
        /// Pm string with periods p.m.
        /// </summary>
        public const string PmWithPeriods = "p.m.";

        /// <summary>
        /// Min Time to represent All times for a post.
        /// </summary>
        public static readonly DateTime MinDate = new DateTime(2000, 1, 1);

        /// <summary>
        /// Max Time to represent all times for a post.
        /// </summary>
        public static readonly DateTime MaxDate = new DateTime(2050, 1, 1);

        /// <summary>
        /// Error string for end time less than start time.
        /// </summary>
        public const string ErrorEndTimeLessThanStart = "End time must be greater than or equal to start time.";


        /// <summary>
        /// Error string for no separator between start and end time.
        /// </summary>
        public const string ErrorStartEndTimeSepartorNotPresent = "Start and end time separator not present, use '-' or 'to'";


        /// <summary>
        /// Error string for no start time provided.
        /// </summary>
        public const string ErrorStartTimeNotProvided = "Start time not provided";


        /// <summary>
        /// Error string for no end time provided.
        /// </summary>
        public const string ErrorEndTimeNotProvided = "End time not provided";
    }

    /// <summary>
    /// 与DateTime相关的工具类
    /// </summary>
    public static class TimeHelper
    {
        #region IsOnTime
        /// <summary>
        /// 时间val与requiredTime之间的差值是否在maxToleranceInSecs范围之内。
        /// </summary>        
        public static bool IsOnTime(DateTime requiredTime, DateTime val, int maxToleranceInSecs)
        {
            TimeSpan span = val - requiredTime;
            double spanMilliseconds = span.TotalMilliseconds < 0 ? (-span.TotalMilliseconds) : span.TotalMilliseconds;

            return (spanMilliseconds <= (maxToleranceInSecs * 1000));
        }

        /// <summary>
        /// 对于循环调用，时间val与startTime之间的差值(>0)对cycleSpanInSecs求余数的结果是否在maxToleranceInSecs范围之内。
        /// </summary>        
        public static bool IsOnTime(DateTime startTime, DateTime val, int cycleSpanInSecs, int maxToleranceInSecs)
        {
            TimeSpan span = val - startTime;
            double spanMilliseconds = span.TotalMilliseconds;
            double residual = spanMilliseconds % (cycleSpanInSecs * 1000);

            return (residual <= (maxToleranceInSecs * 1000));
        }
        #endregion


        #region Time Conversion methods
        /// <summary>
        /// Convert military time ( 1530 = 3:30 pm ) to a TimeSpan.
        /// </summary>
        /// <param name="military">Military time.</param>
        /// <returns>An instance of timespan corresponding to the military time supplied.</returns>
        public static TimeSpan ConvertFromMilitaryTime(int military)
        {
            TimeSpan time = TimeSpan.MinValue;
            int hours = military / 100;
            int hour = hours;
            int minutes = military % 100;

            time = new TimeSpan(hours, minutes, 0);
            return time;
        }


        /// <summary>
        /// Converts to military time.
        /// </summary>
        /// <param name="timeSpan">Instance of time span with time to convert.</param>
        /// <returns>Military time corresponding to the time span supplied.</returns>
        public static int ConvertToMilitary(TimeSpan timeSpan)
        {
            return (timeSpan.Hours * 100) + timeSpan.Minutes;
        }
        #endregion

        #region Formatting methods
        /// <summary>
        /// Returns a military time formatted as a string.
        /// </summary>
        /// <param name="militaryTime">Military time.</param>
        /// <param name="convertSingleDigit">True to convert times with single digit.</param>
        /// <returns>String with formatted equivalent of passed military time.</returns>
        public static string Format(int militaryTime, bool convertSingleDigit)
        {
            if (convertSingleDigit && militaryTime < 10)
                militaryTime = militaryTime * 100;

            TimeSpan t = ConvertFromMilitaryTime(militaryTime);
            string formatted = Format(t);
            return formatted;
        }


        /// <summary>
        /// Get the time formatted correctly to exclude the minutes if
        /// there aren't any. Also includes am - pm.
        /// </summary>
        /// <param name="time">Time span to format.</param>
        /// <returns>String with formatted time span.</returns>
        public static string Format(TimeSpan time)
        {
            int hours = time.Hours;
            string amPm = hours < 12 ? TimeParserConstants.Am : TimeParserConstants.Pm;

            // Convert military time 13 hours to standard time 1pm
            if (hours > 12)
                hours = hours - 12;

            if (time.Minutes == 0)
                return hours + amPm;

            // Handles 11:10 - 11:59
            if (time.Minutes > 10)
                return hours + ":" + time.Minutes + amPm;

            // Handles 11:01 - 11:09
            return hours + ":0" + time.Minutes + amPm;
        }
        #endregion


        #region Miscellaneous Methods
        /// <summary>
        /// Gets the time as a part of the day.( morning, afternoon, evening ).
        /// </summary>
        /// <param name="time">Time of day.</param>
        /// <returns>Instance of start time of day.</returns>
        public static StartTimeOfDay GetTimeOfDay(TimeSpan time)
        {
            if (time.Hours < 12) return StartTimeOfDay.Morning;
            if (time.Hours >= 12 && time.Hours <= 16) return StartTimeOfDay.Afternoon;
            return StartTimeOfDay.Evening;
        }


        /// <summary>
        /// Get the time of day ( morning, afternoon, etc. ) from military time.
        /// </summary>
        /// <param name="militaryTime">Military time.</param>
        /// <returns>Instance with corresponding start time of day.</returns>
        public static StartTimeOfDay GetTimeOfDay(int militaryTime)
        {
            return GetTimeOfDay(ConvertFromMilitaryTime(militaryTime));
        }
        #endregion
    }
}
