using System.Collections.Generic;
using System.Text;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeGenerator : NodeFunction
    {
        public bool IsDone { get; set; } = false;
        
        private int _index = 0;
        public NodeGenerator()
        {
            Type = NodeType.Generator;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (IsAsync) sb.Append("async ");
            sb.Append("function* ");
            if (Function is NodeIdentifier)
            {
                if (!IsAnonymous) sb.Append(((NodeIdentifier) Function).Value);
            }

            return sb.ToString();
        }

        public NodeJSObject Next(Scope context)
        {
            if (Body == null || IsDone)
            {
                IsDone = true;
                return GetJSObject(new NodeUndefined());
            }
            
            var aContext = context.Extend();
            try
            {
                if (Body is NodeProgram program)
                {
                    for (; _index < program.Program.Count; _index++)
                    {
                        var node = program.Program[_index];
                        node.Evaluate(aContext);
                    }
                }
            }
            catch (Exceptions.EvaluateExceptions.ReturnException e)
            {
                IsDone = true;
                return GetJSObject(new NodeJSValue(e.ReturnValue));
            }
            catch (Exceptions.EvaluateExceptions.YieldException e)
            {
                return GetJSObject(new NodeJSValue(e.ReturnValue));
            }

            return GetJSObject(new NodeUndefined());
        }
        
        private NodeJSObject GetJSObject(INode value)
        {
            var jsObject = new NodeJSObject();
            jsObject.Values.Add(new NodeString("done"), new NodeBoolean(IsDone));
            jsObject.Values.Add(new NodeString("value"), value);
            return jsObject;
        }
        
        public override JSValue Evaluate(Scope context)
        {
            if (IsAnonymous)
            {
                
            }
            else
            {
                context.CreateFunction(((NodeIdentifier)Function).Value, this);
            }
            //context.CreateFunction();
            //if (Body == null) return JSValue.undefined;
            return JSValue.undefined;
        }
    }
}