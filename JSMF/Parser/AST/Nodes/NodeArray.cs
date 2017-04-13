using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeArray : INode
    {
        public List<INode> Array { get; set; } = new List<INode>();

        public NodeType Type => NodeType.Array;

        public Position FileInfo {get; set; }
    }
}
