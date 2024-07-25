using JSMF.Interpreter;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class YieldException : EvaluateException
    {
        private JSValue _returnValue;
        public JSValue ReturnValue => _returnValue;

        public YieldException(Scope currentScope, JSValue returnValue) : base(currentScope, "Yield")
        {
            _returnValue = returnValue;
        }
    }

}