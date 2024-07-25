using JSMF.Parser.AST.Nodes;

namespace JSMF.Interpreter
{
    public class Runner
    {
        private readonly Scope GlobalScope = new Scope(null);

        public Runner()
        {
            
        }

        public void Run(INode node)
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
                program.Evaluate(GlobalScope);
            }
            
        }
        
        public static JSValue RunInScope(INode node, Scope scope)
        {
            if (node is NodeProgram program)
            {
                return program.Evaluate(scope);
            }
            return JSValue.undefined;
        }
    }
}
