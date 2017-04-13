using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeAwaitableCall : INode
    {
        public NodeType Type { get; } = NodeType.Await;
        public INode WaitFor { get; set; }
    }
}
