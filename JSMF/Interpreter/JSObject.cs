using System.Collections.Generic;

namespace JSMF.Interpreter
{
    public class JSObject : JSValue
    {
        internal IDictionary<string, JSValue> _fields;
        internal IDictionary<Symbol, JSValue> _symbols;
    }
}
