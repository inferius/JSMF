using System;
using System.Collections.Generic;
using JSMF.EventArgs;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Interpreter
{
    public class Runner
    {
        internal static Scope GlobalScope;

        public static event EventHandler<ConsoleEventArgs> ConsoleEvents;

        public Runner(Scope globalScope = null)
        {
            GlobalScope = globalScope ?? new Scope(null);
        }

        public JSValue Run(INode node, Scope scope = null)
        {
            if (node is NodeProgram program)
            {
                /*foreach (var programLine in program.Program)
                {
                    switch (programLine)
                    {
                        case NodeCall nodeCall:
                        case NodeAssing nodeAssing:
                        //case NodeAwaitableCall nodeAwaitableCall:
                            programLine.Evaluate(GlobalScope);
                            break;
                    }
                }*/
                return program.Evaluate(null);
            }

            /*if (node is NodeReturn returnNode)
            {
                return returnNode.Evaluate(scope ?? GlobalScope);
            }*/
            return JSValue.undefined;
        }
        
        
        public static void RaiseConsoleEvents(ConsoleEventArgsType type, IEnumerable<INode> arguments, Scope scope, Scope globalScope)
        {
            ConsoleEvents?.Invoke(null, new ConsoleEventArgs(type, arguments, scope, globalScope));
        }
    }
}
