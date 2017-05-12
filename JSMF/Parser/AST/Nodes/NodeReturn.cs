using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeReturn : Node
    {
        public INode Body { get; set; }

        public NodeReturn()
        {
            Type = NodeType.Return;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
