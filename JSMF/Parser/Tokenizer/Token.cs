using JSMF.Parser.AST.Nodes;

namespace JSMF.Parser.Tokenizer
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public Position Position { get; }

        #region Constructors
        public Token(TokenType type, string value, Position position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public Token(TokenType type, char value, Position position) : this(type, value.ToString(), position)
        {
        }
        #endregion

        public override string ToString()
        {
            return $"{Type} => {Value}";
        }

        public string ToStringWithFile()
        {
            return ToString() + $" {Position}";
        }
    }
}
