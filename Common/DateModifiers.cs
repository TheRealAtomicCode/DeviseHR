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
    }
}


