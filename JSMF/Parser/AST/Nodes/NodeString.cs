namespace JSMF.Parser.AST.Nodes
{
    public class NodeString : INode
    {
        public string Value { get; set; }
        public NodeType Type { get; } = NodeType.String;

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }
    }
}
