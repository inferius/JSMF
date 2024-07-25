using System;
using JSMF.Exceptions;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIncDecOperator : Node
    {
        public NodeIdentifier Identifier { get; set; }
        public string Operator { get; set; }
        public bool AfterVar { get; set; } = false;

        public NodeIncDecOperator()
        {
            Type=NodeType.IncDecOperator;
        }

        public override JSValue Evaluate(Scope context)
        {
            var val = context.Get(Identifier.Value);
            if (val.Value.ValueType == JSValueType.Integer || val.Value.ValueType == JSValueType.Double)
            {
                if (val.Value.ValueType == JSValueType.Integer)
                {
                    if (Operator == "++")
                    {
                        val.Value._iValue++;
                    }
                    else
                    {
                        val.Value._iValue--;
                    }
                }
                else
                {
                    if (Operator == "++")
                    {
                        val.Value._dValue++;
                    }
                    else
                    {
                        val.Value._dValue--;
                    }
                }

                context.Set(Identifier.Value, val.Value);
            }

            return val.Value;
        }
    }
}
