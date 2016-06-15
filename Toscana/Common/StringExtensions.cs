using System;
using System.Linq;

namespace Toscana.Common
{
    public static class StringExtensions
    {
        public static bool EqualsAny(this string str, params string[] args)
        {
            return args.Any(x =>
              StringComparer.InvariantCultureIgnoreCase.Equals((string) x, str));
        } 
    }
}