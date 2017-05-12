using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNewObject : Node
    {
        public INode Object { get; set; }

        public NodeNewObject()
        {
            Type=NodeType.New;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
