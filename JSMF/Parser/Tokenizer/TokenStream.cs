using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.Tokenizer
{
    public class TokenStream
    {
        private InputStream stream;
        private Queue<Token> queue = new Queue<Token>();
        private Token current = null;
        private Queue<Token> history = new Queue<Token>();
        private bool withoutHistoryRead = false;

        private int position = 0;

        public TokenStream(InputStream stream)
        {
            this.stream = stream;
        }

        public Token Peek()
        {
            if (current == null) current = ReadNext();
            return current;
        }

        public Token Next()
        {
            var tok = current;
            current = null;
            if (tok != null) return tok;
            return ReadNext();
        }

        public bool Eof()
        {
            return Peek() == null;
        }

        public Token ReadNextWithHistory()
        {
            if (!history.Any() && current != null) history.Enqueue(current);
            withoutHistoryRead = true;
            current = ReadNext();
            withoutHistoryRead = false;
            if (current == null) throw new NullReferenceException();
            history.Enqueue(current);
            position++;
            return current;
        }

        public void ReverseToHistoryStart()
        {
            current = null;
            position = 0;
        }

        public Token ReadNext()
        {
            if (history.Any() && !withoutHistoryRead) return history.Dequeue();
            if (queue.Any()) return queue.Dequeue();

            ReadWhile(cha => TokenRegistredWords.WhiteSpaceChars.IndexOf((char)cha) != -1);
            if (stream.Eof()) return null;


            // cteni komentare
            var ch = stream.Next();
            if (ch == '/')
            {
                if (stream.Peek() == '*')
                {
                    TokenRegistredWords.SkipComment(stream, true);
                    return ReadNext();
                }
                else if (stream.Peek() == '/')
                {
                    TokenRegistredWords.SkipComment(stream, false);
                    return ReadNext();
                }
            }
            if (TokenRegistredWords.IsStringStart((char)ch))
            {
                var strTokens = TokenRegistredWords.ReadString(stream, (char)ch);
                foreach (var strToken in strTokens)
                {
                    queue.Enqueue(strToken);
                }
                return ReadNext();
            }

            if (char.IsDigit((char)ch) || (ch == '.' && char.IsDigit((char)stream.Peek())))
            {
                return TokenRegistredWords.ReadNumber(stream, ch);
            }
            if (TokenRegistredWords.IsValidIdentifierStart(ch))
            {
                return TokenRegistredWords.ReadIndetifier(stream, ch);
            }
            if (TokenRegistredWords.IsSeparatorChar(ch))
            {
                return new Token(TokenType.Separator, (char)ch, stream.Line, stream.Column);
            }
            if (TokenRegistredWords.IsLineBreak((char)ch))
            {
                if (ch == '\n' && stream.Peek() == '\r' || ch == '\r' && stream.Peek() == '\n')
                {
                    //return new Token(TokenType.Separator, $"{(char)ch}{(char)stream.Next()}", stream.Line, stream.Column);
                    return new Token(TokenType.Separator, "LB", stream.Line, stream.Column);
                }
                //return new Token(TokenType.Separator, (char)ch, stream.Line, stream.Column);
                return new Token(TokenType.Separator, "LB", stream.Line, stream.Column);
            }
            if (TokenRegistredWords.IsOperatorChar(ch))
            {
                return TokenRegistredWords.ReadOperator(stream, ch);
            }

            stream.Error($"Can't handle character '{(char)ch}'");
            return null;
        }

        private string ReadWhile(Func<int, bool> predicate)
        {
            var str = new StringBuilder();
            while (!stream.Eof() && predicate(stream.Peek()))
                str.Append(stream.Next());
            return str.ToString();
        }
    }
}
