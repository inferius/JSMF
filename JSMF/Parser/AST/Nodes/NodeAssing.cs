using System;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeAssing : Node
    {
        public string Operator { get; set; }
        public INode Left { get; set; }
        public INode Right { get; set; }

        public NodeAssing()
        {
            Type = NodeType.Array;
        }

        public override JSValue Evaluate(Scope context)
        {
            if (Left is NodeVarDef nvd)
            {
                var var = new Variable {Name = nvd.Value, VarType = (VarType)Enum.Parse(typeof(VarType), nvd.VarType, true)};
                context.SetOrUpdate(var, JSValue.ParseINode(Right));
            }
            else if (Left is NodeIdentifier nodeIdentifier)
            {
                context.SetOrUpdate(context.Get(nodeIdentifier.Value, FileInfo), JSValue.ParseINode(Right));
            }

            return JSValue.undefined;
        }

        public override string ToString()
        {
            return $"[{Type}]{Left} {Operator} {Right}";
        }
    }
}
