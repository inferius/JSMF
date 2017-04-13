using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIncDecOperator : INode
    {
        public NodeType Type { get; } = NodeType.IncDecOperator;
        public INode Identifier { get; set; }
        public string Operator { get; set; }
        public bool AfterVar { get; set; } = false;

        public Position FileInfo {get; set; }
    }
}
