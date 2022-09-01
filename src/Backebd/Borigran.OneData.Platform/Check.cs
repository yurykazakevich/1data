using Borigran.OneData.Platform.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borigran.OneData.Platform
{
    /// <summary>
    /// Assertions for testing input values
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Panic and terminate execution
        /// </summary>
        /// <param name="message">Message to return</param>
        /// <param name="values"></param>
        public static void Panic(string message, params object[] values)
        {
            throw new AssertionFailedException("Panic: " + string.Format(message, values));
        }

        /// <summary>
        /// Determines whether the value is null.  If it is it throws an
        /// AssertionFailedException with the provided message.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="message">The message.</param>
        /// <param name="values">Additional formating objects passed to string.Format()</param>
        public static void IsNotNull<T>(T value, string message, params object[] values) where T : class
        {
            if (value == null)
                throw new AssertionFailedException(string.Format(message, values));
        }

        /// <summary>
        /// Determine whether the nullable has a value or not. If it is, it 
        /// throws AssertionFailedException with the provided message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="values"></param>
        public static void IsNotNull<T>(Nullable<T> value, string message, params object[] values) where T : struct
        {
            if (!value.HasValue)
                throw new AssertionFailedException(string.Format(message, values));
        }

        /// <summary>
        /// Determines whether the specified value is true.  Throws an
        /// AssertionFailedException if it is found to be false
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="message">The message.</param>
        /// <param name="values"></param>
        public static void IsTrue(bool value, string message, params object[] values)
        {
            if (!value)
                throw new AssertionFailedException(string.Format(message, values));
        }

        /// <summary>
        /// Throw exception if toCheck is null or white space.
        /// </summary>
        /// <param name="toCheck">Value to check</param>
        /// <param name="message">Message to throw in assertion.</param>
        /// <param name="values"></param>
        public static void IsNotNullOrWhiteSpace(string toCheck, string message, params object[] values)
        {
            if (string.IsNullOrWhiteSpace(toCheck))
                throw new AssertionFailedException(string.Format(message, values));
        }

        public static void IsUtc(DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("DateTime.Kind must be Utc");
        }
    }
}
