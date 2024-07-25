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
        
        public NodeString(string value): this()
        {
            Value = value;
        }
        public override JSValue Evaluate(Scope context)
        {
            return JSValue.ParseINode(this);
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }
    }
}
