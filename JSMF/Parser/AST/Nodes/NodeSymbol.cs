using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeSymbol : NodeWithBody
    {
        public SymbolTypes SymbolType { get; set; }
        
        public NodeSymbol()
        {
            Type = NodeType.Symbol;
        }
        
        public NodeSymbol(SymbolTypes symbolType, INode body = null): this()
        {
            SymbolType = symbolType;
            Body = body;
        }
        public override JSValue Evaluate(Scope context)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            var s = SymbolType.ToString();
            return $"[Symbol.{char.ToLower(s[0]) + s.Substring(1)}]";
        }
    }
}