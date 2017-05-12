using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
