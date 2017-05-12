using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeAssing : Node
    {
        public string Operator { get; set; }
        public INode Left { get; set; }
        public INode Right { get; set; }

        public NodeAssing()
        {
            Type = NodeType.Array;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"[{Type}]{Left} {Operator} {Right}";
        }
    }
}
