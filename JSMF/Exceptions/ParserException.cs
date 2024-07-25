using JSMF.Parser.AST.Nodes;

namespace JSMF.Exceptions
{
    public class ParserException : JSException
    {
        public ParserException(string message, Position position) : base($"Uncaught SyntaxError: {message}", position)
        {
            
        }
    }
}
