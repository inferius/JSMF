using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeClass : Node
    {
        public INode Name { get; set; }
        public bool IsAnonymous { get; set; }
        public INode Extends { get; set; }
        public INode Body { get; set; }


        public NodeClass()
        {
            Type=NodeType.Class;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
