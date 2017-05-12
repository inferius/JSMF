using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeString : Node
    {
        public string Value { get; set; }

        public NodeString()
        {
            Type = NodeType.String;
        }
        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }
    }
}
