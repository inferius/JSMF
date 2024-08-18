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
        
        public NodeIdentifier(string value): this()
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }

        public override JSValue Evaluate(Scope context)
        {
            var v = context.Get(Value, FileInfo);
            // TODO: Zvazit jestli vracet undefined nebo chybu (zkontrolovat dokumentaci)
            if (v == null) return JSValue.undefined;

            return v.Value;
        }
    }
}
