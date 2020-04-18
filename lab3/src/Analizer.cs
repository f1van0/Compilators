using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace lab3
	{

	public enum SystemTypes
		{
		Char,
		String,
		Float,
		Int,
		Bool
		}

	[Serializable]
	public class CompileException : Exception
		{
		string msg = "Compile error";
		public CompileException () { }
		public CompileException (string message) : base(message) { msg += " | "; }
		public override string ToString ()
			{
			return msg + Message;
			}
		}

	[Flags]
	public enum State
		{
		Error = 0,
		/// <summary>
		/// Выражение
		/// </summary>
		Statement	= 4,
		Variable	= 4+1,
		Constant	= 4+2,
		Operator	= 8,
		Declaration = 32,
		Initialization = 32+16,
		IF			= 64,
		/// <summary>
		/// Действие
		/// </summary>
		Expression	= 128,
		Block		= 256,
		Program		= 512
		}

	public class Analizer
		{
		List<string> Lexems;

		readonly List<string> LongOperators;
		readonly List<string> ShortOperators;
		readonly List<string> InplaceOperators;
		readonly List<string> Operators;
		readonly char[] OperatorSymbols;

		public static Dictionary<string, Func<int, int, int>> FuncOperators =
			new Dictionary<string, Func<int, int, int>>
				{
				["+"] =  delegate (int first, int second)
					{ return first + second; },
				["-"] = delegate (int first, int second)
					{ return first - second; },
				["/"] = delegate (int first, int second)
					{ return first / second; },
				["*"] = delegate (int first, int second)
					{ return first * second; },

				};

		Stack<Expression> Program;
		HashSet<Expression> Variables;
		public Analizer()
			{
			Variables = new HashSet<Expression>();
			Lexems = new List<string> { "{", "}", "if ", "else", ";", "(", ")" };
			LongOperators = new List<string> { "==", ">=", "<=" , "||", "&&"};
			ShortOperators = new List<string> { ">", "<", "*", "/", "-", "+" };
			InplaceOperators = new List<string> { "*=", "/=", "-=", "+=" };
			Operators = new List<string>(LongOperators.Concat(ShortOperators));
			OperatorSymbols = new HashSet<char>(String.Concat(Operators)).ToArray();
			Program = new Stack<lab3.Expression>();
			}

		public string analizeCode(string code)
			{
			Program.Clear();
			parseLine(code, State.Program);
			StringBuilder str = new StringBuilder();
			CProgram prog = Program.Peek() as CProgram;
			int index = 0;
			if ( prog != null )
				{
				if (prog.Errors.Count==0)
					return String.Join("\r\n", WriteTree(prog, ref index));
				return String.Join("\r\n", prog.Errors);
				}
			return "";
			}

		public List<string> WriteTree(Expression exp, ref int index)
			{
			List<string> lines = new List<string>();
			lines.Add(new String(' ', index) + exp);
			index++;
			foreach ( var line in exp.Children )
				lines.AddRange(WriteTree(line, ref index));
			index--;
			return lines;
			}

		public static SystemTypes? Recognise (string value)
			{
			int temp_int;
			float temp_float;
			if ( value == "" )
				return null;
			if ( int.TryParse(value, out temp_int) )
				return SystemTypes.Int;
			if ( float.TryParse(value, out temp_float) )
				return SystemTypes.Float;
			if ( value.ToLower() == "false" || value.ToLower() == "true" )
				return SystemTypes.Bool;
			if ( value.First() == '\'' && value.Last() == '\'' && value.Length == 3 )
				return SystemTypes.Char;
			if ( value.First() == '\"' && value.Last() == '\"' )
				return SystemTypes.String;
			return null;
			}

		Expression findUndeclared(HashSet<Expression> declarations, Expression source, State Type)
			{
			Expression res;
			if ( source.Type == Type && !declarations.Contains(source) )
				return source;
			foreach ( var child in source.Children )
				{
				res = findUndeclared(declarations, child, Type);
				if ( res != null )
					return res;
				}
			return null;
			}

		bool parseLine (string code, State search)
			{
			#region StateChecking
			switch ( search )
				{
				case State.Variable:
					{
					#region Variable

					code = code.Trim(" \t".ToArray());
					// проверка выражения A2b4c6d
					Regex rVar = new Regex(@"^\w(?:\w|\d)*$");
					Match mVar = rVar.Match(code);
					if ( mVar.Success )
						{
						Expression var = new Expression(mVar.Groups[0].Value, State.Variable);

						//bool declared = Variables.Contains(var);
						//if ( !declared )
						//	throw new LexemException("Unexpected identifier encountered");
						Program.Push(var);
						return true;
						}
					else
						throw new CompileException($"Unknown identifier '{code}'");
					#endregion
					}

				case State.Constant:
					{
					#region	Constant
					code = code.Trim(" \t".ToArray());
					SystemTypes? constVal = Recognise(code);
					if ( constVal != null )
						{
						Program.Push(new Expression(code, State.Constant));
						return true;
						}
					return false;
					#endregion
					}

				case State.Declaration:
					{
					#region Declaration
					Regex rdecl = new Regex(@"^int\s+(.+\s*,?)*\s*$");
					Match mdecl = rdecl.Match(code);
					if ( mdecl.Success )
						{
						List<Expression> Initializes = new List<Expression>();
						string[] variables = mdecl.Groups[1].Value.Split(',');
						foreach ( var variable in variables )
							if ( parseLine(variable, State.Initialization) )
								Initializes.Add(Program.Pop());
							else
								throw new CompileException("Wrong initialization saw");

						var newVars = Initializes.Select(init => init.Children[0]);
						Variables = new HashSet<Expression>(Variables.Concat(newVars));

						Expression Declaration = new Expression($"int {String.Join(", ", Initializes.Select(var => var.Code))}", State.Declaration);
						Declaration.Children.AddRange(Initializes);
						Program.Push(Declaration);
						return true;
						}
					return false;
					#endregion
					}

				case State.Initialization:
					{
					#region Initialization

					Regex rInit = new Regex(@"\s*(?<left>.+)\s*=\s*(?<right>.+)\s*");
					Match mInit = rInit.Match(code);
					if ( mInit.Success )
						{
						if ( !parseLine(mInit.Groups["left"].Value, State.Variable) )
							throw new CompileException($"Expected variable, but encountered '{mInit.Groups["left"].Value}'");
						Initialization Initialization;
						Expression variable = Program.Pop();
						Expression value;
						if ( parseLine(mInit.Groups["right"].Value, State.Statement) )
							{
							value = Program.Pop();
							Initialization = new Initialization(variable, value);
							Program.Push(Initialization);
							return true;
							}
						else
							throw new CompileException($"Expected statement after '{variable.Code} = '");
						}
					return false;
					#endregion
					}

				case State.IF:
					{
					#region IF

					Regex rIf = new Regex(@"^if\s*\((.*)\)\s*$");
					Match mIf = rIf.Match(code);
					if ( mIf.Success )
						{
						if ( parseLine(mIf.Groups[1].Value, State.Statement) )
							{
							Expression statement = Program.Peek();
							code.Replace(mIf.Groups[0].Value, "");
							Expression ifBlock = new Expression($"if ({statement.Code})", State.IF);
							ifBlock.Children.Add(statement);
							Program.Pop();
							Program.Push(ifBlock);
							return true;
							}
						else
							throw new CompileException("Expected statement in if block");
						}
					return false;
					#endregion
					}

				case State.Statement:
					{
					#region Statement

					Regex rInner = new Regex(@"^\(([^\(\)]+)\)$");
					Match mInner = rInner.Match(code);
					bool parantacies = mInner.Success;
					while ( mInner.Success )
						{
						code = mInner.Groups[1].Value;
						mInner = rInner.Match(code);
						}
					// проверка выражения a>=b
					Regex rStatement = new Regex(@"(?<left>.+)\s*(?<operator>[\*\/\+\-<=>\&\|]{2}|\+|\-|\/|\*|<|>)\s*(?<right>.+)");
					Match mStatement = rStatement.Match(code);
					if ( mStatement.Success )// && mStatement.Groups.Count == 4
						{
						Expression oper;
						string operStr = mStatement.Groups["operator"].Value;
						if ( Operators.Contains(operStr, StringComparer.InvariantCulture) ) 
							oper = new Expression(operStr, State.Operator);
						else
							throw new CompileException($"Unknown identifier encountered {operStr}");
							
						if ( !parseLine(mStatement.Groups["left"].Value, State.Statement) )
							throw new CompileException($"Expected statement before '{oper.Code}'");
						Expression left = Program.Pop();

							
						if ( !parseLine(mStatement.Groups["right"].Value, State.Statement) )
							throw new CompileException($"Expected statement after '{oper.Code}'");
						Expression right = Program.Pop();

						Expression statement;
						// проверяю можно ли применить простой оператор над целыми числами
						if ( left.Type == State.Constant && right.Type == State.Constant && FuncOperators.Keys.Contains(oper.Code) ) 
							{
							SystemTypes? Type1 = Recognise(left.Code);
							SystemTypes? Type2 = Recognise(right.Code);
							if ( Type1 == SystemTypes.Int && Type2 == SystemTypes.Int )
								{
								int result = FuncOperators[oper.Code](int.Parse(left.Code), int.Parse(right.Code));
								statement = new Expression(result.ToString(), State.Constant);
								Program.Push(statement);
								return true;
								}
							}
							if (parantacies)
								statement = new Expression($"({left.Code} {oper.Code} {right.Code})", State.Statement);
							else
								statement = new Expression($"{left.Code} {oper.Code} {right.Code}", State.Statement);
						statement.Children.Add(left);
						statement.Children.Add(oper);
						statement.Children.Add(right);
						Program.Push(statement);
						return true;
						}

					if ( parseLine(code, State.Constant) )
						return true;

					if ( parseLine(code, State.Variable) )
						return true;

					return false;
					#endregion
					}

				case State.Expression:
					{
					#region Expression

					Regex rexp = new Regex(@"\s*([\w\d\s=,\(\)\*/\-\+]+);\s*");
					MatchCollection mexp = rexp.Matches(code);
					if ( mexp.Count > 0 )
						{
						foreach ( Match match in mexp )
							{ 
							string exp = match.Groups[1].Value;
							if ( parseLine(exp, State.Declaration) )
								continue;
							if ( parseLine(exp, State.Initialization) )
								continue;
							throw new CompileException("Wrong expression occured before ';'");
							}
						return true;
						}

					if ( parseLine(code, State.IF) )
						return true;

					throw new CompileException("';' is required");
					#endregion
					}

				case State.Program:
					{
					#region Program
					List<string> errors = new List<string>();
					Expression IfBlock = new Expression("awds", State.Error);
					bool ifblock = false;
					int lineCount = 1;

					foreach ( var line in code.Split("\r\n\t".ToArray(), StringSplitOptions.RemoveEmptyEntries) )
						{
						try
							{
							parseLine(line, State.Expression);
							Expression exp = Program.Peek();
							Expression undef = findUndeclared(Variables, exp, State.Variable);
							if ( undef != null )
								{
								errors.Add($"Line {lineCount}: Encountered undeclared variable {undef.Code}");
								continue;
								}
							if ( exp.Type == State.IF )
								{
								IfBlock = Program.Pop();
								ifblock = true;
								}
							if ( ifblock && ( State.Initialization | State.Declaration ).HasFlag(exp.Type) )
								{
								ifblock = false;
								IfBlock.Children.Add(Program.Pop());
								Program.Push(IfBlock);
								}

							}
						catch (CompileException except )
							{
							errors.Add($"Line {lineCount}: {except.Message}");
							}
						lineCount++;
						}
					if ( ifblock )
						errors.Add($"Line {lineCount-1}: Expected end of if block");
					CProgram Prog = new CProgram(Program.Reverse(), errors);
					Program.Clear();
					Program.Push(Prog);
					return true;
					#endregion
					}
				default:
					return false;
				}
			#endregion
			}
			

		}
	}
