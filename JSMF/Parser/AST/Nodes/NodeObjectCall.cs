using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeObjectCall : Node
    {
        public INode Name { get; set; }
        public INode Child { get; set; }

        public NodeObjectCall()
        {
            Type = NodeType.ObjectCall;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
