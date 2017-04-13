using System;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNull : INode
    {
        public NodeType Type { get; } = NodeType.Null;

        public Position FileInfo {get; set; }

        public override string ToString()
        {
            return $"[{Type}]";
        }
    }
}
