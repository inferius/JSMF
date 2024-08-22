using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeProgram : Node
    {
        public INode Caller { get; set; }
        public List<INode> Program { get; set; } = new List<INode>();
        
        public Scope CurrentEvaluatingScope { get; private set; }
        public bool ThrowExceptions { get; set; } = false;
        
        public bool NoExtendScope { get; set; } = false;

        public NodeProgram()
        {
            Type = NodeType.Program;
        }
        public override JSValue Evaluate(Scope? context)
        {
            /*if (Caller is NodeGenerator generator)
            {
                return JSValue.ParseINode(generator.Next(context));
            }*/
            var aContext = context != null ? (NoExtendScope ? context : context.Extend()) : Runner.GlobalScope;
            CurrentEvaluatingScope = aContext;
            try
            {
                foreach (var node in Program)
                {
                    node.Evaluate(aContext);

                    //if (node is NodeReturn) return result;
                }
            }
            catch (Exceptions.EvaluateExceptions.ReturnException e)
            {
                return !ThrowExceptions ? e.ReturnValue : throw e;
            }
            catch (Exceptions.EvaluateExceptions.YieldException e)
            {
                return !ThrowExceptions ? e.ReturnValue : throw e;
            }

            return JSValue.undefined;
        }
    }
}
