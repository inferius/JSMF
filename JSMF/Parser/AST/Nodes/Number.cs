using System.Globalization;

namespace JSMF.Parser.AST.Nodes
{
    public class Number
    {
        private readonly int _iValue;
        private readonly double _dValue;
        private readonly bool isInteger = false;

        public Number(int value)
        {
            isInteger = true;
            _iValue = value;
        }

        public Number(double value)
        {
            isInteger = false;
            _dValue = value;
        }

        public static Number Parse(string value)
        {
            if (value == null) return 0;
            if (value.IndexOf(".") == -1) return int.Parse(value);
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        public static implicit operator Number(int value)
        {
            return new Number(value);
        }

        public static implicit operator Number(double value)
        {
            return new Number(value);
        }

        public static implicit operator int(Number value)
        {
            if (value.isInteger) return value._iValue;
            return (int) value._dValue;
        }

        public static implicit operator double(Number value)
        {
            if (value.isInteger) return value._iValue;
            return value._dValue;
        }

        public override string ToString()
        {
            if (isInteger) return _iValue.ToString(CultureInfo.InvariantCulture);
            return _dValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}
