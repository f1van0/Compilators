using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
	{
	/// <summary>
	/// Компилятор (интерпретатор) выражений
	/// </summary>
	public static class Compilator
		{
		/// <summary>
		/// Набор базовых операций над идентификаторами
		/// </summary>
		public static Dictionary<string, Operator> Operators =
			new Dictionary<string, Operator>
				{
				["+"] = new Operator("+", 2, delegate(ref Identifier first, Identifier[] args)
					{return first.Value + args[0].Value;}, 3),

				["-"] = new Operator("-", 2, delegate (ref Identifier first, Identifier[] args)
					{ return first.Value - args[0].Value; }, 3),

				["*"] = new Operator("*", 2, delegate (ref Identifier first, Identifier[] args)
					{ return first.Value * args[0].Value; }, 4),

				["/"] = new Operator("/", 2, delegate (ref Identifier first, Identifier[] args)
					{ return first.Value / args[0].Value; }, 4),

				["="] = new Operator("=", 2, delegate (ref Identifier first, Identifier[] args)
					{ IdentifierManager.ReAssign(ref first, args[0].Value); return first.Value; }, 1),
				};
		/// <summary>
		/// Набор служебных символов
		/// </summary>
		static Dictionary<string, Service> Service =
			new Dictionary<string, Service>
				{
				[";"] = new Service(";", 0),
				["("] = new Service("(", 2),
				[")"] = new Service(")", 2),
				};

		/// <summary>
		/// Получает список базовых операторов
		/// </summary>
		public static List<string> AllOperators { get { return Operators.Keys.ToList(); } }

		/// <summary>
		/// Получает список служебных символов
		/// </summary>
		public static List<string> AllServiceSymbols { get { return Service.Keys.ToList(); } }

		/// <summary>
		/// Набор операторов и служебных символов
		/// </summary>
		public static Dictionary<string, ILexem> Symbols;

		static Compilator ()
			{
			Symbols = new Dictionary<string, ILexem>();
			foreach ( var item in Operators )
				Symbols.Add(item.Key, item.Value);
			foreach ( var item in Service )
				Symbols.Add(item.Key, item.Value);
			}

		/// <summary>
		/// Разбивает выражение на лексемы
		/// </summary>
		/// <param name="_formula">Текст программы</param>
		/// <returns></returns>
		static List<string> Partionize (string _formula)
			{
			string formula = new String(_formula.Where(chr => chr != ' ').ToArray());
			HashSet<string> symbols = new HashSet<string>(Compilator.AllOperators.Concat(Compilator.AllServiceSymbols));
			//получаю последовательность идентификаторов
			var Identifiers = formula.Split(symbols.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
			string buffer = String.Join("", formula.Split(Identifiers, StringSplitOptions.RemoveEmptyEntries));
			StringBuilder build = new StringBuilder();

			//получаю последовательность операторов
			List<string> Opers = new List<string>();
			foreach ( var symb in buffer )
				{
				build.Append(symb);
				if ( symbols.Contains(build.ToString()) )
					{
					Opers.Add(build.ToString());
					build.Clear();
					}
				}

			if ( build.Length > 0 )
				throw new LexemException("Ошибка разбора");
			int opCount = Opers.Count();
			int idCount = Identifiers.Count();
			List<string> partioned = new List<string>();
			int i = 0; //итератор по формуле
			int o = 0; //итератор по операторам
			int k = 0; //итератор по идентификаторам
			//сшиваю последовательности операторов и идентификаторов
			while ( idCount + opCount > 0 )
				{
				if ( o < Opers.Count() && Opers[o][0] == formula[i] )
					{
					opCount--;
					i += Opers[o].Length;
					partioned.Add(Opers[o++]);
					}
				else
				if ( k<Identifiers.Count() && Identifiers[k][0] == formula[i] )
					{
					idCount--;
					i += Identifiers[k].Length;
					partioned.Add(Identifiers[k++]);
					}
				}
			return partioned;
			}

		/// <summary>
		/// Лексический анализ
		/// </summary>
		/// <param name="code"></param>
		/// <returns>Возвращает код разбитый на лексемы</returns>
		public static List<List<string>> LAnalize(string code)
			{
			List<string> lines = code.Split("\n\r".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
			return lines.Select(elem => Partionize(elem)).ToList();
			}

		/// <summary>
		/// Синтаксический анализ
		/// </summary>
		/// <param name="parts">Последовательности лексем</param>
		/// <param name="Identifiers">Идентификаторы переменных</param>
		/// <returns>Возвращает последовательности лексем состоящих из операторов и идентификаторов</returns>
		public static List<List<ILexem>> SAnalize (List<List<string>> parts, out HashSet<ILexem> Identifiers)
			{
			List<ILexem> Lexems;
			List<List<ILexem>> Postfixs = new List<List<ILexem>>();
			Identifiers = new HashSet<ILexem>();
			foreach ( var item in parts )
				{
				Lexems = ToLexems(item);
				Identifiers.UnionWith(Lexems); // добавляю новые идентификаторы
				Postfixs.Add(Postfix(Lexems));
				}
			return Postfixs;
			}

		/// <summary>
		/// Синтаксический анализирует выражение состоящее из отдельных слов
		/// </summary>
		/// <param name="expression"></param>
		/// <returns>Возвращает последовательность идентификаторов и операторов</returns>
		public static List<ILexem> ToLexems (List<string> expression)
			{
			List<ILexem> result = new List<ILexem>();
			EvalObject temp;
			for ( int i = 0; i < expression.Count(); i++ )
				if ( Operators.ContainsKey(expression[i]) )
					result.Add( Operators[expression[i]]);
				else
					if ( Service.ContainsKey(expression[i]))
						{
						result.Add(Service[expression[i]]);
						}
					else
						{
						temp = Memory.CreateObjectFromValue(expression[i]);
						if ( temp != null )
							result.Add(new Identifier(temp));
						else
							result.Add(IdentifierManager.ReferrenceIdentifier(expression[i]));
						}
			return result;
			}

		/// <summary>
		/// Формирует постфиксную запись выражения
		/// </summary>
		/// <param name="expression">Лексемы в инфиксной форме записи</param>
		/// <returns>Возвращает лексемы записанные в постфиксной записи</returns>
		public static List<ILexem> Postfix (List<ILexem> expression)
			{
			List<ILexem> postfix = new List<ILexem>();
			Stack<ILexem> opStack = new Stack<ILexem>();
			foreach ( var element in expression )
				{
				if ( element.Type == LexemType.Identifier || element.Type == LexemType.Constant )
					postfix.Add(element);
				else
					switch ( element.Literal )
						{
						case "(":
							{
							opStack.Push(element);
							break;
							}
						case ")":
							{
							var top = opStack.Pop();
							while ( top.Literal != "(" )
								{
								postfix.Add(top);
								top = opStack.Pop();
								}
							break;
							}
						default:
							{
							while ( opStack.Count > 0 )
								{
								ILexem lex = opStack.Peek();
								if ( Symbols[lex.Literal].Priority >= element.Priority )
									postfix.Add(opStack.Pop());
								else
									break;
								}
							opStack.Push(element);
							break;
							}
						}
				}
			while ( opStack.Count > 0 )
				postfix.Add(opStack.Pop());
			return postfix;
			}


		/// <summary>
		/// Выполняет постфиксную запись лексем
		/// </summary>
		/// <param name="Postfix"></param>
		/// <returns></returns>
		static public Identifier Evaluate (List<ILexem> Postfix)
			{
			Stack<ILexem> buffer = new Stack<ILexem>();
			foreach ( var item in Postfix )
				{
				if ( item.Type == LexemType.Identifier || item.Type == LexemType.Constant )
					buffer.Push(item);
				else
					{
					// получаю функцию, взаимодействующую с операторами
					ICallable func;
					if ( item.Type == LexemType.Operator )
						func = (Operator)item;
					else
						func = (Function)item;
					// достаю параметры функции
					List<Identifier> parameters = new List<Identifier>();
					for ( int i = 0; i < func.ParamCount-1; i++ )
						parameters.Add((Identifier)buffer.Pop());

					Identifier op1 = (Identifier)buffer.Pop();

					if ( op1.Value == null && item.Literal!= "=")
						throw new VariableException(op1.Literal, "Not assigned, but referenced");
					if ( parameters.Any((lex) => lex.Address == null) )
						throw new VariableException("Not assigned, but referenced");
					// выполняю вычисление
					EvalObject result = func.Evaluate(ref op1, parameters.ToArray());
					buffer.Push(new Identifier(result));
					}
				}
			var res = (Identifier)buffer.Pop();
			IdentifierManager.Refresh(ref res);
			return res;
			}
		}
	}
