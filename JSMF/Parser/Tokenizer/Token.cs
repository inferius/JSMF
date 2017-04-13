namespace JSMF.Parser.Tokenizer
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Line { get; set; } = 0;
        public int Column { get; set; } = 0;

        #region Constructors
        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
        }

        public Token(TokenType type, char value, int line, int column) : this(type, value.ToString(), line, column)
        {
        }
        #endregion

        public override string ToString()
        {
            return $"{Type} => {Value}";
        }

        public static Token EndToken()
        {
            return new Token(TokenType.EndToken, null, 0, 0);
        }
    }
}
