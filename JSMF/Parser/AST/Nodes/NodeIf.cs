using System;
using JSMF.Exceptions;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeIf : Node
    {
        public INode Condition { get; set; }
        public INode Then { get; set; }
        public INode Else { get; set; }

        public NodeIf()
        {
            Type=NodeType.Condition;
        }

        public override JSValue Evaluate(Scope context)
        {
            NodeFor.SetProgramData(Condition);
            NodeFor.SetProgramData(Then);
            NodeFor.SetProgramData(Else);
            
            var aScope = context.Extend();

            var result = Condition.Evaluate(context);
            if (result.IsTrue())
            {
                Then.Evaluate(aScope);
            }
            else
            {
                Else?.Evaluate(aScope);
            }
        
            return JSValue.undefined;
        }
    }
}
