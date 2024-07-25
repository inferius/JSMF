using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeProperty : NodeWithBody
    {
        public bool IsAsync { get; set; }
        public INode Name { get; set; }
        public List<INode> Arguments { get; set; }
        public bool IsGetter { get; set; }

        public NodeProperty()
        {
            Type = NodeType.Property;
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
