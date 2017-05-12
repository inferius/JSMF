using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIf : Node
    {
        public INode Condition { get; set; }
        public INode Then { get; set; }
        public INode Else { get; set; }

        public NodeIf()
        {
            Type=NodeType.Condition;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
