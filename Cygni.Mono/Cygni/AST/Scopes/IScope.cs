﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.DataTypes;

namespace Cygni.AST.Scopes
{
	/// <summary>
	/// Description of IScope.
	/// </summary>
	public interface IScope
	{
		string ScopeName { get; }

		DynValue Get (string name);

		DynValue Put (string name, DynValue value);

		DynValue Get (int nest, int index);

		DynValue Put (int nest, int index, DynValue value);

		bool TryGetValue (string name, out DynValue value);

		IScope Parent { get; }

		ScopeType type { get; }
	}
}
