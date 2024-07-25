using System.Collections.Generic;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeProgram : Node
    {
        public INode Caller { get; set; }
        public List<INode> Program { get; set; } = new List<INode>();

        public NodeProgram()
        {
            Type = NodeType.Program;
        }
        public override JSValue Evaluate(Scope context)
        {
            /*if (Caller is NodeGenerator generator)
            {
                return JSValue.ParseINode(generator.Next(context));
            }*/
            var aContext = new Scope(context);
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
                return e.ReturnValue;
            }
            catch (Exceptions.EvaluateExceptions.YieldException e)
            {
                return e.ReturnValue;
            }

            return JSValue.undefined;
        }
    }
}
