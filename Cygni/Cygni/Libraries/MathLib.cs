﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Errors;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of MathLib.
	/// </summary>
	public static class MathLib
	{
		public static DynValue sqrt(DynValue[] args)
		{
			return DynValue.FromNumber(Math.Sqrt(args[0].AsNumber()));
		}
		public static DynValue abs(DynValue[] args)
		{
			return DynValue.FromNumber(Math.Abs(args[0].AsNumber()));
		}
		public static DynValue log(DynValue[] args)
		{
			if (args.Length == 1)
				return DynValue.FromNumber(Math.Log(args[0].AsNumber()));
			if (args.Length == 2)
				return DynValue.FromNumber(Math.Log(args[0].AsNumber(), args[1].AsNumber()));
			throw RuntimeException.BadArgsNum ("log", "1 or 2");
		}
		public static DynValue max(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "max");
			return DynValue.FromNumber(Math.Max(args[0].AsNumber(), args[1].AsNumber()));
		}
		public static DynValue min(DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "min");
			return DynValue.FromNumber(Math.Min(args[0].AsNumber(), args[1].AsNumber()));
		}
		public static DynValue exp(DynValue[] args)
		{
			return DynValue.FromNumber(Math.Exp(args[0].AsNumber()));
		}
		public static DynValue sign(DynValue[] args){
			return (double)Math.Sign(args[0].AsNumber());
		}
	}
}
