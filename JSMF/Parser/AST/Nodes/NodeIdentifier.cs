using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIdentifier : Node
    {
        public string Value { get; set; }

        public NodeIdentifier()
        {
            Type = NodeType.Identifier;
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }

        public override JSValue Evaluate(Scope context)
        {
            var v = context.Get(Value);
            // TODO: Zvazit jestli vracet undefined nebo chybu (zkontrolovat dokumentaci)
            if (v == null) return JSValue.undefined;

            return v.Value;
        }
    }
}
