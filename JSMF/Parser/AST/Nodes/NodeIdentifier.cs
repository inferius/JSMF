using System;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIdentifier : INode
    {
        public NodeType Type { get; } = NodeType.Identifier;
        public string Value { get; set; }

        public Position FileInfo {get; set; }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }
    }
}
