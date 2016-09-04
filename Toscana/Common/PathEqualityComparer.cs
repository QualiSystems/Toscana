using System;
using System.Collections.Generic;
using System.IO;

namespace Toscana.Common
{
    internal class PathEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }
            if (string.Compare(NormalizePath(x), NormalizePath(y), StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(string obj)
        {
            return NormalizePath(obj).GetHashCode();
        }

        private static string NormalizePath(string unnormalized)
        {
            return unnormalized.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}