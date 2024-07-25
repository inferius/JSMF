using JSMF.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNativeFunction : Node
    {
        public BlockExpression Body;
        public List<INode> Arguments { get; set; } = new List<INode>();

        public override JSValue Evaluate(Scope context)
        {
            return Expression.Lambda<Func<JSValue[], JSValue>>(Body).Compile()(Arguments.Select(item => JSValue.ParseINode(item)).ToArray());

            //return JSValue.undefined;
        }
    }
}
