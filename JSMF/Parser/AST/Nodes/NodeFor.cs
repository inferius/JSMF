using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeFor : Node
    {
        public INode VarDefs { get; set; }
        public INode Condition { get; set; }
        public INode Iterate { get; set; }
        public INode Body { get; set; }

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
