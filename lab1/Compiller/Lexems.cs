using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
	{
	[Serializable]
	public class TypeMismatchException : Exception
		{
		string msg = "Type Mismatch exception";
		public TypeMismatchException () { }
		public TypeMismatchException (string message) : base(message) { msg += " | ";}
		public override string ToString ()
			{
			return msg + Message;
			}
		}

	[Serializable]
	public class LexemException : Exception
		{
		string msg = "Lexem exception";
		public LexemException () { }
		public LexemException (string message) : base(message) { msg += " | "; }
		public override string ToString ()
			{
			return msg + Message;
			}
		}

	[Serializable]
	public class VariableException : Exception
		{
		string msg = "Variable exception";
		public VariableException () { }
		public VariableException (string message) : base(message) { }
		public VariableException (string name, string message) : base($"Variable {name} : {message}")
			{
			msg += " | ";
			}
		public override string ToString ()
			{
			return msg + Message;
			}
		}

	public enum LexemType
		{
		Constant	= 1,
		Identifier	= 2,
		Service		= 4,
		Function	= 8,
		Operator	= 16
		}

	/* 
	class Lexem
		{
		static HashSet<Char> Operands = new HashSet<char> { '+', '-', '*', '/', '=', '(', ')' };
		public Lexem (string formula)
			{
			formula = formula.Where(chr => chr != ' ').ToString();
			for ( int i = 0; i < formula.Length; i++ )
				if ( Operands.Contains(formula[i]) )
					{
					Value = $"{formula[i]}";
					Type = LexemType.Function;
					string left_formula = formula.Substring(0, i);
					string right_formula = formula.Substring(i + 1, formula.Length-(i+1));

					if ( left_formula.Length > 0 )
						Left = new Lexem(left_formula);
					if ( right_formula.Length > 0 )
						Right = new Lexem(right_formula);
					return;
					}
			try
				{
				float temp = float.Parse(formula);
				Value = temp.ToString();
				Type = LexemType.Constant;
				}
			catch ( FormatException )
				{
				Value = formula;
				Type = LexemType.Identifier;
				}

			}
		public float Evaluate ()
			{
			return 0;
			}

		public override string ToString ()
			{
			return $"{Right}{Left}{Value}";
			}

		public LexemType Type;
		public string Value;
		public int? Address;
		public Lexem Right;
		public Lexem Left;
		}
	 */

	public interface ILexem
		{
		string Literal { get; }
		LexemType Type { get; }
		string Serialize ();
		int Priority { get; }
		}

	public delegate EvalObject Caller(ref Identifier first, params Identifier[] args);

	public interface ICallable
		{
		EvalObject Evaluate (ref Identifier first, params Identifier[] args);
		int ParamCount { get; }
		}

	public abstract class Functor : ILexem, ICallable
		{
		public string Literal { get; }
		public int ParamCount { get; }

		public Functor (string expression, int paramCount)
			{
			Literal = expression;
			ParamCount = paramCount;
			}

		public abstract EvalObject Evaluate (ref Identifier first, params Identifier[] args);

		public string Serialize ()
			{
			return $"{Type}\t{Literal}";
			}

		public override string ToString ()
			{
			return Literal;
			}

		public abstract LexemType Type { get; }

		public abstract int Priority
			{
			get;
			}
		}

	public class Function : Functor
		{
		public override int Priority { get { return 0; } }
		Caller Func;
		public Function(string expression, int paramCount, Caller func) : base(expression, paramCount)
			{
			Func = func;
			}

		public override LexemType Type
			{
			get
				{
				return LexemType.Function;
				}
			}

		public override EvalObject Evaluate (ref Identifier first, params Identifier[] args)
			{
			return Func(ref first, args.ToArray());
			}
		}

	public class Operator : Functor
		{
		Caller Func;
		public override int Priority { get; }
		public Operator (string expression, int paramCount, int priority) : base(expression, paramCount)
			{
			Priority = priority;
			}
		public Operator (string expression, int paramCount, Caller func, int priority) : base(expression, paramCount)
			{
			Priority = priority;
			Func = func;
			}

		public override EvalObject Evaluate (ref Identifier first, params Identifier[] args)
			{
			if (Func != null)
				return Func(ref first, args.ToArray());
			throw new VariableException();
			}

		public override LexemType Type
			{
			get
				{
				return LexemType.Operator;
				}
			}
		}

	public class Identifier : ILexem
		{
		public uint? Address;
		public string Literal { get; }
		public LexemType Type { get; }
		public readonly SystemTypes? SystemType;
		public int Priority { get { return 0; } }
		public bool isNull { get { return Value != null; } }
		public EvalObject Value
			{
			get
				{
				if ( Address != null )
					return Memory.GetObject((uint)Address);
				else
					return null;
				}
			set
				{
				if ( value != null )
					{
					var newAddr = Memory.LocateObject(value.ToString());
					if ( Address == null )
						{
						Address = newAddr;
						return;
						}
					var cur = Memory.GetObject((uint)Address);
					var neww = Memory.GetObject((uint)newAddr);
					if ( cur.GetType() != neww.GetType() )
						throw new TypeMismatchException($"Can't pass object of type [{neww.SystemType}] to variable of [{cur.SystemType}] type");
					else
						Address = newAddr;
					}
				else
					Address = null;
				}
			}
		
		public Identifier(string name, SystemTypes? type)
			{
			Literal = name;
			Address = null;
			SystemType = type;
			Type = LexemType.Identifier;
			}

		public Identifier (string name, EvalObject obj)
			{
			Literal = name;
			Value = obj;
			SystemType = ( obj != null ) ? (SystemTypes?)Value.SystemType : null;
			Type = LexemType.Identifier;
			}

		public Identifier (EvalObject value)
			{
			Literal = value.ToString();
			Value = value;
			SystemType = Value.SystemType;
			Type = LexemType.Constant;
			}

		public override string ToString ()
			{
			return Literal.ToString();
			}

		public string Serialize ()
			{ 
			return $"{Type}\t{Literal}[{Address}]";
			}
			
		}

	public class Service : ILexem
		{
		public int Priority { get; }
		public string Literal { get; }
		public Service(string lexem, int priority)
			{
			Literal = lexem;
			Priority = priority;
			}
		public LexemType Type
			{
			get
				{
				return LexemType.Service;
				}
			}

		public override string ToString ()
			{
			return Literal;
			}

		public string Serialize ()
			{
			return $"{Type}\t{Literal}";
			}
		}
	}
