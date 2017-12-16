using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public interface INode
    {
        NodeType Type { get; }
        Position FileInfo { get; }
        JSValue Evaluate(Scope context);
    }
}
