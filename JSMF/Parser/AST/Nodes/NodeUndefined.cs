using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeUndefined : Node
    {
        public NodeUndefined()
        {
            Type = NodeType.Null;
        }

        public override JSValue Evaluate(Scope context)
        {
            return JSValue.undefined;
        }

        public override string ToString()
        {
            return $"[{Type}]";
        }
    }
}
