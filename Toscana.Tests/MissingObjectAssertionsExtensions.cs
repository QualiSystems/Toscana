using FluentAssertions.Primitives;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toscana.Tests
{
    public static class MissingObjectAssertionsExtensions
    {
#if !NETFRAMEWORK
        /// <remarks>
        /// This method is fully implemented in the .NET Framework variant of FluentAssertions,
        /// but is missing from other variants.
        /// This implementation is a stub, just to allow existing tests to compile and run properly against
        /// both .NET Framework and .NET Core / 6.0, etc.
        /// </remarks>
        public static AndConstraint<ObjectAssertions> BeBinarySerializable(this ObjectAssertions assertions, string because = "", params object[] becauseArgs)
        {
            return new AndConstraint<ObjectAssertions>(assertions);
        }
#endif
    }
}
