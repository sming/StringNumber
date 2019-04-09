using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

// This allows the Unit Tests to access 'internal' methods
[assembly: InternalsVisibleTo("UnitTests")]

namespace Org.Kingswell.Peter
{
    /*
    Represents an aritrarily large number as a string. Only the digits 0-9 are considered 'numeric'. All other characters e.g. '.', '-', 'z', are replaced as follows (consider this a functional
    contract):
    1. If a StringNumber object is cast to a BigInteger, the non-digit characters are converted to 
    zeros. E.g. "1234" -> 1234, "1a3" -> 103, "2a1a3" -> 20103.
    2. If a StringNumber object is added with another, the non-digits are again replaced by zeros, the 
    two numbers added and then any 'columns' with a non-digit are replaced with (by default) the char
    'x'. 
    
    Examples:
    123 + 
    123
    = 246
    
    12a +
    123
    = 24x

    0a34 +
    a239
    = xx73
    3. The same steps apply for multiplication

    To create a StringNumber, simply 'new' one up with a numeric type parameter or string. You can
    override the 'x' char replacement by passing that as the second argument in the constructor.

    To Add and Multiply them, use the + and * operators respectively e.g.
    var oneHundred = new StringNumber("100");
    var twoHundred = new StringNumber("200");
    var threeHundred = oneHundred + twoHundred;

    The conversion operators string and BigInteger can be used to get said types "out of" a 
    StringNumber e.g. for 'string'
    var oneHundred = new StringNumber("100");
    string s = (string)oneHundred;
    [ OR string s = oneHundred.ToString(); ]

    NOTE that StringNumber objects are immutable - operators return new StringNumber objects. 
     */
    public class StringNumber
    {
        // use the Pimpl (Pointer to Implementation) idiom to hide implementation from users.
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
