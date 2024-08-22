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
        private readonly TokenStream _stream;

        public Parser(TokenStream stream)
        {
            _stream = stream;
        }

        public INode FullParse()
        {
            return ParseTopLevel();
        }

        private INode ParseAtom()
        {
            return MaybeCall(() =>
            {
                var _pPos = _stream.CurrentPosition();
                SkipAllLineBreak();
                if (IsSeparator("("))
                {
                    _stream.Next();
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
                        while (!_stream.Eof() && !IsSeparator(")"))
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
                    var op = _stream.Peek().Value;
                    SkipOperator();
                    var nexTok = _stream.Next();
                    if (nexTok.Type != TokenType.Identifier) Unexpected();
                    return new NodeIncDecOperator
                    {
                        AfterVar = false,
                        Identifier = new NodeIdentifier { Value = nexTok.Value, FileInfo = _pPos},
                        Operator = op,
                        FileInfo = _pPos
                    };
                }

                var before = _stream.Peek();
                var tok = _stream.Next();
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

                    if (IsOperator("=") && useLastVarType)
                    {
                        var d= ParseVardef(before);
                        useLastVarType = false;
                        return d;
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

        private INode ParseAtomClass()
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

            if (IsKeyword("static") || IsKeyword("async") || IsKeyword("*") || _stream.Peek().Type == TokenType.Identifier)
            {
                return ParseClassMethod();
            }

            Unexpected();
            return null;
        }

        private INode ParseClassMethod()
        {
            var _pPos = _stream.CurrentPosition();
            var isSatic = IsKeyword("static");
            if (isSatic) SkipKeyword("static");
            var isAsync = IsKeyword("async");
            if (isAsync) SkipKeyword("async");

            if (IsKeyword("get") || IsKeyword("set")) return ParseClassProperty(isAsync);

            var isGenerator = IsOperator("*");
            if (isGenerator) SkipOperator("*");
            var name = _stream.Next();
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

        private INode ParseClassProperty(bool isAsync = false)
        {
            var _pPos = _stream.CurrentPosition();
            var isGetter = IsKeyword("get");
            SkipKeyword();
            var name = _stream.Next();
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

        private INode ParseCall(INode funct)
        {
            var _pPos = _stream.CurrentPosition();
            return new NodeCall()
            {
                Function = funct,
                Arguments = Delimited("(", ")", ",", ParseExpression),
                FileInfo = _pPos
            };
        }

        private INode ParseObjectCall(INode objName)
        {
            var _pPos = _stream.CurrentPosition();
            INode child = null;

            //if (objName.Type != TokenType.Identifier) Unexpected();

            if (IsSeparator("."))
            {
                SkipSeparator(".");
                var ntoken = _stream.Next();
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

        private INode ParseExpression()
        {
            return MaybeCall(() => MaybeBinary(ParseAtom(), 0));
        }

        private INode ParseObject()
        {
            var _pPos = _stream.CurrentPosition();

            var obj = new NodeJSObject() { FileInfo = _pPos};
            if (IsSeparator("{")) SkipSeparator("{");
            if (IsSeparator("}")) return obj;

            while (!_stream.Eof())
            {
                SkipAllLineBreak();
                if (_stream.Peek().Type == TokenType.String || _stream.Peek().Type == TokenType.Identifier)
                {
                    var nameTok = _stream.Next();
                    INode name = null;

                    if (nameTok.Type == TokenType.String) name = new NodeString { Value = nameTok.Value, FileInfo = _stream.CurrentPosition() };
                    else name = new NodeIdentifier { Value = nameTok.Value, FileInfo = _stream.CurrentPosition() };
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

        private INode ParseClass()
        {
            var _pPos = _stream.CurrentPosition();
            SkipKeyword("class");
            SkipAllLineBreak();
            INode extend = null;
            INode name = null;
            var isAnonymous = false;
            if (_stream.Peek().Type == TokenType.Identifier)
            {
                name = new NodeIdentifier { Value = _stream.Next().Value };
            }
            else
            {
                isAnonymous = true;
            }
            if (IsKeyword("extends"))
            {
                SkipKeyword("extends");
                if (_stream.Peek().Type != TokenType.Identifier) Unexpected();
                extend = new NodeIdentifier { Value = _stream.Next().Value };
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
        private INode ParseBool()
        {
            var _pPos = _stream.CurrentPosition();
            var val = _stream.Peek().Value;
            SkipKeyword(_stream.Peek().Value);
            return new NodeBoolean { Value = val == "true", FileInfo = _pPos};
        }

        private INode ParseVardef(Token before = null)
        {
            var _pPos = _stream.CurrentPosition();
            var name = ParseVarname(before);
            INode def = null;
            if (IsOperator("="))
            {
                _stream.Next();
                def = ParseExpression();
            }

            if (IsSeparator(";"))
            {
                useLastVarType = false;
                //SkipSeparator(";");
            }
            // Pokud je promenna oddelena carkama, definuje se vic promennych daneho typu
            if (IsSeparator(","))
            {
                useLastVarType = true;
            }
            return new NodeAssing { Operator = "=", Left = name, Right = def, FileInfo = _pPos};
        }

        private INode ParseProgram()
        {
            var _pPos = _stream.CurrentPosition();
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

            while (!_stream.Eof())
            {
                SkipAllLineBreak();
                prog.Add(ParseExpression());
                if (!_stream.Eof())
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

        private INode ParseIf()
        {
            var _pPos = _stream.CurrentPosition();
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
                _stream.Next();
                SkipAllLineBreak();
                ret.Else = IsSeparator("{") ? ParseProgram() : ParseExpression();
            }

            return (INode)ret;
        }

        private INode ParseWhile()
        {
            var _pPos = _stream.CurrentPosition();
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

        private INode ParseDoWhile()
        {
            var _pPos = _stream.CurrentPosition();
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

        private INode ParseFor()
        {
            var _pPos = _stream.CurrentPosition();
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

        private INode ParseForMultiParam()
        {
            var a = new List<INode>();
            while (!_stream.Eof())
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

        private INode ParseForOf()
        {
            var _pPos = _stream.CurrentPosition();
            SkipSeparator("(");
            if (_stream.Peek().Value != "var" && _stream.Peek().Value != "let" && _stream.Peek().Value != "const")
            {
                Unexpected();
            }
            var varDef = ParseVarname();
            if (_stream.Peek().Value != "of" && !IsKeyword("in")) Unexpected();
            var isForOf = !IsKeyword("in");
            _stream.Next();
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

        private INode ParseArrowFunction(List<INode> args, bool isAsync = false)
        {
            var _pPos = _stream.CurrentPosition();
            if (IsOperator("=>"))
            {
                SkipOperator("=>");
            }

            return new NodeFunction
            {
                IsAsync = isAsync,
                IsAnonymous = true,
                Arguments = args,
                Body = IsSeparator("{") ? ParseProgram() : new NodeProgram() { Program = [new NodeReturn { Body = ParseExpression() }] },
                FileInfo = _pPos
            };
        }

        private INode ParseFunction(bool isAsync = false)
        {
            var _pPos = _stream.CurrentPosition();
            SkipKeyword("function");
            var isGenerator = IsSeparator("*");
            if (IsSeparator("*")) SkipSeparator("*");
            var isAnonym = false;
            NodeIdentifier name = null;
            if (IsSeparator("(")) isAnonym = true;
            if (!isAnonym) name = new NodeIdentifier { Value = _stream.Next().Value };
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

        /// <summary>
        /// Pokud je promenna pri definici oddelena carkama, je potreba si ulozit posledni typ
        /// </summary>
        private string lastVarType = "";
        private bool useLastVarType = false;
        private INode ParseVarname(Token before = null)
        {
            var _pPos = _stream.CurrentPosition();
            string varType = null;
            if (IsKeyword("const") || IsKeyword("let") || IsKeyword("var"))
            {
                varType = _stream.Peek().Value;
                SkipKeyword();
            }
            var name = useLastVarType && varType == null ? before : _stream.Next();
            if (name.Type != TokenType.Identifier) Unexpected();

            if (useLastVarType && varType == null) varType = lastVarType;
            
            if (varType != null)
            {
                lastVarType = varType;
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

        private INode ParseArgument()
        {
            var name = _stream.Next();
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

        private INode ParseTopLevel()
        {
            var _pPos = _stream.CurrentPosition();
            var prog = new List<INode>();

            while (!_stream.Eof())
            {
                SkipAllLineBreak();
                if (_stream.Eof()) break;

                prog.Add(ParseExpression());
                if (!_stream.Eof())
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

        private INode ParseIncDecOperator(INode identifier, bool after = true)
        {
            var _pPos = _stream.CurrentPosition();
            var decinc = new NodeIncDecOperator() { FileInfo = _pPos};
            decinc.Identifier = identifier as NodeIdentifier;
            decinc.Operator = _stream.Next().Value;
            decinc.AfterVar = after;

            return decinc;
        }

        private INode MaybeCall(Func<INode> action)
        {
            var actResult = action();
            return IsSeparator("(") ? ParseCall(actResult) : actResult;
            //if (IsSeparator("(")) return ParseCall(actResult);
            //else if (IsOperator("=>")) return ParseArrowFunction(null);
            //else return actResult;
        }

        private INode MaybeBinary(INode left, int priority)
        {
            if (IsOperator())
            {
                if (IsOperator("++") || IsOperator("--"))
                {
                    return ParseIncDecOperator(left);
                }
                var itsPriority = AstTreeMethods.OperatorsPriority[_stream.Peek().Value];
                if (itsPriority > priority)
                {
                    var tok = _stream.Next();
                    var right = MaybeBinary(ParseAtom(), itsPriority);
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
            while (!_stream.Eof())
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
            _stream.ReadNextWithHistory();
            SkipAllLineBreak(true);
            var tok = _stream.ReadNextWithHistory();
            SkipAllLineBreak(true);
            if (IsOperator(":"))
            {
                _stream.ReverseToHistoryStart();
                return true;
            }
            _stream.ReverseToHistoryStart();

            return false;
        }

        public bool IsForOfOrForIn()
        {
            _stream.ReadNextWithHistory();
            _stream.ReadNextWithHistory();
            _stream.ReadNextWithHistory();
            if (_stream.Peek().Value == "of" || IsKeyword("in"))
            {
                _stream.ReverseToHistoryStart();
                return true;
            }

            _stream.ReverseToHistoryStart();
            return false;
        }

        public bool IsArrowFunction()
        {
            var bracket = 0;
            if (IsSeparator("("))
            {
                while (!_stream.Eof())
                {
                    if (IsSeparator("(")) bracket++;
                    else if (IsSeparator(")"))
                    {
                        if (--bracket == 0)
                        {
                            _stream.ReadNextWithHistory();
                            if (IsOperator("=>"))
                            {
                                _stream.ReverseToHistoryStart();
                                return true;
                            }
                            _stream.ReverseToHistoryStart();
                            return false;
                        }
                    }
                    _stream.ReadNextWithHistory();
                }
            }
            else if (_stream.Peek().Type == TokenType.Identifier)
            {
                _stream.ReadNextWithHistory();
                if (IsOperator("=>"))
                {
                    _stream.ReverseToHistoryStart();
                    return true;
                }
            }

            return false;
        }

        public bool IsSeparator(string sep = null)
        {
            return _stream.Peek()?.Type == TokenType.Separator && (sep == null || _stream.Peek()?.Value == sep);
        }

        public bool IsKeyword(string keyword = null)
        {
            return _stream.Peek()?.Type == TokenType.Keyword && (keyword == null || _stream.Peek()?.Value == keyword);
        }

        public bool IsOperator(string op = null)
        {
            return _stream.Peek()?.Type == TokenType.Operator && (op == null || _stream.Peek()?.Value == op);
        }

        public void SkipSeparator(string sep = null)
        {
            if (IsSeparator(sep)) _stream.Next();
            else Unexpected();
        }

        public void SkipKeyword(string keyword = null)
        {
            if (IsKeyword(keyword)) _stream.Next();
            else Unexpected();
        }

        public void SkipOperator(string op = null)
        {
            if (IsOperator(op)) _stream.Next();
            else Unexpected();
        }

        public void SkipAllLineBreak(bool withHistory = false)
        {
            while (!_stream.Eof() && IsSeparator("LB"))
            {
                if (withHistory) _stream.ReadNextWithHistory();
                else _stream.Next();
            }
        }

        public void Unexpected()
        {
            throw new ParserException($"Unexpected token '{_stream.Peek().Value}'", _stream.CurrentPosition());
        }
    }
}
