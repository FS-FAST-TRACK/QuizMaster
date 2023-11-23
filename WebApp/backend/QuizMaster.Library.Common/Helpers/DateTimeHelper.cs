using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Helpers
{

    public static class DateTimeHelper
    {
        public static string GetPhilippinesTimestamp()
        {
            var philippinesTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
            var philippinesTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, philippinesTimeZone);
            return philippinesTime.ToString();
        }
    }

}
