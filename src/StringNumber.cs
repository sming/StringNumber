using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

// This allows the Unit Tests to access 'internal' methods
[assembly: InternalsVisibleTo("UnitTests")]

namespace Org.Kingswell.Peter
{

    // TODO use Pimpl idiom to hide implementation
    public class StringNumber
    {
        private enum Operation
        {
            ADDITION,
            MULTIPLICATION
        }

        private const char DEFAULT_NON_DIGIT_REPLACEMENT = 'x';
        public char NonDigitReplacement { get; private set; }
        public string InitialValue { get; private set; }
        private StringNumber() { /* this is intentionally private to prevent post-construction assignment */ }

        public StringNumber(string s, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            NonDigitReplacement = nonDigitReplacement;
            InitialValue = s;
        }

        public StringNumber(int n, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            Initialise(n, nonDigitReplacement);
        }

        public StringNumber(long n, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            Initialise(n, nonDigitReplacement);
        }

        public StringNumber(short n, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            Initialise(n, nonDigitReplacement);
        }

        public StringNumber(byte n, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            Initialise(n, nonDigitReplacement);
        }

        private void Initialise(long n, char nonDigitReplacement)
        {
            NonDigitReplacement = nonDigitReplacement;
            InitialValue = n.ToString();
        }

        public StringNumber(BigInteger n, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            NonDigitReplacement = nonDigitReplacement;
            InitialValue = n.ToString();
        }

        public static explicit operator BigInteger(StringNumber s) => s.ToNumber();

        public override string ToString()
        {
            return (string)this;
        }

        public static explicit operator string(StringNumber s) {
           if (s == null)
                return null;

            int sLen = s.InitialValue.Length;
            var sb = new StringBuilder(sLen);
            for (int i = 0; i < sLen; i++)
            {
                if (IsNonDigit(s.InitialValue, sLen, i))
                {
                    sb.Append(s.NonDigitReplacement);
                }
                else
                {
                    sb.Append(s.InitialValue[i]);
                }
            }

            return sb.ToString();
        }

        public static StringNumber operator +(StringNumber a, StringNumber b)
        {
            return OperatorImplementation(a, b, Operation.ADDITION);
        }

        public static StringNumber operator *(StringNumber a, StringNumber b)
        {
            return OperatorImplementation(a, b, Operation.MULTIPLICATION);
        }

        private static StringNumber OperatorImplementation(StringNumber a, StringNumber b, Operation o)
        {
            if (a == null || b == null)
                return null;

            var aNum = (BigInteger)a;
            var bNum = (BigInteger)b;
            var result = (o == Operation.ADDITION) ? aNum + bNum : aNum * bNum;

            int aLen = a.InitialValue.Length;
            int bLen = b.InitialValue.Length;
            int maxLen = Math.Max(result.ToString().Length, Math.Max(aLen, bLen));

            string resultStr = result.ToString("D" + maxLen);

            var sb = new StringBuilder(maxLen);
            for (int i = 0; i < maxLen; i++)
            {
                if (IsNonDigit(a.InitialValue, aLen, i) || IsNonDigit(b.InitialValue, bLen, i))
                {
                    sb.Append(a.NonDigitReplacement);
                }
                else
                {
                    sb.Append(resultStr[i]);
                }
            }

            return new StringNumber(sb.ToString(), a.NonDigitReplacement);
        }

        private static bool IsNonDigit(string s, int sLen, int index)
        {
            if (index >= sLen)
                return false;
            else
                return !Char.IsDigit(s[index]);
        }

        private BigInteger ToNumber()
        {
            var sb = new StringBuilder();
            foreach (char c in InitialValue)
            {
                sb.Append(Char.IsDigit(c) ? c : '0');
            }

            // Since the requirements state "Do not use parseInt or any other large integer library",
            // we have to do the conversion ourselves.
            return convertStringToBigInt(sb.ToString());
        }

        internal static BigInteger convertStringToBigInt(string s)
        {
            var result = new BigInteger(0);
            int sLength = s.Length;
            for (int i = 0; i < sLength; i++)
            {
                // We don't bother testing that the current char is a digit since this method is
                // 'internal' and only ever called from ToNumber().

                // We know that char '0' is the 'first' digit so we can subtract it's integer value
                // from the digit's integer value to get the digit's 'actual' integer value
                int digit = s[i] - '0';
                if (digit > 0)
                {
                    result = result + (digit * BigInteger.Pow(10, sLength - (i + 1)));
                }
            }

            return result;
        }
    }
}
