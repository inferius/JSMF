using System.Collections.Generic;
using JSMF.EventArgs;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes.PredefinedNativeFuncions
{
    public class NodeNativeConsoleLog : NodeNativeFunction
    {
        public override JSValue Evaluate(Scope context)
        {
            Runner.RaiseConsoleEvents(ConsoleEventArgsType.ConsoleLog, Arguments, context, Runner.GlobalScope);

            return JSValue.undefined;
        }
    }
}