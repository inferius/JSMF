using JSMF.Interpreter;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Core
{
    public static class Tools
    {

        public static string Printer(INode? node, Scope scope, Scope globalScope)
        {
            switch (node)
            {
                case NodeIdentifier identifier:
                {
                    var variable = scope.Get(identifier.Value, null);
                    if (variable.Value.ValueType == JSValueType.EvaluatableObject)
                    {
                        if (variable.Value.Value is NodeBinary nodeBinary)
                        {
                            return nodeBinary.Evaluate(scope).ToString();
                        }
                    }
                    return variable.Value?.ToString() ?? "null";
                }
                case NodeString nodeString:
                    return nodeString.Value;
                case NodeNumber nodeNumber:
                    return nodeNumber.Value.ToString();
                case NodeBoolean nodeBoolean:
                    return nodeBoolean.Value ? "true" : "false";
                case NodeCall nodeCall:
                    return nodeCall.Evaluate(scope).ToString();
                default:
                    return node?.ToString() ?? "undefined";
            }
        }
    }
}
