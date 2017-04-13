namespace JSMF.Parser.AST.Nodes
{
    public class NodeNull : INode
    {
        public NodeType Type { get; } = NodeType.Null;

        public override string ToString()
        {
            return $"[{Type}]";
        }
    }
}
