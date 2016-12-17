﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;
using Cygni.AST;
using Cygni.AST.Scopes;

namespace Cygni.Executors
{
	/// <summary>
	/// Description of Executor.
	/// </summary>
	public abstract class Executor
	{
		public abstract DynValue Run();
		protected IScope GlobalScope;
		protected Executor(IScope GlobalScope){
			this.GlobalScope = GlobalScope;
		}
	}
}
