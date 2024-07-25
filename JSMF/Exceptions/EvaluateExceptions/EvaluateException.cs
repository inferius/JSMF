using JSMF.Interpreter;
using System;

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
