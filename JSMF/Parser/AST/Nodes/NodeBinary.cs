namespace JSMF.Parser.AST.Nodes
{
    public class NodeBinary : INode
    {
        public NodeType Type { get; } = NodeType.Binary;
        public string Operator { get; set; }
        public INode Left { get; set; }
        public INode Right { get; set; }
    }
}
