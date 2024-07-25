using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Core;
using JSMF.Exceptions;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Interpreter
{
    public enum JSValueType
    {
        NotExists = 0,
        NotExistsInObject = 1,
        Null = 2,
        Undefined = 3,                          // 000000000011 // nedefinovaný hodnota znamená, že objekt je ve skutečnosti definován, ale jeho hodnota není
        Boolean = 4 | Undefined,                // 000000000111
        Integer = 8 | Undefined,                // 000000001011
        Double = 16 | Undefined,                // 000000010011
        String = 32 | Undefined,                // 000000100011
        Symbol = 64 | Undefined,                // 000001000011
        Object = 128 | Undefined,               // 000010000011
        Function = 256 | Undefined,             // 000100000011
        Date = 512 | Undefined,                 // 001000000011
        Property = 1024 | Undefined,            // 010000000011
        SpreadOperatorResult = 2048 | Undefined // 100000000011
    }

    public enum JSValueObjectType
    {
        ArrayType = 2,
        ClassType = 4,
        JsonType = 8,
    }

    [Flags]
    internal enum JSValueAttributesInternal : uint
    {
        None = 0,
        DoNotEnumerate = 1 << 0,
        DoNotDelete = 1 << 1,
        ReadOnly = 1 << 2,
        Immutable = 1 << 3,
        NonConfigurable = 1 << 4,
        Argument = 1 << 16,
        SystemObject = 1 << 17,
        ProxyPrototype = 1 << 18,
        Field = 1 << 19,
        Eval = 1 << 20,
        Temporary = 1 << 21,
        Cloned = 1 << 22,
        Reassign = 1 << 25,
        IntrinsicFunction = 1 << 26,
        ConstructingObject = 1 << 27
    }

    public enum PropertyScope
    {
        Сommon = 0,
        Own = 1,
        Super = 2,
        PrototypeOfSuperclass = 3
    }

    public class JSValue : IEnumerable<KeyValuePair<string, JSValue>>, IComparable<JSValue>
    {
        internal static readonly JSValue undefined = new JSValue() { _valueType = JSValueType.Undefined, _attributes = JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject };
        internal static readonly JSValue nullValue = new JSValue() { _valueType = JSValueType.Null, _attributes = JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable | JSValueAttributesInternal.SystemObject };

        internal JSValueAttributesInternal _attributes;
        internal JSValueType _valueType;
        internal JSValueObjectType _valueObjectSubtype;
        internal int _iValue;
        internal double _dValue;
        internal object _oValue;
        internal Position _position;

        public virtual object Value
        {
            get
            {
                switch (_valueType)
                {
                    case JSValueType.Boolean:
                        return _iValue != 0;
                    case JSValueType.Integer:
                        return _iValue;
                    case JSValueType.Double:
                        return _dValue;
                    case JSValueType.String:
                        return _oValue.ToString();
                    case JSValueType.Symbol:
                        return _oValue;
                    case JSValueType.Object:
                    case JSValueType.Function:
                    case JSValueType.Property:
                    case JSValueType.SpreadOperatorResult:
                    case JSValueType.Date:
                        {
                            if (_oValue != this && _oValue is JSObject)
                                return (_oValue as JSObject).Value;
                            return _oValue;
                        }
                    case JSValueType.Undefined:
                    case JSValueType.Null:
                    case JSValueType.NotExistsInObject:
                    default:
                        return null;
                }
            }
            protected set
            {
                switch (_valueType)
                {
                    case JSValueType.Boolean:
                        {
                            _iValue = (bool)value ? 1 : 0;
                            break;
                        }
                    case JSValueType.Integer:
                        {
                            _iValue = (int)value;
                            break;
                        }
                    case JSValueType.Double:
                        {
                            _dValue = (double)value;
                            break;
                        }
                    case JSValueType.String:
                        {
                            _oValue = (string)value;
                            break;
                        }
                    case JSValueType.Object:
                    case JSValueType.Function:
                    case JSValueType.Property:
                    case JSValueType.Date:
                        {
                            _oValue = value;
                            break;
                        }
                    case JSValueType.Undefined:
                    case JSValueType.Null:
                    case JSValueType.NotExistsInObject:
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        internal bool IsTrue()
        {
            switch (_valueType)
            {
                case JSValueType.Boolean:
                case JSValueType.Integer:
                    {
                        if (_iValue == 0)
                            return false;
                        return true;
                    }
                case JSValueType.Double:
                    {
                        if (_dValue == 0)
                            return false;
                        return true;
                    }
                case JSValueType.String:
                    {
                        if (_oValue == null || _oValue.ToString() == "")
                            return false;
                        return true;
                    }
                case JSValueType.Object:
                case JSValueType.Function:
                case JSValueType.Property:
                case JSValueType.Date:
                    {
                        if (_oValue == null)
                            return false;
                        return true;
                    }
                case JSValueType.Undefined:
                case JSValueType.Null:
                case JSValueType.NotExistsInObject:
                default:
                    return false;
            }
        }

        public JSValueType ValueType
        {
            get => _valueType;
            protected set
            {
                _valueType = value;
            }
        }

        public static JSValue ParseINode(INode data)
        {
            switch (data)
            {
                case NodeNull n:
                    return JSValue.nullValue;
                case NodeNumber n: 
                    if (n.Value.IsInteger) return new JSValue() { _valueType = JSValueType.Integer, Value = n.Value._iValue, _attributes = JSValueAttributesInternal.DoNotEnumerate };
                    return new JSValue() { _valueType = JSValueType.Double, Value = n.Value._dValue, _attributes = JSValueAttributesInternal.DoNotEnumerate};
                case NodeBoolean n: return new JSValue() { _valueType = JSValueType.Boolean, Value = n.Value, _attributes = JSValueAttributesInternal.DoNotEnumerate };
                case NodeArray n: return new JSValue() { _valueType = JSValueType.Object, _valueObjectSubtype = JSValueObjectType.ArrayType, Value = n.Array.Select(item => ParseINode(item)).ToArray(), _attributes = JSValueAttributesInternal.None };

            }
            return null;
        }

        public override string ToString()
        {
            if (_valueType == JSValueType.Object)
            {
                if (_oValue is IEnumerable arrayData)
                {
                    var sb = new StringBuilder("[");
                    foreach (var item in arrayData)
                    {
                        sb.Append(item.ToString());
                    }
                    sb.Append("]");
                }
            }

            return Value.ToString();
        }


        public IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int CompareTo(JSValue other)
        {
            throw new NotImplementedException();
        }
    }
}
