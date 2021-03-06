﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST.Scopes;
using Cygni.AST.Visitors;

namespace Cygni.AST
{
    public class DictionaryInitEx:InitEx
    {
        public override NodeType type { get { return NodeType.DictionaryInit; } }

        public DictionaryInitEx(ASTNode[]  initializers)
            : base(initializers)
        {
        }

        public override DynValue Eval(IScope scope)
        {
            DynDictionary dict = new DynDictionary(initializers.Length);
            foreach (ASTNode item in initializers)
            {
                KeyValuePair pair = item.Eval(scope).As<KeyValuePair>();
                dict.Add(pair.Key, pair.Value);
            }
            return DynValue.FromDictionary(dict);
        }

        internal override void Accept(ASTVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

