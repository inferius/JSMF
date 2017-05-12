using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Interpreter.BaseLibrary
{
    public static class JITHelper
    {
        //public static readonly ParameterExpression DynamicValuesParameter = Expression.Parameter(typeof(CodeNode[]), "dv");
        public static readonly ParameterExpression ContextParameter = Expression.Parameter(typeof(Scope), "context");
    }
}
