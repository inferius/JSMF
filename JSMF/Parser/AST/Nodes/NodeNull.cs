using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNull : Node
    {
        public NodeNull()
        {
            Type = NodeType.Null;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"[{Type}]";
        }
    }
}
