using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetMessages(this Exception exc)
        {
            var messages = new List<string>();
            while (exc != null)
            {
                messages.Add(exc.Message);
                exc = exc.InnerException;
            }

            return string.Join(Environment.NewLine, messages);
        }
    }
}
