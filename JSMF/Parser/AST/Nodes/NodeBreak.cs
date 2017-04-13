using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNoValue : INode
    {
        public NodeType Type { get; }

        public Position FileInfo {get; set; }

        public NodeNoValue(NodeType type)
        {
            Type = type;
        }
    }
}
