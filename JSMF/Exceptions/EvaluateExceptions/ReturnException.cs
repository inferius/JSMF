﻿using JSMF.Interpreter;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class ReturnException : EvaluateException
    {
        private JSValue _returnValue;
        public JSValue ReturnValue => _returnValue;

        public ReturnException(Scope currentScope, JSValue returnValue) : base(currentScope, "Return")
        {
            _returnValue = returnValue;
        }
    }
}
