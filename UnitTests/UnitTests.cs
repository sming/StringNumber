using System;
using Xunit;
using Org.Kingswell.Peter;

namespace Org.Kingswell.Peter.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void ConvertStringToIntTest()
        {
            Assert.Equal(123, StringNumber.convertStringToInt("123"));
            Assert.Equal(1, StringNumber.convertStringToInt("1"));
            Assert.Equal(0, StringNumber.convertStringToInt("0"));
            Assert.Equal(int.MaxValue, StringNumber.convertStringToInt("2147483647"));

            // Test that strings representing numbers larger than ints do not overflow
            long intOverflow = int.MaxValue + 5L;
            Assert.Equal(int.MaxValue, StringNumber.convertStringToInt(intOverflow.ToString()));
        }

        [Fact]
        public void ToNumberTest()
        {
            Assert.Equal(123, (int)new StringNumber(123));
            Assert.Equal(123, (int)new StringNumber("123"));
            Assert.Equal(103, (int)new StringNumber("1a3"));
            Assert.Equal(103, (int)new StringNumber("a1a3"));
            Assert.Equal(20103, (int)new StringNumber("2a1a3"));
        }


        [Fact]
        public void AdditionOperatorTest()
        {
            Assert.Equal("x", (string)(new StringNumber("a") + new StringNumber("1")));
            Assert.Equal("xxx", (string)(new StringNumber("a1a") + new StringNumber("1a1")));
            Assert.Equal("xxx", (string)(new StringNumber("aaa") + new StringNumber("aaa")));
            Assert.Equal("333", (string)(new StringNumber("111") + new StringNumber("222")));
            Assert.Equal("2003", (string)(new StringNumber("1001") + new StringNumber("1002")));
            Assert.Equal("002003", (string)(new StringNumber("001001") + new StringNumber("001002")));
            string actual = (string)(new StringNumber("a234") + new StringNumber("00a1"));
            Assert.Equal("x2x5", actual);
        }
    }
}
