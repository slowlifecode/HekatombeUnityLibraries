using System;
using System.Collections;
using UnityEngine;

namespace Hekatombe.Utils
{
    public static class TimeUtils
    {
		static private readonly DateTime Epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        static public long GetTimestamp(DateTime dt)
        {
            return (long)dt.Subtract(Epoch).TotalSeconds;
        }

        static public long GetTimestampMilliseconds(DateTime dt)
        {
            return (long)dt.Subtract(Epoch).TotalMilliseconds;
        }

        static public DateTime GetTime(double timestamp)
        {
			return GetTimeUtc(timestamp).ToLocalTime();
		}

		static public DateTime GetTimeUtc(double timestamp)
		{
			return Epoch.AddSeconds(timestamp);
		}
        
        static public long TimeStamp
        {
            get
            {
                return GetTimestamp(DateTime.UtcNow);
            }
        }

        static public long TimeStampMilliseconds
        {
            get
            {
                return GetTimestampMilliseconds(DateTime.UtcNow);
            }
		}

		public static string GetIsoTimeStr (DateTime dt)
		{
			return dt.ToString ("yyyyMMddTHHmmssZ");
		}

		public static string GetIsoTimeStr (string dts)
		{
			DateTime dt = DateTime.ParseExact (dts, "yyyyMMdd", null);
			return GetIsoTimeStr (new DateTime (dt.Year, dt.Month, dt.Day, 0, 0, 0));
		}
		
		public static string FormatTime(int timeInSeconds)
		{
			return FormatTime((float) timeInSeconds);
		}
		
		public static string FormatTime(float time)
		{
			float tempTime = time;
			int days = (int)(time / (60f * 60f * 24f));
			tempTime -= days * 60f * 60f * 24f;
			int hours = (int)(tempTime / (60f * 60f));
			tempTime -= hours * 60f * 60f;
			int minutes = (int)(tempTime / 60f);
			tempTime -= minutes * 60f;
			int seconds = (int)(tempTime);
			
			if( days > 0 )
			{
				return string.Format("{0}d {1}h", days, hours);
			}
			else if( hours > 0 )
			{
				return string.Format("{0}h {1}m", hours, minutes);
			}
			else if( minutes > 0 )
			{
				return string.Format("{0}m {1}s", minutes, seconds);
			}
			else
			{
				return string.Format("{0}s", seconds);
			}
		}

		//To avoid stop the action even if the it Time.timeScale = 0
		//Ex: yield return StartCoroutine(TimeUtils.WaitForRealSeconds(0.5f));
		public static IEnumerator WaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
		}
		
		public static DateTime ToDateTime(this long timestamp)
		{
			return ToDateTime((float)timestamp);
		}
		
		public static DateTime ToDateTime(this int timestamp)
		{
			return ToDateTime((float)timestamp);
		}

		public static DateTime ToDateTime(this double timestamp)
		{
			return Epoch.AddSeconds(timestamp).ToLocalTime();
		}    

		/// <summary>
		/// Gets the 12:00:00 instance of a DateTime
		/// </summary>
		public static DateTime AbsoluteStart(this DateTime dateTime)
		{
			return dateTime.Date;
		}

		/// <summary>
		/// Gets the 11:59:59 instance of a DateTime
		/// </summary>
		public static DateTime AbsoluteEnd(this DateTime dateTime)
		{
			return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
		}
    }
}
