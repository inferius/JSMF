using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Exceptions
{
    public class ParserException : JSException
    {
        public ParserException(string message, int line, int column) : base($"Uncaught SyntaxError: {message}", line,
            column)
        {
            
        }
    }
}
