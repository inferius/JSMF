using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
