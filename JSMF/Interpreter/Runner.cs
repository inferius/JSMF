using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Exceptions;
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
            node.Evaluate(GlobalScope);
        }
    }
}
