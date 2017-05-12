using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeAwaitableCall : Node
    {
        public INode WaitFor { get; set; }

        public NodeAwaitableCall()
        {
            Type = NodeType.Await;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
