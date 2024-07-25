using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSMF.Parser;
using JSMF.Parser.Tokenizer;
using System.IO;
using JSMF.Parser.AST.Nodes;
using JSMF.Interpreter;
using System.Linq.Expressions;

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

            using (var scope = new Scope(null))
            {
                if (result is NodeProgram)
                {
                    var programResult = result.Evaluate(scope);
                }

            }

            var tokens = TokenRegistredWords.ReadString(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));

        }

        static void registerGlobalData(Scope context)
        {
            //context.Define(new Variable { Name = "console", VarType = VarType.Let, Value });
        }
    }
}
