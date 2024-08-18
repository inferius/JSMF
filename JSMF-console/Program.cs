using System;
using System.Text;
using JSMF.Parser;
using JSMF.Parser.Tokenizer;
using System.IO;
using JSMF.Core;
using JSMF.EventArgs;
using JSMF.Interpreter;
using JSMF.Interpreter.BaseLibrary;

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

            var runner = Initialize();
            
            
            runner.Run(result);

            var tokens = TokenRegistredWords.ReadString(new InputStream("`${aa}ahoj ${ah} \\` ˛ \\` $fsa \\${pp}${ui}`"));

        }

        static Runner Initialize()
        {
            GlobalScopeCreator globalScopeCreator = new GlobalScopeCreator();
            Runner runner = new Runner(globalScopeCreator.GlobalScope);
            //context.Define(new Variable { Name = "console", VarType = VarType.Let, Value });
            Runner.ConsoleEvents += new EventHandler<ConsoleEventArgs>((sender, e) =>
            {
                if (e.Type == ConsoleEventArgsType.ConsoleLog)
                {
                    foreach (var arg in e.Arguments)
                    {
                        Console.WriteLine(Tools.Printer(arg, e.CurrentScope, e.GlobalScope));
                    }
                }
            });
                
            return runner;
        }
    }
}
