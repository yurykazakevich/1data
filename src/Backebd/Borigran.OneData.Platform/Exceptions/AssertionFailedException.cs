using System;

namespace Borigran.OneData.Platform.Exceptions
{
    public class AssertionFailedException : Exception
    {
        public AssertionFailedException(string message)
            : base("Assertion failed: " + message)
        {
        }
    }
}
