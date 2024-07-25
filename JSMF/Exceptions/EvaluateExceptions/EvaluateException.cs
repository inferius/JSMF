using JSMF.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class EvaluateException : Exception
    {
        protected Scope _scope;

        public Scope Scope => _scope;

        public EvaluateException() { }

        public EvaluateException(Scope currentScope, string message) : base(message)
        {
            _scope = currentScope;
        }
    }
}
