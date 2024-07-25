using System;
using System.Linq.Expressions;

namespace JSMF.Interpreter
{
    public class Function : JSObject
    {
        public bool IsAsync { get; set; }
        private BlockExpression Body;


        public void Invoke(Scope callingScope, JSValue[] parameters)
        {
            Expression.Lambda<Func<JSValue[], JSValue>>(Body).Compile()(parameters);
        }
    }
}
