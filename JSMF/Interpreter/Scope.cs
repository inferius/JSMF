using System;
using System.Collections.Generic;
using JSMF.Exceptions;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Interpreter
{
    public class Scope : IDisposable
    {
        internal Scope Parent { get; set; }
        internal IDictionary<string, Variable> Variables = new Dictionary<string, Variable>();
        internal IDictionary<string, NodeFunction> Functions = new Dictionary<string, NodeFunction>();

        public Scope RootContext
        {
            get
            {
                var res = this;

                while (res?.Parent != null)
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

        /// <summary>
        /// Vyhledává proměnou v daném kontextu, podle názvu, pokud ji nenajde vrátí null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Scope Lookup(string name)
        {
            var scope = this;
            while (scope != null)
            {
                if (scope.Variables.ContainsKey(name)) return scope;
                scope = scope.Parent;
            }

            return null;
        }
        
        /// <summary>
        /// Vyhledává funkci v daném kontextu, podle názvu, pokud ji nenajde vrátí null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Scope LookupFunction(string name)
        {
            var scope = this;
            while (scope != null)
            {
                if (scope.Functions.ContainsKey(name)) return scope;
                scope = scope.Parent;
            }

            return null;
        }

        /// <summary>
        /// Vytvoří nebo aktualizuje hodnotu stávající proměnné.
        /// </summary>
        /// <param name="var"></param>
        /// <param name="val"></param>
        public void SetOrUpdate(Variable var, JSValue val)
        {
            var scope = Lookup(var.Name);

            if (scope == null)
            {
                Define(var);
                scope = this;
            }
            scope.Variables[var.Name].Value = val;
        }

        /// <summary>
        /// Nastavuje hodotu proměnné.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <exception cref="JSException">Pokud proměnná neexistuje</exception>
        public void Set(string name, JSValue val)
        {
            var scope = Lookup(name);

            if (scope == null) throw new JSException($"Undefined variable {name}", val._position);
            scope.Variables[name].Value = val;
        }

        public Variable Get(string name, Position? callerPosition)
        {
            var scope = Lookup(name);

            if (scope == null) throw new JSException($"Undefined variable {name}", callerPosition ?? new Position()); // TODO: Doplnit aktualni pozici, odkud se cetlo, informace v Node je, globalne predavat
            return scope.Variables[name];
        }
        
        public void CreateFunction(string name, NodeFunction function)
        {
            Functions[name] = function;
        }
        
        public NodeFunction GetFunction(string name, Position? callerPosition)
        {
            var scope = LookupFunction(name);

            if (scope == null) throw new JSException($"Uncaught ReferenceError: {name} is not defined", callerPosition ?? new Position()); 
            return scope.Functions[name];
        }

        /// <summary>
        /// Vytvoří prázdnou proměnnou
        /// </summary>
        /// <param name="var"></param>
        public void Define(Variable var)
        {
            Variables[var.Name] = var;
        }

        public void Dispose()
        {
            Variables.Clear();
            Variables = null;
        }
    }
}
