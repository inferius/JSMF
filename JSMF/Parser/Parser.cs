using System;
using System.Collections.Generic;
using System.Linq;
using JSMF.Exceptions;
using JSMF.Parser.AST;
using JSMF.Parser.AST.Nodes;
using JSMF.Parser.Tokenizer;

/*
TODO: Je potreba zkontrolovat a upravit parser, aby odpovidal specifikaci jazyka
TODO: [SyntaxError] Resit redeclaration of let/const
TODO: [SyntaxError] Unexpected strict mode reserved word (yield pouziti v normlani funkci)
 */

namespace JSMF.Parser
{
    public class Parser
    {
        private TokenStream stream;

        public Parser(TokenStream stream)
        {
            this.stream = stream;
        }

        public INode FullParse()
        {
            return ParseTopLevel();
        }

        public INode ParseAtom()
        {
            return MaybeCall(() =>
            {
                var _pPos = stream.CurrentPosition();
                SkipAllLineBreak();
                if (IsSeparator("("))
                {
                    stream.Next();
                    if (IsSeparator(")"))
                    {
                        SkipSeparator(")");
                        if (IsOperator("=>")) return ParseArrowFunction(null);
                        else Unexpected();
                    }

                    var r = ParseExpression();
                    if (IsSeparator(","))
                    {
                        var args = new List<INode>();
                        args.Add(r);
                        while (!stream.Eof() && !IsSeparator(")"))
                        {
                            SkipSeparator(",");
                            args.Add(ParseExpression());
                        }
                        SkipSeparator(")");
                        if (IsOperator("=>")) return ParseArrowFunction(args);
                        else Unexpected();
                    }
                    SkipSeparator(")");
                    if (IsOperator("=>")) return ParseArrowFunction(new List<INode>(new[] { r }));
                    return r;
                    //if (IsArrowFunction())
                    //{
                    //var args = Delimited("(", ")", ",", ParseArgument);
                    //    return ParseArrowFunction(args);
                    //}
                    //stream.Next();
                    //var r = ParseExpression();
                    //SkipSeparator(")");
                    //return r;
                }
                if (IsSeparator("{"))
                {
                    if (IsObject()) return ParseObject();
                    return ParseExpression();
                }
                if (IsSeparator("["))
                {
                    var array = Delimited("[", "]", ",", ParseExpression);
                    return new NodeArray() { Array = array, FileInfo = _pPos};
                }
                if (IsKeyword("if")) return ParseIf();
                if (IsKeyword("true") || IsKeyword("false")) return ParseBool();
                if (IsKeyword("null"))
                {
                    SkipKeyword("null");
                    return new NodeNull() {FileInfo = _pPos};
                }
                if (IsKeyword("function")) return ParseFunction();
                if (IsKeyword("while")) return ParseWhile();
                if (IsKeyword("do")) return ParseDoWhile();
                if (IsKeyword("for")) return ParseFor();
                if (IsKeyword("let") || IsKeyword("const") || IsKeyword("var"))
                {
                    return ParseVardef();
                }
                if (IsKeyword("return"))
                {
                    SkipKeyword("return");
                    var body = IsSeparator(";") ? null : ParseExpression();
                    if (IsSeparator(";")) SkipSeparator(";");
                    return new NodeReturn { Body = body, FileInfo = _pPos};
                }
                if (IsKeyword("break"))
                {
                    SkipKeyword();
                    if (IsSeparator(";")) SkipSeparator(";");
                    return new NodeNoValue(NodeType.Break) { FileInfo = _pPos};
                }
                if (IsKeyword("continue"))
                {
                    SkipKeyword();
                    if (IsSeparator(";")) SkipSeparator(";");
                    return new NodeNoValue(NodeType.Continue) {FileInfo = _pPos};
                }
                if (IsKeyword("async"))
                {
                    SkipKeyword("async");
                    if (IsKeyword("function")) return ParseFunction(true);
                    if (IsSeparator("("))
                    {
                        var args = Delimited("(", ")", ",", ParseArgument);
                        return ParseArrowFunction(args, true);
                    }
                }
                if (IsKeyword("await"))
                {
                    SkipKeyword("await");
                    return new NodeAwaitableCall { WaitFor = ParseExpression(), FileInfo = _pPos};
                }
                if (IsKeyword("new"))
                {
                    SkipKeyword("new");
                    return new NodeNewObject { Object = ParseExpression(), FileInfo = _pPos};
                }
                if (IsKeyword("class"))
                {
                    return ParseClass();
                }


                if (IsOperator("++") || IsOperator("--"))
                {
                    var op = stream.Peek().Value;
                    SkipOperator();
                    var nexTok = stream.Next();
                    if (nexTok.Type != TokenType.Identifier) Unexpected();
                    return new NodeIncDecOperator
                    {
                        AfterVar = false,
                        Identifier = new NodeIdentifier { Value = nexTok.Value, FileInfo = _pPos},
                        Operator = op,
                        FileInfo = _pPos
                    };
                }

                var before = stream.Peek();
                var tok = stream.Next();
                if (tok.Type == TokenType.Identifier)
                {
                    if (IsOperator("=>"))
                    {
                        return ParseArrowFunction(new List<INode> { new NodeArgument { Value = tok.Value, FileInfo = _pPos } });
                    }
                    // identifikator objektu nebo pole
                    if (IsSeparator(".") || IsSeparator("["))
                    {
                        return ParseObjectCall(new NodeIdentifier { Value = tok.Value, FileInfo = _pPos });
                    }

                    return new NodeIdentifier { Value = tok.Value, FileInfo = _pPos};
                }
                if (tok.Type == TokenType.String)
                {
                    return new NodeString { Value = tok.Value, FileInfo = _pPos };
                }
                if (tok.Type == TokenType.Numeric)
                {
                    return new NodeNumber(Number.Parse(tok.Value)) { FileInfo = _pPos };
                }

                if (IsSeparator(";") || IsSeparator("LB"))
                {
                    SkipSeparator();
                }

                Unexpected();
                return null;
            });
        }

        public INode ParseAtomClass()
        {
            SkipAllLineBreak();
            if (IsSeparator("{"))
            {
                SkipSeparator("{");
                var r = ParseAtomClass();
                SkipAllLineBreak();
                //SkipSeparator("}");
                return r;
            }

            if (IsKeyword("get") || IsKeyword("set"))
            {
                return ParseClassProperty();
            }

            if (IsKeyword("static") || IsKeyword("async") || IsKeyword("*") || stream.Peek().Type == TokenType.Identifier)
            {
                return ParseClassMethod();
            }

            Unexpected();
            return null;
        }

        public INode ParseClassMethod()
        {
            var _pPos = stream.CurrentPosition();
            var isSatic = IsKeyword("static");
            if (isSatic) SkipKeyword("static");
            var isAsync = IsKeyword("async");
            if (isAsync) SkipKeyword("async");

            if (IsKeyword("get") || IsKeyword("set")) return ParseClassProperty(isAsync);

            var isGenerator = IsOperator("*");
            if (isGenerator) SkipOperator("*");
            var name = stream.Next();
            if (name.Type != TokenType.Identifier) Unexpected();
            var args = Delimited("(", ")", ",", ParseExpression);
            SkipAllLineBreak();
            if (!IsSeparator("{")) Unexpected();
            var body = ParseProgram();

            if (isGenerator)
            {
                return new NodeClassMethodGenerator
                {
                    Arguments = args,
                    Body = body,
                    Function = new NodeIdentifier { Value = name.Value },
                    IsAnonymous = false,
                    IsStatic = isSatic,
                    IsAsync = isAsync,
                    FileInfo = _pPos
                };
            }

            return new NodeClassMethod
            {
                Arguments = args,
                Body = body,
                Function = new NodeIdentifier { Value = name.Value },
                IsAnonymous = false,
                IsStatic = isSatic,
                IsAsync = isAsync,
                FileInfo = _pPos
            };
        }

        public INode ParseClassProperty(bool isAsync = false)
        {
            var _pPos = stream.CurrentPosition();
            var isGetter = IsKeyword("get");
            SkipKeyword();
            var name = stream.Next();
            if (name.Type != TokenType.Identifier) Unexpected();
            var args = Delimited("(", ")", ",", ParseExpression);
            SkipAllLineBreak();
            if (!IsSeparator("{")) Unexpected();
            var body = ParseProgram();

            return new NodeProperty
            {
                Body = body,
                IsGetter = isGetter,
                IsAsync = isAsync,
                Arguments = args,
                Name = new NodeIdentifier { Value = name.Value },
                FileInfo = _pPos
            };
        }

        public INode ParseCall(INode funct)
        {
            var _pPos = stream.CurrentPosition();
            return new NodeCall()
            {
                Function = funct,
                Arguments = Delimited("(", ")", ",", ParseExpression),
                FileInfo = _pPos
            };
        }

        public INode ParseObjectCall(INode objName)
        {
            var _pPos = stream.CurrentPosition();
            INode child = null;

            //if (objName.Type != TokenType.Identifier) Unexpected();

            if (IsSeparator("."))
            {
                SkipSeparator(".");
                var ntoken = stream.Next();
                if (ntoken.Type != TokenType.Identifier) Unexpected();
                child = ParseObjectCall(new NodeIdentifier { Value = ntoken.Value, FileInfo = _pPos});
            }
            if (IsSeparator("["))
            {
                //SkipSeparator("[");
                var del = Delimited("[", "]", "NB", ParseExpression);
                if (!del.Any()) Unexpected();
                child = ParseObjectCall(del[0]);
            }
            if (IsSeparator("("))
            {
                var args = Delimited("(", ")", ",", ParseExpression);
                child = ParseObjectCall(new NodeCall { Arguments = args, Function = (NodeIdentifier)objName, FileInfo = _pPos});
            }

            if (child == null) return objName;

            return new NodeObjectCall
            {
                Name = objName,
                Child = child,
                FileInfo = _pPos
            };
        }

        public INode ParseExpression()
        {
            return MaybeCall(() => MaybeBinary(ParseAtom(), 0));
        }

        public INode ParseObject()
        {
            var _pPos = stream.CurrentPosition();

            var obj = new NodeJSObject() { FileInfo = _pPos};
            if (IsSeparator("{")) SkipSeparator("{");
            if (IsSeparator("}")) return obj;

            while (!stream.Eof())
            {
                SkipAllLineBreak();
                if (stream.Peek().Type == TokenType.String || stream.Peek().Type == TokenType.Identifier)
                {
                    var nameTok = stream.Next();
                    INode name = null;

                    if (nameTok.Type == TokenType.String) name = new NodeString { Value = nameTok.Value, FileInfo = stream.CurrentPosition() };
                    else name = new NodeIdentifier { Value = nameTok.Value, FileInfo = stream.CurrentPosition() };
                    SkipAllLineBreak();
                    if (IsSeparator(","))
                    {
                        SkipSeparator(",");
                        continue; // ES2015 podporuje objekty zadane {a, b, c}
                    }
                    if (!IsOperator(":")) Unexpected();
                    SkipOperator(":");
                    SkipAllLineBreak();
                    var val = ParseAtom();

                    obj.Values.Add(name, val);
                    SkipAllLineBreak();
                    if (IsSeparator(",")) SkipSeparator(",");
                    if (IsSeparator("}"))
                    {
                        SkipSeparator("}");
                        break;
                    }
                    SkipAllLineBreak();
                }
                else
                {
                    Unexpected();
                }
            }

            return obj;
        }

        public INode ParseClass()
        {
            var _pPos = stream.CurrentPosition();
            SkipKeyword("class");
            SkipAllLineBreak();
            INode extend = null;
            INode name = null;
            var isAnonymous = false;
            if (stream.Peek().Type == TokenType.Identifier)
            {
                name = new NodeIdentifier { Value = stream.Next().Value };
            }
            else
            {
                isAnonymous = true;
            }
            if (IsKeyword("extends"))
            {
                SkipKeyword("extends");
                if (stream.Peek().Type != TokenType.Identifier) Unexpected();
                extend = new NodeIdentifier { Value = stream.Next().Value };
            }
            SkipAllLineBreak();
            if (!IsSeparator("{")) Unexpected();
            INode body = ParseAtomClass();

            return new NodeClass
            {
                Name = name,
                Extends = extend,
                Body = body,
                IsAnonymous = isAnonymous,
                FileInfo = _pPos
            };
        }

        #region Helper parse keywords methods
        public INode ParseBool()
        {
            var _pPos = stream.CurrentPosition();
            var val = stream.Peek().Value;
            SkipKeyword(stream.Peek().Value);
            return new NodeBoolean { Value = val == "true", FileInfo = _pPos};
        }

        public INode ParseVardef()
        {
            var _pPos = stream.CurrentPosition();
            var name = ParseVarname();
            INode def = null;
            if (IsOperator("="))
            {
                stream.Next();
                def = ParseExpression();
            }
            if (IsSeparator(";")) SkipSeparator(";");
            return new NodeAssing { Operator = "=", Left = name, Right = def, FileInfo = _pPos};
        }

        public INode ParseProgram()
        {
            var _pPos = stream.CurrentPosition();
            var prog = new List<INode>();
            SkipAllLineBreak();
            SkipSeparator("{");
            SkipAllLineBreak();

            if (IsSeparator("}"))
            {
                SkipSeparator("}");
                return new NodeProgram
                {
                    Program = prog,
                    FileInfo = _pPos
                };
            }

            while (!stream.Eof())
            {
                SkipAllLineBreak();
                prog.Add(ParseExpression());
                if (!stream.Eof())
                {
                    if (IsSeparator(";")) SkipSeparator(";");
                    else if (IsSeparator(",")) SkipSeparator(",");

                    if (IsSeparator("LB")) SkipAllLineBreak();

                    if (IsSeparator("}"))
                    {
                        SkipSeparator("}");
                        break;
                    }
                }
            }

            return new NodeProgram
            {
                Program = prog
            };

        }

        public INode ParseIf()
        {
            var _pPos = stream.CurrentPosition();
            SkipKeyword("if");
            var cond = ParseExpression();
            SkipAllLineBreak();
            var then = IsSeparator("{") ? ParseProgram() : ParseExpression();
            if (IsSeparator(";")) SkipSeparator(";");
            var ret = new NodeIf
            {
                Condition = cond,
                Then = then,
                FileInfo = _pPos
            };

            if (IsKeyword("else"))
            {
                stream.Next();
                SkipAllLineBreak();
                ret.Else = IsSeparator("{") ? ParseProgram() : ParseExpression();
            }

            return (INode)ret;
        }

        public INode ParseWhile()
        {
            var _pPos = stream.CurrentPosition();
            SkipKeyword("while");

            var cond = ParseExpression();
            SkipAllLineBreak();
            var body = IsSeparator("{") ? ParseProgram() : ParseExpression();

            return new NodeWhile
            {
                Condition = cond,
                Body = body,
                FileInfo = _pPos
            };
        }

        public INode ParseDoWhile()
        {
            var _pPos = stream.CurrentPosition();
            SkipKeyword("do");
            var body = ParseProgram();
            if (IsSeparator(";")) SkipSeparator(";");
            SkipKeyword("while");
            var cond = Delimited("(", ")", ";", ParseExpression);
            if (!cond.Any()) Unexpected();
            if (IsSeparator(";")) SkipSeparator(";");

            return new NodeWhile
            {
                Condition = cond[0],
                Body = body,
                IsDoWhile = true,
                FileInfo = _pPos
            };
        }

        public INode ParseFor()
        {
            var _pPos = stream.CurrentPosition();
            SkipKeyword("for");
            if (!IsSeparator("(")) Unexpected();

            if (IsForOfOrForIn())
            {
                return ParseForOf();
            }

            var conds = Delimited("(", ")", ";", ParseForMultiParam);
            var body = IsSeparator("{") ? ParseProgram() : ParseExpression();
            if (IsSeparator(";")) SkipSeparator(";");

            return new NodeFor
            {
                Body = body,
                VarDefs = conds[0],
                Condition = conds[1],
                Iterate = conds[2],
                FileInfo = _pPos
            };

        }

        public INode ParseForMultiParam()
        {
            var a = new List<INode>();
            while (!stream.Eof())
            {
                a.Add(ParseExpression());
                if (!IsSeparator(",")) break;
                SkipSeparator(",");
            }

            return new NodeProgram
            {
                Program = a
            };
        }

        public INode ParseForOf()
        {
            var _pPos = stream.CurrentPosition();
            SkipSeparator("(");
            if (stream.Peek().Value != "var" && stream.Peek().Value != "let" && stream.Peek().Value != "const")
            {
                Unexpected();
            }
            var varDef = ParseVarname();
            if (stream.Peek().Value != "of" && !IsKeyword("in")) Unexpected();
            var isForOf = !IsKeyword("in");
            stream.Next();
            var array = ParseExpression();
            SkipSeparator(")");
            var body = IsSeparator("{") ? ParseProgram() : ParseExpression();

            return new NodeForOf
            {
                IsForOf = isForOf,
                VarDef = varDef,
                Enumerate = array,
                Body = body,
                FileInfo = _pPos
            };

        }

        public INode ParseArrowFunction(List<INode> args, bool isAsync = false)
        {
            var _pPos = stream.CurrentPosition();
            if (IsOperator("=>"))
            {
                SkipOperator("=>");
            }

            return new NodeFunction
            {
                IsAsync = isAsync,
                IsAnonymous = true,
                Arguments = args,
                Body = IsSeparator("{") ? ParseProgram() : new NodeReturn { Body = ParseExpression() },
                FileInfo = _pPos
            };
        }

        public INode ParseFunction(bool isAsync = false)
        {
            var _pPos = stream.CurrentPosition();
            SkipKeyword("function");
            var isGenerator = IsSeparator("*");
            if (IsSeparator("*")) SkipSeparator("*");
            var isAnonym = false;
            NodeIdentifier name = null;
            if (IsSeparator("(")) isAnonym = true;
            if (!isAnonym) name = new NodeIdentifier { Value = stream.Next().Value };
            if (!IsSeparator("(")) Unexpected();
            var args = Delimited("(", ")", ",", ParseArgument);
            var body = ParseProgram();

            if (isGenerator)
            {
                return new NodeGenerator()
                {
                    IsAsync = isAsync,
                    IsAnonymous = isAnonym,
                    Function = name,
                    Arguments = args,
                    Body = body,
                    FileInfo = _pPos
                };
            }
            return new NodeFunction()
            {
                IsAsync = isAsync,
                IsAnonymous = isAnonym,
                Function = name,
                Arguments = args,
                Body = body,
                FileInfo = _pPos
            };
        }

        public INode ParseVarname()
        {
            var _pPos = stream.CurrentPosition();
            string varType = null;
            if (IsKeyword("const") || IsKeyword("let") || IsKeyword("var"))
            {
                varType = stream.Peek().Value;
                SkipKeyword();
            }
            var name = stream.Next();
            if (name.Type != TokenType.Identifier) Unexpected();

            if (varType != null)
            {
                return new NodeVarDef
                {
                    Value = name.Value,
                    VarType = varType
                };
            }

            return new NodeIdentifier
            {
                Value = name.Value,
                FileInfo = _pPos
            };
        }

        public INode ParseArgument()
        {
            var name = stream.Next();
            if (name.Type != TokenType.Identifier) Unexpected();
            INode defaultValue = null;
            if (IsOperator("=")) defaultValue = ParseExpression();

            return new NodeArgument
            {
                Value = name.Value,
                DefaultValue = defaultValue
            };
        }
        #endregion

        public INode ParseTopLevel()
        {
            var _pPos = stream.CurrentPosition();
            var prog = new List<INode>();

            while (!stream.Eof())
            {
                SkipAllLineBreak();
                if (stream.Eof()) break;

                prog.Add(ParseExpression());
                if (!stream.Eof())
                {
                    if (IsSeparator(";")) SkipSeparator(";");
                    else if (IsSeparator("LB")) SkipAllLineBreak();
                    else if (IsSeparator(",")) SkipSeparator(",");
                    //else Unexpected();
                }
            }

            return new NodeProgram
            {
                Program = prog,
                FileInfo = _pPos
            };
        }

        public INode ParseIncDecOperator(INode identifier, bool after = true)
        {
            var _pPos = stream.CurrentPosition();
            var decinc = new NodeIncDecOperator() { FileInfo = _pPos};
            decinc.Identifier = identifier as NodeIdentifier;
            decinc.Operator = stream.Next().Value;
            decinc.AfterVar = after;

            return decinc;
        }

        public INode MaybeCall(Func<INode> action)
        {
            var actResult = action();
            return IsSeparator("(") ? ParseCall(actResult) : actResult;
            //if (IsSeparator("(")) return ParseCall(actResult);
            //else if (IsOperator("=>")) return ParseArrowFunction(null);
            //else return actResult;
        }

        public INode MaybeBinary(INode left, int priority)
        {
            if (IsOperator())
            {
                if (IsOperator("++") || IsOperator("--"))
                {
                    return ParseIncDecOperator(left);
                }
                var hisPriority = AstTreeMethods.OperatorsPriority[stream.Peek().Value];
                if (hisPriority > priority)
                {
                    var tok = stream.Next();
                    var right = MaybeBinary(ParseAtom(), hisPriority);
                    INode binary = null;
                    if (tok.Value == "=")
                    {
                        binary = new NodeAssing()
                        {
                            Operator = tok.Value,
                            Left = left,
                            Right = right
                        };
                    }
                    else
                    {
                        binary = new NodeBinary()
                        {
                            Operator = tok.Value,
                            Left = left,
                            Right = right
                        };
                    }

                    return MaybeBinary(binary, priority);
                }
            }

            return left;
        }

        public List<INode> Delimited(string start, string stop, string separator, Func<INode> parser)
        {
            var a = new List<INode>();
            var first = true;
            SkipSeparator(start);
            while (!stream.Eof())
            {
                if (IsSeparator(stop)) break;
                if (first) first = false;
                else SkipSeparator(separator);
                if (IsSeparator(stop)) break;
                a.Add(parser());
            }
            SkipSeparator(stop);

            return a;
        }

        public bool IsObject()
        {
            stream.ReadNextWithHistory();
            SkipAllLineBreak(true);
            var tok = stream.ReadNextWithHistory();
            SkipAllLineBreak(true);
            if (IsOperator(":"))
            {
                stream.ReverseToHistoryStart();
                return true;
            }
            stream.ReverseToHistoryStart();

            return false;
        }

        public bool IsForOfOrForIn()
        {
            stream.ReadNextWithHistory();
            stream.ReadNextWithHistory();
            stream.ReadNextWithHistory();
            if (stream.Peek().Value == "of" || IsKeyword("in"))
            {
                stream.ReverseToHistoryStart();
                return true;
            }

            stream.ReverseToHistoryStart();
            return false;
        }

        public bool IsArrowFunction()
        {
            var bracket = 0;
            if (IsSeparator("("))
            {
                while (!stream.Eof())
                {
                    if (IsSeparator("(")) bracket++;
                    else if (IsSeparator(")"))
                    {
                        if (--bracket == 0)
                        {
                            stream.ReadNextWithHistory();
                            if (IsOperator("=>"))
                            {
                                stream.ReverseToHistoryStart();
                                return true;
                            }
                            stream.ReverseToHistoryStart();
                            return false;
                        }
                    }
                    stream.ReadNextWithHistory();
                }
            }
            else if (stream.Peek().Type == TokenType.Identifier)
            {
                stream.ReadNextWithHistory();
                if (IsOperator("=>"))
                {
                    stream.ReverseToHistoryStart();
                    return true;
                }
            }

            return false;
        }

        public bool IsSeparator(string sep = null)
        {
            return stream.Peek()?.Type == TokenType.Separator && (sep == null || stream.Peek()?.Value == sep);
        }

        public bool IsKeyword(string keyword = null)
        {
            return stream.Peek()?.Type == TokenType.Keyword && (keyword == null || stream.Peek()?.Value == keyword);
        }

        public bool IsOperator(string op = null)
        {
            return stream.Peek()?.Type == TokenType.Operator && (op == null || stream.Peek()?.Value == op);
        }

        public void SkipSeparator(string sep = null)
        {
            if (IsSeparator(sep)) stream.Next();
            else Unexpected();
        }

        public void SkipKeyword(string keyword = null)
        {
            if (IsKeyword(keyword)) stream.Next();
            else Unexpected();
        }

        public void SkipOperator(string op = null)
        {
            if (IsOperator(op)) stream.Next();
            else Unexpected();
        }

        public void SkipAllLineBreak(bool withHistory = false)
        {
            while (!stream.Eof() && IsSeparator("LB"))
            {
                if (withHistory) stream.ReadNextWithHistory();
                else stream.Next();
            }
        }

        public void Unexpected()
        {
            throw new ParserException($"Unexpected token '{stream.Peek().Value}'", stream.CurrentPosition());
        }
    }
}
