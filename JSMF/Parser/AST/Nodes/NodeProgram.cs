using System.Collections.Generic;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeProgram : INode
    {
        public NodeType Type { get; } = NodeType.Program;
        public List<INode> Program { get; set; } = new List<INode>();
    }
}
