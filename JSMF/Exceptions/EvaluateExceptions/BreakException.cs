using JSMF.Interpreter;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class BreakException : EvaluateException
    {
        public BreakException(Scope currentScope) : base(currentScope, "Break")
        {
        }
    }
}
