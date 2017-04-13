using System;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeBoolean : INode
    {
        public NodeType Type { get; } = NodeType.Boolean;

        public bool Value { get; set; }

        public Position FileInfo {get; set; }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }
    }
}
