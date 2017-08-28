using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public abstract class Node : INode
    {
        public NodeType Type { get; protected set; }
        public Position FileInfo { get; set; }

        public abstract JSValue Evaluate(Scope context);

        //public virtual bool Build(ref Node _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, CompilerMessageCallback message, FunctionInfo stats, Options opts)
        //{
        //    return false;
        //}

        //public virtual void Optimize(ref Node _this, FunctionDefinition owner, CompilerMessageCallback message, Options opts, FunctionInfo stats)
        //{

        //}
    }

    public sealed class FunctionInfo
    {
        public bool UseGetMember;
        public bool UseCall;
        public bool WithLexicalEnvironment;
        public bool ContainsArguments;
        public bool ContainsRestParameters;
        public bool ContainsEval;
        public bool ContainsWith;
        public bool ContainsYield;
        public bool ContainsAwait;
        public bool ContainsInnerEntities;
        public bool ContainsThis;
        public bool ContainsDebugger;
        public bool ContainsTry;
        public readonly List<Node> Returns = new List<Node>();
    }
}
