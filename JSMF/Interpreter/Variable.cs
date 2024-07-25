using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Exceptions;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Interpreter
{
    public class Variable
    {
        private JSValue _value;
        public VarType VarType { get; set; } = VarType.Var;
        public string Name { get; set; }
        private bool _initialized = false;

        public JSValue Value
        {
            get => _value;
            set
            {
                if (VarType == VarType.Const && _initialized)
                {
                    throw new ArgumentException("Assignment to constant variable.");
                }
                            
                _initialized = true;
                _value = value;
            }
        }

        public override string ToString()
        {
            return $"{VarType} {Name} = {Value}";
        }
    }

    public enum VarType
    {
        Const,
        Var,
        Let
    }
}
