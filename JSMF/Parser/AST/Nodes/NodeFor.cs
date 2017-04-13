using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeFor : INode
    {
        public NodeType Type { get; } = NodeType.For;
        public INode VarDefs { get; set; }
        public INode Condition { get; set; }
        public INode Iterate { get; set; }
        public INode Body { get; set; }

        public Position FileInfo {get; set; }
    }
}
