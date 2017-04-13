namespace JSMF.Parser.AST.Nodes
{
    public class NodeAssing : INode
    {
        public NodeType Type { get; } = NodeType.Assign;
        public string Operator { get; set; }
        public INode Left { get; set; }
        public INode Right { get; set; }

        public override string ToString()
        {
            return $"[{Type}]{Left} {Operator} {Right}";
        }
    }
}
