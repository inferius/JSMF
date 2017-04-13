using System.IO;

namespace JSMF.Parser
{
    public class InputStream
    {
        private StreamReader stream;

        public int Line { get; private set; } = 0;
        public int Column { get; private set; } = 0;
        public int Position { get; private set; } = 0;

        public InputStream(string data)
        {
            stream = new StreamReader(StreamFromString(data));
        }

        public InputStream(StreamReader fileReader)
        {
            stream = fileReader;
        }

        public int Next()
        {
            var ch = (char)stream.Read();

            if (ch == -1) return -1;
            Position++;

            if (Tokenizer.TokenRegistredWords.IsLineBreak(ch))
            {
                if (!(ch == '\n' && stream.Peek() == '\r' || ch == '\r' && stream.Peek() == '\n')) Line++;
                Column = 0;
            }
            else Column++;

            return ch;
        }

        public int Peek()
        {
            return stream.Peek();
        }

        public bool Eof()
        {
            return stream.EndOfStream;
        }

        public void Error(string msg)
        {
            throw new Exceptions.ParserException(msg, Line, Column);
        }

        internal static Stream StreamFromString(string data)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
