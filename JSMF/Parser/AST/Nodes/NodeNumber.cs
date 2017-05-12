using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNumber : Node
    {
        public Number Value { get; set; }


        public NodeNumber(Number value)
        {
            Type=NodeType.Number;
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
