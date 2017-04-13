using System.IO;
using JSMF.Parser.AST.Nodes;

namespace JSMF.Parser
{
    public class InputStream
    {
        private StreamReader stream;
        private Position _filePosition;
        public Position FilePosition => _filePosition;
        public int Position { get; private set; } = 0;

        public InputStream(string data)
        {
            _filePosition = new Position(null, data);
            stream = new StreamReader(StreamFromString(data));
        }

        public InputStream(StreamReader fileReader, string filePath)
        {
            _filePosition = new Position(filePath);
            stream = fileReader;
        }

        public int Next()
        {
            var ch = (char)stream.Read();

            if (ch == -1) return -1;
            Position++;

            if (Tokenizer.TokenRegistredWords.IsLineBreak(ch))
            {
                if (!(ch == '\n' && stream.Peek() == '\r' || ch == '\r' && stream.Peek() == '\n')) _filePosition.Line++;
                _filePosition.Column = 0;
            }
            else _filePosition.Column++;

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
            throw new Exceptions.ParserException(msg, FilePosition);
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
