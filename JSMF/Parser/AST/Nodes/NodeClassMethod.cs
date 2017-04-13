using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    class NodeClassMethod : NodeFunction
    {
        public bool IsStatic { get; set; } = false;
    }
}
