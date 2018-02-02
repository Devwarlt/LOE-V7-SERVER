﻿using System;
using System.Linq;

namespace appengine
{
    public class DateProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is DateTime)) throw new NotSupportedException();

            DateTime dt = (DateTime)arg;

            string suffix, _suffix;

            if (new[] { 11, 12, 13 }.Contains(dt.Day))
                suffix = "th";
            else if (dt.Day % 10 == 1)
                suffix = "st";
            else if (dt.Day % 10 == 2)
                suffix = "nd";
            else if (dt.Day % 10 == 3)
                suffix = "rd";
            else
                suffix = "th";

            string[] time = dt.TimeOfDay.ToString().Split('.')[0].Split(':');

            _suffix = Convert.ToInt32(time[0]) >= 12 ? "PM" : "AM";

            return $"{time[0]}:{time[1]} {_suffix} {dt.Day}{suffix} {arg:MMM} {arg:yyyy}";
        }
    }
}