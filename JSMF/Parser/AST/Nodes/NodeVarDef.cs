﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSMF.Parser.AST.Nodes
{
    public class NodeVarDef : NodeIdentifier
    {
        public string VarType { get; set; }
    }
}
