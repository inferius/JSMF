using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeClass : INode
    {
        public NodeType Type { get; } = NodeType.Class;

        public INode Name { get; set; }
        public bool IsAnonymous { get; set; }
        public INode Extends { get; set; }
        public INode Body { get; set; }
    }
}
