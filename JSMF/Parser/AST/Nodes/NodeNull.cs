﻿using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeNull : Node
    {
        public NodeNull()
        {
            Type = NodeType.Null;
        }

        public override JSValue Evaluate(Scope context)
        {
            return JSValue.nullValue;
        }

        public override string ToString()
        {
            return $"[{Type}]";
        }
    }
}
