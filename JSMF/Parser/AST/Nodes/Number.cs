using System.Globalization;

namespace JSMF.Parser.AST.Nodes
{
    public class Number
    {
        internal readonly int _iValue;
        internal readonly double _dValue;
        internal readonly bool IsInteger = false;

        public Number(int value)
        {
            IsInteger = true;
            _iValue = value;
        }

        public Number(double value)
        {
            IsInteger = false;
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
            if (value.IsInteger) return value._iValue;
            return (int) value._dValue;
        }

        public static implicit operator double(Number value)
        {
            if (value.IsInteger) return value._iValue;
            return value._dValue;
        }

        public override string ToString()
        {
            if (IsInteger) return _iValue.ToString(CultureInfo.InvariantCulture);
            return _dValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}
