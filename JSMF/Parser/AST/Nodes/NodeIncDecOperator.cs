using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIncDecOperator : Node
    {
        public INode Identifier { get; set; }
        public string Operator { get; set; }
        public bool AfterVar { get; set; } = false;

        public NodeIncDecOperator()
        {
            Type=NodeType.IncDecOperator;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
