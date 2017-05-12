using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeJSObject : Node
    {
        public Dictionary<INode, INode> Values = new Dictionary<INode, INode>();

        public NodeJSObject()
        {
            Type = NodeType.JSObject;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
