using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeObjectCall : Node
    {
        public INode Name { get; set; }
        public INode Child { get; set; }

        public NodeObjectCall()
        {
            Type = NodeType.ObjectCall;
        }

        public override JSValue Evaluate(Scope context)
        {
            //if (((NodeIdentifier)Name).Value == "console" && ((NodeObjectCall)Child).Name.Value == "log")
            {
                return Child.Evaluate(context);
            }
            // ignored
            //return JSValue.undefined;
        }
    }
}
