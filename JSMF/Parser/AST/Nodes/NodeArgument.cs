namespace JSMF.Parser.AST.Nodes
{
    public class NodeArgument : NodeIdentifier
    {
        public INode DefaultValue { get; set; } = null;

        public override string ToString()
        {
            return $"[{Type}]{Value} = {DefaultValue}";
        }
    }
}
