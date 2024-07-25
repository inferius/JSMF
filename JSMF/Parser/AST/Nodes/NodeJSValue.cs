using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeJSValue : Node
    {
        public NodeJSValue()
        {
            Type = NodeType.JSValue;
        }
        
        public NodeJSValue(JSValue value) : this()
        {
            Value = value;
        }
        public JSValue Value { get; set; }
        public override JSValue Evaluate(Scope context)
        {
            return Value;
        }
    }
}