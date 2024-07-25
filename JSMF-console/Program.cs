using System.Text;
using JSMF.Parser;
using JSMF.Parser.Tokenizer;
using System.IO;
using JSMF.Interpreter;

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

            /*using (var scope = new Scope(null))
            {
                if (result is NodeProgram)
                {
                    var programResult = result.Evaluate(scope);
                }

            }*/
            
            Runner runner = new Runner();
            
            runner.Run(result);

            var tokens = TokenRegistredWords.ReadString(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));

        }

        static void registerGlobalData(Scope context)
        {
            //context.Define(new Variable { Name = "console", VarType = VarType.Let, Value });
        }
    }
}
