using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

// This allows the Unit Tests to access 'internal' methods
[assembly: InternalsVisibleTo("UnitTests")]

namespace Org.Kingswell.Peter
{

    public class StringNumber
    {
        // use the Pimpl (Pointer to Implementation) idiom to hide implementation
        private StringNumberImplementation Implementation { get; set; }
        private const char DEFAULT_NON_DIGIT_REPLACEMENT = 'x';
        public char NonDigitReplacement { get; private set; }
        public string InitialValue { get; private set; }
        private StringNumber() { /* this is intentionally private to prevent default construction */ }

        public StringNumber(string s, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            NonDigitReplacement = nonDigitReplacement;
            InitialValue = s;
            Implementation = new StringNumberImplementation(this);
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
            Implementation = new StringNumberImplementation(this);
        }

        public StringNumber(BigInteger n, char nonDigitReplacement = DEFAULT_NON_DIGIT_REPLACEMENT)
        {
            NonDigitReplacement = nonDigitReplacement;
            InitialValue = n.ToString();
            Implementation = new StringNumberImplementation(this);
        }

        public static explicit operator BigInteger(StringNumber s) => s.ToNumber();

        public override string ToString()
        {
            return (string)this;
        }

        public static explicit operator string(StringNumber s) {
            return StringNumberImplementation.AsString(s);
        }

        public static StringNumber operator +(StringNumber a, StringNumber b)
        {
            return StringNumberImplementation.AdditionOperator(a, b);
        }

        public static StringNumber operator *(StringNumber a, StringNumber b)
        {
            return StringNumberImplementation.MultiplicationOperator(a, b);
        }

        private BigInteger ToNumber()
        {
            return Implementation.ToNumber();
        }
    }
}
