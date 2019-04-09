﻿using System;
using System.Runtime.CompilerServices;
using System.Text;

// This allows the Unit Tests to access 'internal' methods
[assembly: InternalsVisibleTo("UnitTests")]

namespace Org.Kingswell.Peter
{

    // TODO use Pimpl idiom to hide implementation
    public class StringNumber
    {
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

        public static explicit operator int(StringNumber s) => s.ToNumber();

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
            if (a == null || b == null)
                return null;

            int aNum = (int)a;
            int bNum = (int)b;
            int result = aNum + bNum;

            int aLen = a.InitialValue.Length;
            int bLen = b.InitialValue.Length;
            int longestLength = Math.Max(aLen, bLen);
            // int rawResultLength = result.ToString().Length;
            // int leadingNumberOfZerosNeeded = Math.Abs(longestLength - rawResultLength);

            string resultStr = result.ToString("D" + longestLength);

            var sb = new StringBuilder(longestLength);
            for (int i = 0; i < longestLength; i++)
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

        private int ToNumber()
        {
            var sb = new StringBuilder();
            foreach (char c in InitialValue)
            {
                sb.Append(Char.IsDigit(c) ? c : '0');
            }

            // Since the requirements state "Do not use parseInt or any other large integer library",
            // we have to do the conversion ourselves.
            return convertStringToInt(sb.ToString());
        }

        internal static int convertStringToInt(string s)
        {
            long result = 0;
            int sLength = s.Length;
            for (int i = 0; i < sLength; i++)
            {
                // We don't bother testing that the current char is a digit since it's 'internal' and
                // only ever called from ToNumber().

                // We know that char '0' is the 'first' digit so we can subtract it's integer value
                // from the digit's integer value to get the digit's 'actual' integer value
                int digit = s[i] - '0';
                if (digit > 0)
                {
                    // TODO test very large numbers don't overflow
                    result += Math.Min(int.MaxValue, digit * (int)Math.Pow(10, sLength - (i + 1)));
                }
            }

            return result > int.MaxValue ? int.MaxValue : (int)result;
        }
    }
}