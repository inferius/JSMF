using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Exceptions
{
    public class JSException: Exception
    {
        public int Column { get; private set; }
        public int Line { get; private set; }

        public JSException()
        {
            Column = -1;
            Line = -1;
        }

        public JSException(string message, int line, int column) : base($"{message}: at {line + 1}:{column}")
        {
            Line = line;
            Column = column;
        }
    }
}
