using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeCall : Node
    {
        public INode Function { get; set; }
        public List<INode> Arguments { get; set; } = new List<INode>();


        public NodeCall()
        {
            Type = NodeType.Call;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
