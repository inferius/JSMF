using System;
using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeJSObject : Node
    {
        public Dictionary<INode, INode?> Values = new();

        public NodeJSObject()
        {
            Type = NodeType.JSObject;
        }

        private INode? GetValue(NodeIdentifier node)
        {
            return GetValue(node.Value);
        }
        
        private INode? GetValue(string value)
        {
            foreach (var item in Values)
            {
                switch (item.Key)
                {
                    case NodeIdentifier key when value == key.Value:
                        return item.Value;
                    case NodeString keyString when value == keyString.Value:
                        return item.Value;
                }
            }

            return null;
        }

        private void SetValue(NodeIdentifier node, INode? value)
        {
            SetValue(node.Value, value);
        }
        
        private void SetValue(string keyName, INode? value)
        {
            var node = GetValue(keyName);
            if (node == null)
            {
                Values.Add(new NodeIdentifier(keyName), value);
            }
            else
            {
                foreach (var item in Values)
                {
                    var keyStr = item.Key switch
                    {
                        NodeIdentifier key => key.Value,
                        NodeString keyString => keyString.Value,
                        _ => ""
                    };

                    if (keyName == keyStr)
                    {
                        Values[item.Key] = value;
                        break;
                    }
                }
            }
        }
        
        public INode? this[NodeIdentifier node] { get => GetValue(node); set => SetValue(node, value); }
        public INode? this[string node] { get => GetValue(node); set => SetValue(node, value); }

        public override JSValue Evaluate(Scope context)
        {
            throw new NotImplementedException();
        }
    }
}
