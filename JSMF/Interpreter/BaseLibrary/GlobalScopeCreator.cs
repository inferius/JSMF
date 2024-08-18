using JSMF.Parser.AST.Nodes;
using JSMF.Parser.AST.Nodes.PredefinedNativeFuncions;

namespace JSMF.Interpreter.BaseLibrary
{
    public class GlobalScopeCreator
    {
        private Scope _globalScope = new Scope(null);
        public Scope GlobalScope => _globalScope;

        public GlobalScopeCreator()
        {
            RegisterConsoleFunctions();
        }

        private void RegisterConsoleFunctions()
        {
            var argsObject = new NodeJSObject();
            //var objectScope = new Scope { Parent = context.RootContext };
            argsObject.Values.Add(new NodeString("log"), new NodeNativeConsoleLog());
            _globalScope.Define(new Variable { Name = "console", Value = JSValue.ParseINode(argsObject), VarType = VarType.Let });
        }
    }
}