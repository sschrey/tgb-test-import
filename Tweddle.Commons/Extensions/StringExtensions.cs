using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tweddle.Commons.Extensions
{
    public static class StringExtensions
    {
        public static string MaxLength(this string s, int length)
        {
            return s.Length <= length ? s : s.Substring(0, length);
        }
    }
}
