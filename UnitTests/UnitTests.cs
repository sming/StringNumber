using System;
using Xunit;
using Org.Kingswell.Peter;
using System.Numerics;

namespace Org.Kingswell.Peter.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void ConvertStringToBigIntTest()
        {
            Assert.Equal(123, StringNumber.convertStringToBigInt("123"));
            Assert.Equal(1, StringNumber.convertStringToBigInt("1"));
            Assert.Equal(0, StringNumber.convertStringToBigInt("0"));
            Assert.Equal(int.MaxValue, StringNumber.convertStringToBigInt("2147483647"));
            Assert.Equal(long.MaxValue, StringNumber.convertStringToBigInt("9223372036854775807"));
            Assert.Equal(BigInteger.Parse("33333333333333333333"), StringNumber.convertStringToBigInt("33333333333333333333"));
        }

        [Fact]
        public void ToNumberTest()
        {
            Assert.Equal(123, (BigInteger)new StringNumber(123));
            Assert.Equal(123, (BigInteger)new StringNumber("123"));
            Assert.Equal(103, (BigInteger)new StringNumber("1a3"));
            Assert.Equal(103, (BigInteger)new StringNumber("a1a3"));
            Assert.Equal(20103, (BigInteger)new StringNumber("2a1a3"));
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
