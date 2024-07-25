using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeFor : NodeWithBody
    {
        public INode VarDefs { get; set; }
        public INode Condition { get; set; }
        public INode Iterate { get; set; }

        public NodeFor()
        {
            Type = NodeType.For;
        }

        public override JSValue Evaluate(Scope context)
        {
            //for ()

            return JSValue.undefined;
        }
    }
}
