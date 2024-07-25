using System;
using System.Collections;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeForOf : Node
    {
        /// <summary>
        /// Pokud true je ForOf pokud false ForIn
        /// </summary>
        public bool IsForOf { get; set; } = true;
        public INode VarDef { get; set; }
        public INode Enumerate { get; set; }
        public INode Body { get; set; }

        public NodeForOf()
        {
            Type = NodeType.ForOf;
        }

        public override JSValue Evaluate(Scope context)
        {
            if (VarDef is NodeIdentifier varName && Enumerate is NodeIdentifier varEnumerate)
            {
                if (IsForOf)
                {
                    var array = context.Get(varEnumerate.Value);

                    if (array.Value._oValue is IEnumerable arrayData)
                    {
                        var c = new Scope(context);
                        foreach (var item in arrayData)
                        {
                            c.SetOrUpdate(new Variable { Name = varName.Value, VarType = VarType.Let }, (JSValue)item);
                            if (Body is NodeProgram)
                            {
                                Body.Evaluate(c);
                            }
                        }
                    }

                }
            }

            return JSValue.undefined;
        }
    }
}
