using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Parser;
using JSMF.Parser.Tokenizer;
using System.IO;

namespace JSMF_console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var tokenStream = new TokenStream(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));
            var tokstr = new TokenStream(new InputStream(new StreamReader("../../Tests/Test1.js", Encoding.UTF8)));
            File.Delete(@"C:\Temp\tokens.txt");
            while (!tokstr.Eof())
            {
                File.AppendAllText(@"C:\Temp\tokens.txt", $"{tokstr.Next()}\n");
            }

            var tokenStream = new Parser(new TokenStream(new InputStream(new StreamReader("../../Tests/Test1.js", Encoding.UTF8))));
            var result = tokenStream.FullParse();

            var tokens = TokenRegistredWords.ReadString(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));

        }
    }
}
