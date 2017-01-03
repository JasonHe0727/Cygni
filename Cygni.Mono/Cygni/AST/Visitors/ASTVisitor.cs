﻿using System;

namespace Cygni.AST.Visitors
{
	internal abstract class ASTVisitor
	{
		internal virtual void Visit (BinaryEx binaryEx)
		{
			binaryEx.Left.Accept (this);
			binaryEx.Right.Accept (this);
		}

		internal virtual void Visit (AssignEx assignEx)
		{
			assignEx.Target.Accept (this);
			assignEx.Value.Accept (this);
		}

		internal virtual void Visit (UnpackEx unpackEx)
		{
			foreach (var item in unpackEx.Items) {
				item.Accept (this);
			}
			unpackEx.Tuple.Accept (this);
		}

		internal virtual void Visit (RangeEx rangeEx)
		{
			rangeEx.Start.Accept (this);
			rangeEx.End.Accept (this);
			if (rangeEx.Step != null) {
				rangeEx.Step.Accept (this);
			}
		}

		internal virtual void Visit (BlockEx blockEx)
		{
			foreach (var item in blockEx.expressions)
				item.Accept (this);
		}

		internal virtual void Visit (LocalEx localEx)
		{
			foreach (var variable in localEx.Variables) {
				variable.Accept (this);
			}
			foreach (var value in localEx.Values) {
				value.Accept (this);
			}
		}

		internal virtual void Visit (Constant constant)
		{
			return;
		}

		internal virtual void Visit (DefClassEx defClassEx)
		{
			defClassEx.Body.Accept (this);
		}

		internal virtual void Visit (DefFuncEx defFuncEx)
		{
			defFuncEx.Body.Accept (this);
		}

		internal virtual void Visit (DefClosureEx defClosureEx)
		{
			defClosureEx.Body.Accept (this);
		}

		internal virtual void Visit (DotEx dotEx)
		{
			dotEx.Target.Accept (this);
		}

		internal virtual void Visit (ForEx forEx)
		{
			forEx.Iterator.Accept (this);
			forEx.Collection.Accept (this);
			forEx.Body.Accept (this);
		}

		internal virtual void Visit (ConditionEx conditionEx)
		{
			foreach (Branch branch in conditionEx.Branches) {
				branch.Condition.Accept (this);
				branch.Block.Accept (this);
			}
			if (conditionEx.ElseBlock != null) {
				conditionEx.ElseBlock.Accept (this);
			}
		}

		internal virtual void Visit (IfEx ifEx)
		{
			ifEx.Condition.Accept (this);
			ifEx.IfTrue.Accept (this);
			if (ifEx.IfFalse != null) {
				ifEx.IfFalse.Accept (this);
			}
		}

		internal virtual void Visit (IndexEx indexEx)
		{
			indexEx.Collection.Accept (this);
			foreach (var item in indexEx.Indexes)
				item.Accept (this);
		}

		internal virtual void Visit (SingleIndexEx indexEx)
		{
			indexEx.Collection.Accept (this);
			indexEx.Index.Accept (this);
		}

		internal virtual void Visit (InvokeEx invokeEx)
		{
			invokeEx.Func.Accept (this);
			foreach (var item in invokeEx.Arguments)
				item.Accept (this);
		}

		internal virtual void Visit (ListInitEx listInitEx)
		{
			foreach (var item in listInitEx.Initializers)
				item.Accept (this);
		}

		internal virtual void Visit (DictionaryInitEx dictionaryInitEx)
		{
			foreach (var item in dictionaryInitEx.Initializers) {
				item.Key.Accept (this);
				item.Value.Accept (this);
			}
		}

		internal virtual void Visit (NameEx nameEx)
		{
			nameEx.Accept (this);
			return;
		}

		internal virtual void Visit (ReturnEx returnEx)
		{
			returnEx.Value.Accept (this);
		}

		internal virtual void Visit (UnaryEx unaryEx)
		{
			unaryEx.Operand.Accept (this);
		}

		internal virtual void Visit (WhileEx whileEx)
		{
			whileEx.Condition.Accept (this);
			whileEx.Body.Accept (this);
		}

	}
}

