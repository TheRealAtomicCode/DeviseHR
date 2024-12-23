using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class NumberUtils
    {

        public static int RoundCustom(double number)
        {
            if (number - Math.Floor(number) < 0.5)
            {
                return (int)Math.Floor(number); // Round down
            }
            else
            {
                return (int)Math.Ceiling(number); // Round up
            }
        }


    }
}
