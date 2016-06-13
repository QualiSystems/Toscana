using System;
using System.Linq;

namespace Toscana.Engine
{
    public static class StringExtensions
    {
        public static bool EqualsAny(this string str, params string[] args)
        {
            return args.Any(x =>
              StringComparer.InvariantCultureIgnoreCase.Equals(x, str));
        } 
    }
}