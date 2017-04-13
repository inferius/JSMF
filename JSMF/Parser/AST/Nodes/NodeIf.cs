namespace JSMF.Parser.AST.Nodes
{
    public class NodeIf : INode
    {
        public NodeType Type { get; } = NodeType.Condition;
        public INode Condition { get; set; }
        public INode Then { get; set; }
        public INode Else { get; set; }
    }
}
