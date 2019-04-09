using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

// This allows the Unit Tests to access 'internal' methods
[assembly: InternalsVisibleTo("UnitTests")]

namespace Org.Kingswell.Peter
{
    // StringNumberImplementation is an instance of the PIMPL idiom, which hides a libary's
    // implementation from users.
    class StringNumberImplementation
    {
        internal enum Operation
        {
            ADDITION,
            MULTIPLICATION
        }

        public StringNumber StringNumber { get; }

        internal StringNumberImplementation(StringNumber s)
        {
            this.StringNumber = s;
        }

        internal static StringNumber AdditionOperator(StringNumber a, StringNumber b)
        {
            return OperatorImplementation(a, b, Operation.ADDITION);
        }

        internal static StringNumber MultiplicationOperator(StringNumber a, StringNumber b)
        {
            return OperatorImplementation(a, b, Operation.MULTIPLICATION);
        }

        /// <summary>
        /// This method implements both multiplication and addition (determined via the 'o' operand).
        /// It takes StringNumbers a and b and adds/multiplies them and returns a new StringNumber
        /// with the result. It implements the contract defined in StringNumber class comment.
        /// 
        /// It basically works by calculating the numeric result and then stepping (a char at a time) through
        /// the result, a and b simultaneously, checking for non-digits. If one is found for the 
        /// current 'column', a.NonDigitReplacement is appended to a string result. Else the digit
        /// in the numeric result is appended.
        /// Finally the string result is converted into a new StringNumber (StringNumber's are 
        /// immutable, remember) and returned.
        /// </summary>
        /// <param name="a">the first StringNumber to add/multiply</param>
        /// <param name="b">the second StringNumber to add/multiply</param>
        /// <param name="o">which operation to apply to a and b: Multiply or Add</param>
        /// <returns></returns>
        internal static StringNumber OperatorImplementation(StringNumber a, StringNumber b, Operation o)
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
            
            int aOffset = maxLen - aLen;
            int bOffset = maxLen - bLen;
            for (int i = 0; i < maxLen; i++)
            {
                int index = maxLen - i - 1;
                if ((aLen > index) && !Char.IsDigit(a.InitialValue[i - aOffset]))
                {
                    sb.Append(a.NonDigitReplacement);
                }
                else if ((bLen > index) && !Char.IsDigit(b.InitialValue[i - bOffset]))
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

        internal static string AsString(StringNumber s)
        {
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
        
        private static bool IsNonDigit(string s, int sLen, int index)
        {
            if (index >= sLen)
                return false;
            else
                return !Char.IsDigit(s[index]);
        }
        
        internal BigInteger ToNumber()
        {
            var sb = new StringBuilder();
            foreach (char c in StringNumber.InitialValue)
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