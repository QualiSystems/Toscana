using System;
using System.Linq;

namespace Toscana.Common
{
    public static class ExceptionExtensions
    {
        public static string GetaAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }
    }
}