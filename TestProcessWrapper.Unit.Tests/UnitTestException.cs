using System;

namespace TestProcessWrapper.Unit.Tests;

public class UnitTestException : Exception
{
    public UnitTestException(string message)
        : base(message) { }
}
