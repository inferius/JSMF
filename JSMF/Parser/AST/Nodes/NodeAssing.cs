using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeAssing : Node
    {
        public string Operator { get; set; }
        public INode Left { get; set; }
        public INode Right { get; set; }

        public bool IsEvaluated { get; private set; }
        private JSValue EvaluatedValue { get; set; }

        public NodeAssing()
        {
            Type = NodeType.Array;
        }

        public override JSValue Evaluate(Scope context)
        {
            if (!IsEvaluated)
            {
                if (Left is NodeVarDef nvd)
                {
                    var var = new Variable
                        { Name = nvd.Value, VarType = (VarType)Enum.Parse(typeof(VarType), nvd.VarType, true) };
                    if (Right is NodeBinary right)
                    {
                        context.SetOrUpdate(var, right.Evaluate(context));
                    }
                    else
                    {
                        context.SetOrUpdate(var, JSValue.ParseINode(Right));
                    }
                }
                else if (Left is NodeIdentifier nodeIdentifier)
                {
                    if (Right is NodeBinary right)
                    {
                        context.SetOrUpdate(context.Get(nodeIdentifier.Value, FileInfo), right.Evaluate(context));
                    }
                    else
                    {
                        context.SetOrUpdate(context.Get(nodeIdentifier.Value, FileInfo), JSValue.ParseINode(Right));
                    }
                }

                IsEvaluated = true;
            }

            return JSValue.undefined;
        }

        public override string ToString()
        {
            return $"[{Type}]{Left} {Operator} {Right}";
        }
    }
}
