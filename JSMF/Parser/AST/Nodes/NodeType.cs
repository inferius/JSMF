﻿namespace JSMF.Parser.AST.Nodes
{
    public enum NodeType { 
        Class,
        Property,
        New,
        Array,
        Await,
        Number,
        String,
        Boolean,
        Identifier,
        Null,
        Call,
        ObjectCall,
        Assign,
        Binary,
        Program,
        Condition,
        Function,
        JSObject,
        Break,
        Continue,
        Return,
        While,
        ForOf,
        For,
        IncDecOperator,
        Symbol,
        Generator,
        JSValue
    }
}
