using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Utilities
{
    public class TimeUtilities
    {
        public static Boolean IsBetweenTimes(DateTime datum, TimeSpan? start, TimeSpan? end)
        {
            Boolean isBetween = false;
            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;
            if (start >= end)
                EndDate = EndDate.AddDays(1);
            StartDate = StartDate.Date + start.Value;
            EndDate = EndDate.Date + end.Value;
            if ((datum >= StartDate) && (datum <= EndDate))
                isBetween = true;
            return isBetween;
        }
    }
}
