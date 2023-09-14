using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class TimeConvert
    {
        //https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp,bool blnUTC=true)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            try
            {
                if (blnUTC)
                {
                    dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();//from php vteam api to C# 使用
                }
                else
                {
                    dateTime = dateTime.AddSeconds(unixTimeStamp);//from SQLite STRFTIME('%s',max(report_time)) to C# 使用
                }
            }
            catch (Exception ex)
            {
                String StrLog = String.Format("{0}: {1}", "TimeConvert_UnixTimeStampToDateTime() Error", ex.ToString());
                LogFile.Write(StrLog);
            }

            return dateTime;
        }
        
        //https://ourcodeworld.com/articles/read/865/how-to-convert-an-unixtime-to-datetime-class-and-viceversa-in-c-sharp
        public static long DateTimeToUnixTimeStamp(DateTime MyDateTime)
        {
            /*
            TimeSpan timeSpan = MyDateTime - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)(timeSpan.TotalSeconds-8*60*60);//8*60*60 來源: GMT+08:00 ~ https://www.epochconverter.com/
            */

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = MyDateTime.ToUniversalTime() - origin;
            return (long)(diff.TotalSeconds);//https://stackoverflow.com/questions/3354893/how-can-i-convert-a-datetime-to-the-number-of-seconds-since-1970
        }

        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            // Java timestamp is milliseconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dateTime;
        }
    }//TimeConvert
}
