using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGathering.Web.Structs
{
    public struct Time
    {
        // Variables
        public int hour;
        public int minute;

        // Convert to String
        public override string ToString()
        {
            int formattedHour = hour % 12;
            if (formattedHour == 0)
                formattedHour = 12;
            return hour + ":" + minute;
        }

        // Constructors
        public Time(int hour, int minute)
        {
            this.hour = hour;
            this.minute = minute;

            checkTime(hour + ":" + minute);
        }
        public Time(string str)
        {
            try
            {
                string[] splitStr = str.Split(':');

                string hourStr = splitStr[0];
                string minuteStr = splitStr[1];

                hour = int.Parse(hourStr);
                minute = int.Parse(minuteStr);

                checkTime(str);
            }
            catch
            {
                throw new Exception("Invalid Time Input '" + str + "'");
            }
        }

        // Check for Invalid Times
        private void checkTime(string timeInput)
        {
            if (minute >= 60 || hour > 24 || minute < 0 || hour < 0)
            {
                throw new Exception("Invalid Time Input '" + timeInput + "'");
            }
        }
    }
}