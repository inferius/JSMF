using System.Collections.Generic;
using System.Text;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeFunction : NodeWithBody
    {
        public INode Function { get; set; }

        public bool IsAsync { get; set; } = false;
        public bool IsAnonymous { get; set; } = false;

        public List<INode> Arguments { get; set; } = new List<INode>();

        public NodeFunction()
        {
            Type = NodeType.Function;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (IsAsync) sb.Append("async ");
            sb.Append("function ");
            if (Function is NodeIdentifier)
            {
                if (!IsAnonymous) sb.Append(((NodeIdentifier) Function).Value);
            }

            return sb.ToString();
        }

        public override JSValue Evaluate(Scope context)
        {
            if (IsAnonymous)
            {
                
            }
            else
            {
                context.CreateFunction(((NodeIdentifier)Function).Value, this);
            }
            //context.CreateFunction();
            //if (Body == null) return JSValue.undefined;
            return JSValue.undefined;
        }
    }
}
