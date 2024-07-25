using JSMF.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class ContinueException : EvaluateException
    {
        public ContinueException(Scope currentScope) : base(currentScope, "Continue")
        {
        }
    }
}
