using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeForOf : Node
    {
        /// <summary>
        /// Pokud true je ForOf pokud false ForIn
        /// </summary>
        public bool IsForOf { get; set; } = true;
        public INode VarDef { get; set; }
        public INode Enumerate { get; set; }
        public INode Body { get; set; }

        public NodeForOf()
        {
            Type = NodeType.ForOf;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
