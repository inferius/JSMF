using System;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNumber : INode
    {
        public Number Value { get; set; }
        public NodeType Type { get; } = NodeType.Number;

        public Position FileInfo {get; set; }

        public NodeNumber(Number value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }
    }
}
