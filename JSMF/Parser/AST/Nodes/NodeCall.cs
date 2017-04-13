using System;
using System.Collections.Generic;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeCall : INode
    {
        public NodeType Type { get; } = NodeType.Call;
        public INode Function { get; set; }
        public List<INode> Arguments { get; set; } = new List<INode>();

        public Position FileInfo {get; set; }
    }
}
