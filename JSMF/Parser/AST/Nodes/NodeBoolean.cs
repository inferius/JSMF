﻿using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeBoolean : Node
    {
        public bool Value { get; set; }

        public NodeBoolean()
        {
            Type = NodeType.Boolean;
        }

        public override string ToString()
        {
            return $"[{Type}]{Value}";
        }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
