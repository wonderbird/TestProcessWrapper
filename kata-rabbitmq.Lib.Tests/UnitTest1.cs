using System;
using Xunit;

namespace kata_rabbitmq.Lib.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("World", Class1.Hello());
        }
    }
}
