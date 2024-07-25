using JSMF.Interpreter;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class ContinueException : EvaluateException
    {
        public ContinueException(Scope currentScope) : base(currentScope, "Continue")
        {
        }
    }
}
