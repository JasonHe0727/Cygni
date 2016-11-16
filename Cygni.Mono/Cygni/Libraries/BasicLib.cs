﻿using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Cygni.AST;
using Cygni.DataTypes;
using Cygni.Extensions;
using Cygni.Errors;
using System.Reflection;
using System.IO;
using Cygni.Settings;
using System.Threading;
namespace Cygni.Libraries
{
	/// <summary>
	/// Description of BasicLib.
	/// </summary>
	public static class BasicLib
	{
		public static DynValue print (DynValue[] args)
		{
			if (args == null || args.Length == 0)
				return DynValue.Null;
			Console.Write (args [0].Value);
			for (int i = 1; i < args.Length; i++) {
				Console.Write ('\t');
				Console.Write (args [i].Value);
			}
			Console.WriteLine ();
			return DynValue.Null;
		}

		public static DynValue printf (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "printf");
			var arguments = new object[args.Length - 1];
			for (int i = 0; i < arguments.Length; i++)
				arguments [i] = args [i + 1].Value;
			Console.WriteLine (args [0].AsString (), arguments);
			return DynValue.Null;
		}

		public static DynValue Struct (DynValue[] args)
		{
			if ((args.Length & 1) == 0) {/* even */
				var structure = new Structure (args.Length >> 1);
				for (int i = 0, j = 0; i < args.Length - 1; i += 2,j++)
					structure.SetAt (j, args [i].AsString (), args [i + 1]);
				return new DynValue (DataType.Struct, structure);
			} 
			throw RuntimeException.BadArgsNum ("struct", "even");
		}

		public static DynValue cast (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "cast");
			string typeName = args [1].AsString ();
			object obj = args [0].Value;
			switch (typeName.ToLower ()) {
			case "int16":
				return DynValue.FromUserData (Convert.ToInt16 (obj));
			case "int":
			case "int32":
				return DynValue.FromUserData (Convert.ToInt32 (obj));
			case "long":
			case "int64":
				return DynValue.FromUserData (Convert.ToInt64 (obj));
			case "float":
			case "single":
				return DynValue.FromUserData (Convert.ToSingle (obj));
			case "double":
			case "number":
				return DynValue.FromNumber (Convert.ToDouble (obj));
			case "bool":
			case "boolean":
				return DynValue.FromBoolean (Convert.ToBoolean (obj));
			case "string":
				return DynValue.FromString (Convert.ToString (obj));
			case "char":
				return DynValue.FromUserData (Convert.ToChar (obj));
			case "datetime":
			case "date":
			case "time":
				return DynValue.FromUserData (Convert.ToDateTime (obj));
			default:
				return DynValue.FromObject (Convert.ChangeType (obj, Type.GetType (typeName)));
			}
		}

		public static DynValue typeinfo (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "typeinfo");
			if (args [0].type != DataType.UserData)
				return DynValue.FromString (args [0].type.ToString ());
			else
				return DynValue.FromString (args [0].GetDynType ().Name);
		}

		public static DynValue toNumber (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "toNumber");
			var value = args [0];
			if (value.type == DataType.Number)
				return value;
			else
				return Convert.ToDouble (value.Value);
		}

		public static DynValue toString (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 1, "toString");
			var value = args [0];
			if (value.type == DataType.String)
				return value;
			else
				return Convert.ToString (value.Value);
		}

		public static DynValue quiet (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length <= 1, "toString");
			if (args.Length == 0) {
				GlobalSettings.Quiet = true;
			} else {
				GlobalSettings.Quiet = args [0].AsBoolean ();
			}
			return GlobalSettings.Quiet;
		}

		public static DynValue scan (DynValue[] args)
		{
			if (args.Length == 1)
				Console.Write (args [0].AsString ());
			return Console.ReadLine ();
		}

		public static DynValue bit_or (DynValue[] args)
		{
			int value = (int)args [0].AsNumber ();
			for (int i = 1; i < args.Length; i++) {
				value |= (int)args [i].AsNumber ();
			}
			return (double)value;
		}

		public static DynValue CSharpDll (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2, "CSharpDll");
			string filepath = args [0].AsString ();
			string class_name = args [1].AsString ();
			if (!Path.IsPathRooted (filepath))
				filepath = Path.GetFullPath (filepath);

			Assembly assembly = Assembly.LoadFile (filepath);
			Type t = assembly.GetType (class_name, true, true);  //namespace.class
			MethodInfo[] methods = t.GetMethods ();
			var list = new List<StructureItem> ();

			foreach (MethodInfo method in methods.Where(i => i.ReturnType == typeof(DynValue))) {
				var parameters = method.GetParameters ();
				if (parameters.Length == 1 && parameters [0].ParameterType == typeof(DynValue[])) {
					var method_name = method.Name;
					list.Add (new StructureItem (method_name, DynValue.FromDelegate (
						method.CreateDelegate (typeof(Func<DynValue[],DynValue>)) as Func<DynValue[],DynValue>)));
				}
			}
			var arr = new StructureItem[list.Count];
			list.CopyTo (arr);
			Structure structure = new Structure (arr);
			return DynValue.FromStructure (structure);
		}

		public static DynValue console_clear (DynValue[]args)
		{
			Console.Clear ();
			return DynValue.Null;
		}

		public static DynValue console_write (DynValue[]args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "console.write");
			if (args.Length == 1)
				Console.Write (args [0].Value);
			else {
				var arguments = new object[args.Length - 1];
				for (int i = 0; i < arguments.Length; i++)
					arguments [i] = args [i + 1].Value;
				Console.Write (args [0].AsString (), arguments);
			}
			return DynValue.Null;
		}

		public static DynValue console_read (DynValue[]args)
		{
			return (double)Console.Read ();
		}

		public static DynValue console_readLine (DynValue[]args)
		{
			return Console.ReadLine ();
		}

		public static DynValue os_clock (DynValue[] args)
		{
			return (double)DateTime.Now.Ticks;
		}

		public static DynValue dispose (DynValue[] args)
		{
			for (int i = 0; i < args.Length; i++) {
				args [i].As<IDisposable> ().Dispose ();
			}
			return DynValue.Null;
		}

		public static DynValue exit (DynValue[] args)
		{
			Environment.Exit ((int)args [0].AsNumber ());
			return DynValue.Null;
		}

		public static DynValue Throw (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length >= 1, "throw");
			if (args.Length == 1)
				throw new RuntimeException (args [0].AsString ());
			else
				throw new RuntimeException (args [0].AsString (), args.SkipMap (1, i => i.Value));
		}

		public static DynValue Range (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length == 2||args.Length == 3, "range");
			if (args.Length == 2)
				return DynValue.FromUserData (Extension.Range ((int)args [0].AsNumber (), 
					(int)args [1].AsNumber ()).Select (i=>DynValue.FromNumber(i)));
			else
				return DynValue.FromUserData (Extension.Range ((int)args [0].AsNumber ()
					, (int)args [1].AsNumber ()
					, (int)args [2].AsNumber ()).Select (i=>DynValue.FromNumber(i)));
		}
		public static DynValue collect (DynValue[] args)
		{
			RuntimeException.FuncArgsCheck (args.Length ==1, "collect");
			return new DynList (args [0].As<IEnumerable<DynValue>> ());
		}
		public static DynValue len(DynValue[] args){
			RuntimeException.FuncArgsCheck (args.Length ==1, "len");
			return (double) args [0].As<ICollection> ().Count;
		}
		public static DynValue tryCatch(DynValue[] args){
			/* Inspire by Lua */
			RuntimeException.FuncArgsCheck (args.Length >= 1, "tryCatch");
			var func = args [0].As<IFunction> ();
			try {
				DynValue[] parameters = new DynValue[args.Length - 1];
				for (int i = 0; i < parameters.Length; i++) 
					parameters[i] = args[i+1];
				func.DynInvoke(parameters);
				return true;
			} catch (RuntimeException ex) {
				return ex.Message;
			}
			catch (Exception ex){
				return ex.Message;
			}
		}
	}
}