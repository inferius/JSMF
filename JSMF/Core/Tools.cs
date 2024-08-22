using System.IO;
using System.Text;
using JSMF.Interpreter;
using JSMF.Parser.AST.Nodes;
using JSMF.Parser.Tokenizer;

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
                case NodeBinary nodeCall:
                    return nodeCall.Evaluate(scope).ToString();
                default:
                    return node?.ToString() ?? "undefined";
            }
        }

        public static void TokenStringDebugPrinter(TokenStream tokenStrem, string target, string? stylePath = null)
        {
            var str = new StringBuilder();
            
            if (stylePath != null) str.Append($"<link rel='stylesheet' href='{stylePath}'>");

            while (!tokenStrem.Eof())
            {
                var token = tokenStrem.Next();
                if (token.Type == TokenType.Separator && token.Value == "LB")
                {
                    str.Append("<br>");
                    continue;
                }
                str.Append($"<span class='token {token.Type}' title='{token.Type}:{token.Position}'>{token.Value}</span>");
            }
            
            File.WriteAllText(target, str.ToString());
            
        }
    }
}
