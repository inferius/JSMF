using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSMF.Parser.Tokenizer
{
    public static class TokenRegistredWords
    {
        public static HashSet<char> LineBreak = new HashSet<char>(new[] { '\n', '\r', '\x2028', '\x2029' });
        public static HashSet<char> WhiteSpace = new HashSet<char>(new[] { '\t', '\v', '\f', '\x0020', '\x00A0' });
        public static HashSet<string> KeyWords = new HashSet<string>(new[] {
            "break",
            "case",
            "catch",
            "get",
            "set",
            "class",
            "const",
            "continue",
            "debugger",
            "default",
            "delete",
            "do",
            "else",
            "export",
            "extends",
            "finally",
            "for",
            "function",
            "if",
            "import",
            "in",
            "instanceof",
            "new",
            "return",
            "super",
            "switch",
            "this",
            "throw",
            "try",
            "typeof",
            "var",
            "void",
            "while",
            "with",
            "yield",
            "implements",
            "interface",
            "let",
            "package",
            "private",
            "protected",
            "public",
            "static",
            "await",
            "async",
            "true",
            "false",
            "null"}
        );

        public static HashSet<char> IdentifierStart = new HashSet<char>(new[] { '$', '_' });
        public static HashSet<string> Operators = new HashSet<string>(new[] { "=>", "<", ">", "<=", ">=", "=", "==", "===", "!", "!=", "!==", "?", ":", "^", "~", "+", "-", "++", "--", "**", "+=", "-=", "*=", "/=", "|=", "&=", "&&", "||", "&", "|", ">>", "<<", ">>>", "%", "%=", "*", "/" });
        public static string SeparatorChars = "()[]{};,.";
        public static string OperatorChars = "=+-*/&|~^?:<>!%";
        public static string WhiteSpaceChars = "\t\v\f\x0020\x00A0 ";

        public static bool IsLineBreak(char ch)
        {
            return LineBreak.Contains(ch);
        }

        public static bool IsStringStart(char ch)
        {
            if (ch == '\'' || ch == '"' || ch == '`') return true;
            return false;
        }

        #region Read string methods
        public static IEnumerable<Token> ReadString(InputStream stream, char startChar = '\0')
        {
            var str = new StringBuilder();
            var startCh = startChar == '\0' ? stream.Next() : startChar;

            var isTemplateString = startCh == '`';
            var isEscaped = false;

            if (isTemplateString) return ReadTemplateString(stream);
            while (!stream.Eof() && !(stream.Peek() == startCh && !isEscaped))
            {
                var ch = stream.Next();

                if (ch == '\\')
                {
                    isEscaped = true;
                    continue;
                }
                if (!IsValidStringChar(ch)) stream.Error($"Invalid string char '{ch}'");

                str.Append((char)ch);
            }
            stream.Next();
            return new[] { new Token(TokenType.String, str.ToString(), stream.FilePosition) };
        }

        private static IEnumerable<Token> ReadTemplateString(InputStream stream)
        {
            var tokens = new List<Token>();
            var str = new StringBuilder();
            var isEscaped = false;
            var isTemplateStart = false;

            while (!stream.Eof() && !(stream.Peek() == '`' && !isEscaped))
            {
                var ch = stream.Next();
                if (!IsValidStringChar(ch)) stream.Error($"Invalid string char '{(char)ch}'");

                if (ch == '\\')
                {
                    isEscaped = true;
                    continue;
                }
                else if (ch == '$')
                {
                    if (!isEscaped)
                    {
                        isTemplateStart = true;
                        continue;
                    }
                }
                else if (ch == '{')
                {
                    if (isTemplateStart)
                    {
                        if (str.Length > 0) tokens.Add(new Token(TokenType.String, str.ToString(), stream.FilePosition));
                        str.Clear();

                        if (tokens.Any()) tokens.Add(new Token(TokenType.Operator, "+", stream.FilePosition));

                        tokens.Add(ReadLiteralOrVar(stream));
                        if (stream.Peek() != '}') stream.Error($"Unexpected token '{(char)stream.Peek()}'");
                        stream.Next();

                        if (!stream.Eof()) tokens.Add(new Token(TokenType.Operator, "+", stream.FilePosition));
                        isTemplateStart = false;
                        continue;
                    }
                }
                else
                {
                    if (isTemplateStart) str.Append('$');
                    isEscaped = false;
                    isTemplateStart = false;
                }

                str.Append((char)ch);
            }
            stream.Next();
            if (str.Length > 0) tokens.Add(new Token(TokenType.String, str.ToString(), stream.FilePosition));
            
            // Pri vkladani doslo, ze se po promenne vlozil operator +, aniz by musel rozbijelo to potom syntaktickou kontrolu
            if (tokens.Last().Type == TokenType.Operator && tokens.Last().Value == "+") tokens.Remove(tokens.Last());

            return tokens;
        }

        /// <summary>
        /// Precte literal z template stringu
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static Token ReadLiteralOrVar(InputStream stream)
        {
            if (!IsValidIdentifierStart(stream.Peek())) stream.Error($"Unexpected token '{stream.Peek()}'");
            var str = new StringBuilder();
            while (!stream.Eof() && IsValidIdentifierChar(stream.Peek()))
            {
                str.Append((char)stream.Next());
            }

            if (!IsStringLiteralOrVariableValid(str.ToString())) stream.Error($"Unexpected token '{str}'");

            return new Token(KeyWords.Contains(str.ToString()) ? TokenType.Keyword : TokenType.Identifier, str.ToString(), stream.FilePosition);
        }

        /// <summary>
        /// Kontroluje literal z template stringu
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static bool IsStringLiteralOrVariableValid(string varName)
        {
            if (KeyWords.Contains(varName)) return false;
            // TODO: Doladit veskere unicode znaky
            return true;
        }
        #endregion

        #region Comments skip methods

        public static void SkipComment(InputStream stream, bool multiline = false)
        {
            while (!stream.Eof())
            {
                var ch = stream.Next();

                if (multiline)
                {
                    if (ch == '*' && stream.Peek() == '/')
                    {
                        stream.Next();
                        return;
                    }
                }
                else
                {
                    if (LineBreak.Contains((char)ch)) return;
                }
            }
        }

        #endregion

        public static bool IsOperatorChar(int ch)
        {
            return OperatorChars.IndexOf((char)ch) != -1;
        }

        public static Token ReadOperator(InputStream stream, int firstChar = 0)
        {
            var str = new StringBuilder();
            if (firstChar != 0) str.Append((char)firstChar);
            while (!stream.Eof() && IsOperatorChar(stream.Peek()))
            {
                str.Append((char)stream.Next());
            }
            var fstr = str.ToString();

            return new Token(TokenType.Operator, fstr, stream.FilePosition);
        }

        public static Token ReadNumber(InputStream stream, int firstChar = 0)
        {
            var str = new StringBuilder();
            if (firstChar != 0) str.Append((char)firstChar);
            while (!stream.Eof() && (char.IsDigit((char)stream.Peek()) || stream.Peek() == '.'))
            {
                str.Append((char)stream.Next());
            }

            return new Token(TokenType.Numeric, str.ToString(), stream.FilePosition);
        }

        public static bool IsSeparatorChar(int ch)
        {
            return SeparatorChars.IndexOf((char)ch) != -1;
        }

        public static bool IsValidIdentifierStart(int ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch == '$' || ch == '_' || char.IsLetter((char)ch)) return true;
            return false;
        }

        public static bool IsValidIdentifierChar(int ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9' || ch == '_' || char.IsLetterOrDigit((char)ch)) return true;
            return false;
        }

        public static Token ReadIndetifier(InputStream stream, int firstChar = 0)
        {
            var str = new StringBuilder();
            if (firstChar != 0) str.Append((char)firstChar);
            else
            {
                firstChar = stream.Next();
                if (!IsValidIdentifierStart(firstChar)) stream.Error($"Unexpected token '{(char)firstChar}'");
                str.Append((char)firstChar);
            }

            while (!stream.Eof() && IsValidIdentifierChar(stream.Peek()))
            {
                str.Append((char)stream.Next());
            }
            var finalString = str.ToString();
            if (KeyWords.Contains(finalString)) return new Token(TokenType.Keyword, finalString, stream.FilePosition);

            return new Token(TokenType.Identifier, finalString, stream.FilePosition);
        }

        public static bool IsValidStringChar(int ch)
        {
            return true;
        }

        public static bool IsVariableValid(string varName)
        {
            if (KeyWords.Contains(varName)) return false;
            // TODO: Doladit veskere unicode znaky
            return true;
        }
    }
}
