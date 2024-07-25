using System;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Exceptions
{
    public class JSException: Exception
    {
        public Position Position { get; private set; }
        public JSException(string message, Position position) : base($"{message}: at {position}")
        {
            Position = position;
        }
    }
}
