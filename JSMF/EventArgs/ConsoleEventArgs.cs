using System.Collections;
using System.Collections.Generic;
using JSMF.Interpreter;
using JSMF.Parser.AST.Nodes;

namespace JSMF.EventArgs
{
    public class ConsoleEventArgs : System.EventArgs
    {
        public ConsoleEventArgsType Type { get; }
        public IEnumerable<INode> Arguments { get;  }
        public Scope CurrentScope { get; }
        public Scope GlobalScope { get; }

        public ConsoleEventArgs(ConsoleEventArgsType type, IEnumerable<INode> arguments, Scope currentScope, Scope globalScope)
        {
            Type = type;
            Arguments = arguments;
            CurrentScope = currentScope;
            GlobalScope = globalScope;
        }
    }
}