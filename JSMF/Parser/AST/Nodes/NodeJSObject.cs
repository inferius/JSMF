using System;
using System.Collections.Generic;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeJSObject : INode
    {
        public NodeType Type { get; } = NodeType.JSObject;

        public Position FileInfo {get; set; }

        public Dictionary<INode, INode> Values = new Dictionary<INode, INode>();
    }
}
