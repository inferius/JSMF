using System.Linq;
using JSMF.Exceptions;
using JSMF.Exceptions.EvaluateExceptions;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeFor : NodeWithBody
    {
        public INode VarDefs { get; set; }
        public INode Condition { get; set; }
        public INode Iterate { get; set; }

        public NodeFor()
        {
            Type = NodeType.For;
        }

        public override JSValue Evaluate(Scope context)
        {
            var aScope = context.Extend();

            SetProgramData(VarDefs);
            SetProgramData(Condition);
            SetProgramData(Body);
            SetProgramData(Iterate);
            
            VarDefs.Evaluate(aScope);
            
            while (true)
            {
                try
                {
                    if (Condition is NodeProgram condition)
                    {
                        var result = condition.Program.Last()?.Evaluate(aScope);
                        if (!result?.IsTrue() ?? throw new JSException("Condition not valid", FileInfo)) break;
                    }
                }
                catch (ReturnException e)
                {
                    if (!e.ReturnValue.IsTrue())
                    {
                        break;
                    }
                }
                try
                {
                    Body.Evaluate(aScope);
                    Iterate.Evaluate(aScope);
                }
                catch (ReturnException e)
                {
                    return e.ReturnValue;
                }
                catch (BreakException e)
                {
                    break;
                }
                catch (ContinueException e)
                {
                    continue;
                }
            }

            return JSValue.undefined;
        }

        private static void SetProgramData(INode node)
        {
            if (node is NodeProgram program)
            {
                program.NoExtendScope = true;
                program.ThrowExceptions = true;
            }
        }
    }
}
