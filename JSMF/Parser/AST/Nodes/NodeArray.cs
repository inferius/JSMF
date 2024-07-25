using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeArray : Node
    {
        public List<INode> Array { get; set; } = new List<INode>();

        public NodeArray()
        {
            Type = NodeType.Array;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
