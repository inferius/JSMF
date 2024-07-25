using JSMF.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Exceptions.EvaluateExceptions
{
    public class BreakException : EvaluateException
    {
        public BreakException(Scope currentScope) : base(currentScope, "Break")
        {
        }
    }
}
