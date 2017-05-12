using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Parser;
using JSMF.Parser.Tokenizer;
using System.IO;
using JSMF.Parser.AST.Nodes;

namespace JSMF_console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var tokenStream = new TokenStream(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));
            var filePathRelative = "../../Tests/Test1.js";
            //var tokstr = new TokenStream(new InputStream(new StreamReader(filePathRelative, Encoding.UTF8), filePathRelative));
            //File.Delete(@"C:\Temp\tokens.txt");
            //while (!tokstr.Eof())
            //{
            //    File.AppendAllText(@"C:\Temp\tokens.txt", $"{tokstr.Next().ToStringWithFile()}\n");
            //}

            var tokenStream = new Parser(new TokenStream(new InputStream(new StreamReader(filePathRelative, Encoding.UTF8), filePathRelative)));
            var result = tokenStream.FullParse();


            var tokens = TokenRegistredWords.ReadString(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));

        }
    }
}
