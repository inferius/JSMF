using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Interpreter;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Exceptions
{
    public static class ExceptionHelper
    {

        internal static void Throw(JSValue error)
        {
            //throw new JSException(error ?? JSValue.undefined, 0, 0);
        }

        internal static void ThrowTypeError(string message)
        {
            Throw(new JSException(message, new Position()));
        }

        internal static void Throw(Exception exception)
        {
            throw exception;
        }
    }
}
