using System.Collections.Generic;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeProperty : INode
    {
        public NodeType Type { get; } = NodeType.Property;
        public bool IsAsync { get; set; }
        public INode Name { get; set; }
        public INode Body { get; set; }
        public List<INode> Arguments { get; set; }
        public bool IsGetter { get; set; }
    }
}
