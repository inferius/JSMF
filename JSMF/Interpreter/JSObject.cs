using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Interpreter
{
    public class JSObject : JSValue
    {
        internal IDictionary<string, JSValue> _fields;
        internal IDictionary<Symbol, JSValue> _symbols;
    }
}
