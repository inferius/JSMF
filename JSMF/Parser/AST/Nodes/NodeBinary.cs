using System;
using JSMF.Exceptions;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeBinary : Node
    {
        public string Operator { get; set; }
        public INode Left { get; set; }
        public INode Right { get; set; }

        public NodeBinary()
        {
            Type = NodeType.Binary;
        }

        public override JSValue Evaluate(Scope context)
        {
            var l = Left.Evaluate(context);
            var r = Right.Evaluate(context);
            switch (Operator)
            {
                case "+":
                    return l + r;
                case "-":
                    return l - r;
                case "+=":
                    if (Left is NodeIdentifier nodeIdentifier)
                    {
                        context.Set(nodeIdentifier.Value, l + r);
                        return context.Get(nodeIdentifier.Value).Value;
                    }
                    ExceptionHelper.ThrowTypeError("Left side of assignment must be a variable");
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
