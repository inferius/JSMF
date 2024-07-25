using JSMF.Interpreter;

namespace JSMF.Parser.AST.Nodes
{
    public interface INode
    {
        NodeType Type { get; }
        Position FileInfo { get; }
        JSValue Evaluate(Scope context);
    }
}
