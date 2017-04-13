using System;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeReturn : INode
    {
        public NodeType Type { get; } = NodeType.Return;
        public INode Body { get; set; }

        public Position FileInfo {get; set; }
    }
}
