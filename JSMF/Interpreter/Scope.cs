using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSMF.Exceptions;

namespace JSMF.Interpreter
{
    public class Scope
    {
        internal Scope Parent { get; set; }
        internal IDictionary<string, Variable> Variables = new Dictionary<string, Variable>();

        public Scope RootContext
        {
            get
            {
                var res = this;

                while (res.Parent?.Parent != null)
                    res = res.Parent;

                return res;
            }
        }

        public Scope(Scope parent)
        {
            Parent = parent;
        }

        public Scope Extend()
        {
            return new Scope(this);
        }

        public Scope Lookup(string name)
        {
            var scope = this;
            while (scope != null)
            {
                if (Variables.ContainsKey(name)) return scope;
                scope = scope.Parent;
            }

            return null;
        }

        public void Set(string name, JSValue val)
        {
            var scope = Lookup(name);

            if (scope == null) throw new JSException($"Undefined variable {name}",0,0);
            scope.Variables[name].Value = val;
        }

        public Variable Get(string name)
        {
            var scope = Lookup(name);

            if (scope == null) throw new JSException($"Undefined variable {name}", 0, 0);
            return scope.Variables[name];
        }

        public void Define(Variable var)
        {
            Variables[var.Name] = var;
        }
    }
}
