using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Shared
{
    public class Utils
    {
        public static string[] SplitList(string list)
        {
            if (list == null)
            {
                return new string[] { string.Empty };
            }
            else
            {
                var l = list.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < l.Count; i++)
                {
                    l[i] = l[i].Trim();
                }
                return l.ToArray();
            }
        }

        public static int ToInt(string number)
        {
            int integer = 0;
            if (number.Contains("."))
            {
                integer = Convert.ToInt32(number.Split('.')[0]);
            }
            else
            {
                Int32.TryParse(number, out integer);
            }
            return integer;
        }
    }
}
