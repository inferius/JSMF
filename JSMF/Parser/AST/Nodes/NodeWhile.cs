namespace JSMF.Parser.AST.Nodes
{
    public class NodeWhile : INode
    {
        public NodeType Type { get; } = NodeType.While;
        public INode Condition { get; set; }
        public bool IsDoWhile { get; set; } = false;
        public INode Body { get; set; }
    }
}
