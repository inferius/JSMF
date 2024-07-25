using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeWhile : NodeWithBody
    {
        public INode Condition { get; set; }
        public bool IsDoWhile { get; set; } = false;

        public NodeWhile()
        {
            Type = NodeType.While;
        }

        public override JSValue Evaluate(Scope context)
        {
            while (true)
            {
                var result = Condition.Evaluate(context);
                if (result.IsTrue())
                {
                    try
                    {
                        Body.Evaluate(context);
                    }
                    catch (Exceptions.EvaluateExceptions.BreakException)
                    {
                        break;
                    }
                    catch (Exceptions.EvaluateExceptions.ContinueException)
                    {
                        continue;
                    }
                }
            }
        

            return JSValue.undefined;
        }
    }
}
