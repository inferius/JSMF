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
                return CallFunction(context, nodeFunction);
            }

            if (Function is NodeNativeFunction nodeNativeFunction)
            {
                nodeNativeFunction.Arguments = Arguments;
                return nodeNativeFunction.Evaluate(context);
            }

            if (Function is NodeIdentifier nodeIdentifier)
            {
                var fnc = context.GetFunction(nodeIdentifier.Value);
                return CallFunction(context, fnc);
            }

            return JSValue.undefined;
        }
        
        private JSValue CallFunction(Scope context, NodeFunction nodeFunction)
        {
            using (var functionContext = context.Extend())
            {
                var i = 0;
                var argsObject = new NodeJSObject();
                //var objectScope = new Scope { Parent = context.RootContext };
                argsObject.Values.Add(new NodeString("length"), new NodeNumber(Arguments.Count));
                // TODO: Zpracovat Symboly a generatory
                //argsObject.Values.Add(new NodeSymbol(SymbolTypes.Iterator), new NodeFunction { IsGenerator = true, Body = new NodeBlock { Statements = new List<INode> { new NodeSymbol(SymbolTypes.Yield, new NodeIdentifier("this")) } };
                argsObject.Values.Add(new NodeString("callee"), Function);

                foreach (NodeIdentifier nodeIdentifier in nodeFunction.Arguments)
                {
                    functionContext.Define(new Variable { Name = nodeIdentifier.Value, Value = JSValue.undefined, VarType = VarType.Let });
                }
                 
                foreach (INode arg in Arguments)
                {
                    if (i < nodeFunction.Arguments.Count)
                    {
                        var identifier = nodeFunction.Arguments[i] as NodeIdentifier;
                        functionContext.SetOrUpdate(functionContext.Get(identifier.Value), JSValue.ParseINode(Arguments[i]));
                    }
                    
                    argsObject.Values.Add(new NodeNumber(i), Arguments[i]);
                    i++;
                }

                functionContext.Define(new Variable { Name = "arguments", Value = JSValue.ParseINode(argsObject), VarType = VarType.Let });
                return Runner.RunInScope(nodeFunction.Body, functionContext);
            }
        }
    }
    
    
}
