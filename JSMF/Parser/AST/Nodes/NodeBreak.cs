using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNoValue : Node
    {
        public NodeNoValue(NodeType type)
        {
            Type = type;
        }

        public override JSValue Evaluate(Scope context)
        {
            if (Type == NodeType.Break)
            {
                throw new Exceptions.EvaluateExceptions.BreakException(context);
            }
            else if (Type == NodeType.Continue)
            {
                throw new Exceptions.EvaluateExceptions.ContinueException(context);
            }
            else
            {
                return JSValue.undefined;
            }
        }
    }
}
