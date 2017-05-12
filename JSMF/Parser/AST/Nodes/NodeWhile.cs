using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeWhile : Node
    {
        public INode Condition { get; set; }
        public bool IsDoWhile { get; set; } = false;
        public INode Body { get; set; }

        public NodeWhile()
        {
            Type = NodeType.While;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
