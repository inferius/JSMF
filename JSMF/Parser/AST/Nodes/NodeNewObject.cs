using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNewObject : INode
    {
        public NodeType Type { get; } = NodeType.New;
        public INode Object { get; set; }
    }
}
