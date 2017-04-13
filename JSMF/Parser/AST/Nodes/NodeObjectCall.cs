using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeObjectCall : INode
    {
        public NodeType Type { get; } = NodeType.ObjectCall;
        public INode Name { get; set; }
        public INode Child { get; set; }

        public Position FileInfo {get; set; }
    }
}
