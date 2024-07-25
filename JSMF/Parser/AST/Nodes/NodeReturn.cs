using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeReturn : NodeWithBody
    {
        public NodeReturn()
        {
            Type = NodeType.Return;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new Exceptions.EvaluateExceptions.ReturnException(context, Body.Evaluate(context));
        }
    }
}
