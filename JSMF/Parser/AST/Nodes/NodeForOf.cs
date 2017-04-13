using System;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeForOf : INode
    {
        public NodeType Type { get; } = NodeType.ForOf;
        
        /// <summary>
        /// Pokud true je ForOf pokud false ForIn
        /// </summary>
        public bool IsForOf { get; set; } = true;
        public INode VarDef { get; set; }
        public INode Enumerate { get; set; }
        public INode Body { get; set; }

        public Position FileInfo {get; set; }
    }
}
