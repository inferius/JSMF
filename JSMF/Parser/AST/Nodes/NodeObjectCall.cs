using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeObjectCall : Node
    {
        public INode Name { get; set; }
        public INode Child { get; set; }

        public NodeObjectCall()
        {
            Type = NodeType.ObjectCall;
        }

        public override JSValue Evaluate(Scope context)
        {
            if (Name is NodeIdentifier name && Child is NodeObjectCall child)
            {
                return child.Evaluate(context, context.Get(name.Value, FileInfo));
            }
            
            return JSValue.undefined;
        }
        public JSValue Evaluate(Scope context, Variable parent = null)
        {
            if (Name is NodeIdentifier name)
            {
                if (Child is NodeObjectCall child)
                {
                    return child.Evaluate(context, context.Get(name.Value, FileInfo));
                }

                if (parent?.Value.Value is NodeJSObject parentCall)
                {
                    if (Child is NodeCall childCall)
                    {
                        return Call(context, parentCall);
                    }

                    if (Child is NodeObjectCall childObjCall)
                    {
                        return childObjCall.Evaluate(context, parentCall);
                    }
                }

                //if (((NodeIdentifier)Name).Value == "console" && ((NodeObjectCall)Child).Name.Value == "log")
                //{
                //    return Child.Evaluate(context);
                //}
                // ignored
                //return JSValue.undefined;
            }
            
            return JSValue.undefined;
        }

        public JSValue Call(Scope context, NodeJSObject node)
        {
            if (Child is NodeCall childCall && Name is NodeIdentifier name)
            {
                if (node[name.Value] is NodeNativeFunction nodeNative)
                {
                    nodeNative.Arguments = [..childCall.Arguments];
                    return nodeNative.Evaluate(context);
                }
            }
            
            return JSValue.undefined;
        }
        
        public JSValue Evaluate(Scope context, NodeJSObject parent = null)
        {
            if (Name is NodeIdentifier name)
            {
                if (Child is NodeObjectCall child)
                {
                    if (parent[name] is NodeJSObject parentCall)
                    {
                        return child.Evaluate(context, parentCall);
                    }
                }

                if (Child is NodeCall childCall)
                {
                    if (parent[name] is NodeNativeFunction nodeNative)
                    {
                        nodeNative.Arguments = [..childCall.Arguments];
                        return childCall.Evaluate(context);
                    }
                }
            }
            
            return JSValue.undefined;
        }
    }
}
