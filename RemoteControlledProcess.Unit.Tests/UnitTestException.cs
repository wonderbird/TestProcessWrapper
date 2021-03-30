using System;

namespace RemoteControlledProcess.Unit.Tests
{
    public class UnitTestException : Exception
    {
        public UnitTestException(string message)
            : base(message)
        {
        }
    }
}