using JSMF.Interpreter;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Parser.AST.Helper
{
    internal class NodeHelpers
    {
        public static NodeSymbol GetSymbolIterator(NodeJSObject node, Scope globalScope)
        {
            var ns = new NodeSymbol();

            var scope = new Scope(globalScope);
            
            ns.SymbolType = SymbolTypes.Iterator;
            
            

            return ns;
        }
    }
}