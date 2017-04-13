using System.Collections.Generic;

namespace JSMF.Parser.AST
{
    public static class AstTreeMethods
    {
        public static Dictionary<string, int> OperatorsPriority = new Dictionary<string, int>
        {
            {"=", 1 },
            {"||", 2 },
            {"&&", 3 },
            { "<", 7 }, { ">", 7 }, { "<=", 7 }, { ">=", 7 }, { "==", 7 }, { "!=", 7 },
            { "+", 10 }, { "-", 10 }, {"++", 10}, {"--", 10},
            { "*", 20 }, { "/", 20 }, { "%", 20 },
            { "**", 30 }
        };
    }
}
