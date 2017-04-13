using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Interpreter
{
    public class Variable
    {
        public VarType VarType { get; set; }
        public string Name { get; set; }
        public JSValue Value { get; set; }
    }

    public enum VarType
    {
        Const,
        Var,
        Let
    }
}
