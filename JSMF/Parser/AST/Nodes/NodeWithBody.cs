namespace JSMF.Parser.AST.Nodes
{
    public abstract class NodeWithBody : Node
    {
        protected INode _body;

        public INode Body
        {
            get => _body;
            set
            {
                _body = value;
                if (_body is NodeProgram program)
                {
                    program.Caller = this;
                }
            }
        }
    }
}