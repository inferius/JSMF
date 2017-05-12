using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeProgram : Node
    {
        public List<INode> Program { get; set; } = new List<INode>();

        public NodeProgram()
        {
            Type = NodeType.Program;
        }
        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
