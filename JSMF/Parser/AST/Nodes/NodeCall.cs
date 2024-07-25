using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeCall : Node
    {
        public INode Function { get; set; }
        public List<INode> Arguments { get; set; } = new List<INode>();


        public NodeCall()
        {
            Type = NodeType.Call;
        }

        public override JSValue Evaluate(Scope context)
        {
            if (Function is NodeFunction nodeFunction)
            {
                using (var functionContext = context.Extend())
                {
                    var i = 0;
                    foreach (NodeIdentifier arg in nodeFunction.Arguments)
                    {
                        functionContext.Define(new Variable { Name = arg.Value, Value = Arguments.Count <= i ? JSValue.undefined : JSValue.ParseINode(Arguments[i]), VarType = VarType.Let });
                    }

                    nodeFunction?.Body?.Evaluate(functionContext);
                }
            }
            else if (Function is NodeNativeFunction nodeNativeFunction)
            {
                nodeNativeFunction.Arguments = Arguments;
                return nodeNativeFunction.Evaluate(context);
            }

            return JSValue.undefined;
        }
    }
}
