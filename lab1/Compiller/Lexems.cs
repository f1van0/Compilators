using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
{
    /// <summary>
    /// Вызывается при выполнении операции между недопустимыми типами
    /// </summary>
    [Serializable]
    public class TypeMismatchException : Exception
    {
        string msg = "Вызвано исключение несоответствия типа";
        public TypeMismatchException() { }
        public TypeMismatchException(string message) : base(message) { msg += " | "; }
        public override string ToString()
        {
            return msg + Message;
        }
    }

    /// <summary>
    /// Вызывается при ошибке лексического разбора
    /// </summary>
    [Serializable]
    public class LexemException : Exception
    {
        string msg = "Вызвано исключение связанное с лексемой";
        public LexemException() { }
        public LexemException(string message) : base(message) { msg += " | "; }
        public override string ToString()
        {
            return msg + Message;
        }
    }

    /// <summary>
    /// Вызывается во времени выполнения
    /// </summary>
    [Serializable]
    public class VariableException : Exception
    {
        string msg = "Вызвано исключение в выражении";
        public VariableException() { }
        public VariableException(string message) : base(message) { }
        public VariableException(string name, string message) : base($"Выражение {name} : {message}")
        {
            msg += " | ";
        }
        public override string ToString()
        {
            return msg + Message;
        }
    }

    /// <summary>
    /// Типы лексем в интерпретаторе
    /// </summary>
    public enum LexemType
    {
        Constant = 1,
        Identifier = 2,
        Service = 4,
        Function = 8,
        Operator = 16
    }

    /// <summary>
    /// Лексема
    /// </summary>
    public interface ILexem
    {
        /// <summary>
        /// Строковое представление лексемы
        /// </summary>
        string Literal { get; }

        /// <summary>
        /// Тип лексемы
        /// </summary>
        LexemType Type { get; }

        /// <summary>
        /// Системное представление лексемы
        /// </summary>
        /// <returns>Строковое описание лексемы</returns>
        string Serialize();

        /// <summary>
        /// Приоритет при синтаксическом разборе
        /// </summary>
        int Priority { get; }
    }

    /// <summary>
    /// Тип вызываемой функции с переменным числом аргументов
    /// </summary>
    /// <param name="first"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate EvalObject Caller(ref Identifier first, params Identifier[] args);

    /// <summary>
    /// Вызываемая функция
    /// </summary>
    public interface ICallable
    {
        /// <summary>
        /// Выполняет вызов функции
        /// </summary>
        /// <param name="first">Первый аргумент</param>
        /// <param name="args">Последующие</param>
        /// <returns></returns>
        EvalObject Evaluate(ref Identifier first, params Identifier[] args);

        /// <summary>
        /// Количество параметров функции
        /// </summary>
        int ParamCount { get; }
    }

    /// <summary>
    /// Функтор, вызываемый объект, всегда возвращает результат при вызове
    /// </summary>
    public abstract class Functor : ILexem, ICallable
    {
        public string Literal { get; }
        public int ParamCount { get; }

        public Functor(string expression, int paramCount)
        {
            Literal = expression;
            ParamCount = paramCount;
        }

        public abstract EvalObject Evaluate(ref Identifier first, params Identifier[] args);

        public string Serialize()
        {
            return $"{Type}\t{Literal}";
        }

        public override string ToString()
        {
            return Literal;
        }

        public abstract LexemType Type { get; }

        public abstract int Priority { get; }
    }

    /// <summary>
    /// Системная функция, принимает некоторое число аргументов
    /// </summary>
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

        public override EvalObject Evaluate(ref Identifier first, params Identifier[] args)
        {
            return Func(ref first, args.ToArray());
        }
    }

    /// <summary>
    /// Системный оператор
    /// </summary>
    public class Operator : Functor
    {
        Caller Func;
        public override int Priority { get; }
        public Operator(string expression, int paramCount, int priority) : base(expression, paramCount)
        {
            Priority = priority;
        }
        /// <summary>
        /// Конструктор встроенного оператора
        /// </summary>
        /// <param name="expression">Строковое представление</param>
        /// <param name="paramCount">Число параметров</param>
        /// <param name="func">Тело вызываемой функции</param>
        /// <param name="priority">Приоритет оператора</param>
        public Operator(string expression, int paramCount, Caller func, int priority) : base(expression, paramCount)
        {
            Priority = priority;
            Func = func;
        }

        public override EvalObject Evaluate(ref Identifier first, params Identifier[] args)
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

    /// <summary>
    /// Идентификатор - переменная или литерал(константа)
    /// </summary>
    public class Identifier : ILexem
    {
        /// <summary>
        /// Адрес объекта в памяти
        /// </summary>
        public uint? Address;
        public string Literal { get; }
        public LexemType Type { get; }
        public readonly SystemTypes? SystemType;
        public int Priority { get { return 0; } }

        /// <summary>
        /// Проверка на отсутствие значения
        /// </summary>
        public bool isNull { get { return Value != null; } }

        /// <summary>
        /// Хранимый объект
        /// </summary>
        public EvalObject Value
        {
            get
            {
                if (Address != null)
                    return Memory.GetObject((uint)Address);
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    Address = null;
                    return;
                }

                var newAddr = Memory.LocateObject(value.ToString());
                if (Address == null)
                {
                    Address = newAddr;
                    return;
                }
                var cur = Memory.GetObject((uint)Address);
                var neww = Memory.GetObject((uint)newAddr);
                if (cur.GetType() != neww.GetType())
                    throw new TypeMismatchException($"Не получается передать объект типа [{neww.SystemType}] в переменную типа [{cur.SystemType}]");

                Address = newAddr;
            }
        }

        /// <summary>
        /// Конструктор создания переменной некоторого типа
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public Identifier(string name, SystemTypes? type)
        {
            Literal = name;
            Address = null;
            SystemType = type;
            Type = LexemType.Identifier;
        }

        /// <summary>
        /// Конструктор создания переменной, ссылающейся на объект
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        public Identifier(string name, EvalObject obj)
        {
            Literal = name;
            Value = obj;
            SystemType = (obj != null) ? (SystemTypes?)Value.SystemType : null;
            Type = LexemType.Identifier;
        }

        /// <summary>
        /// Конструктор создания константы
        /// </summary>
        /// <param name="value"></param>
        public Identifier(EvalObject value)
        {
            Literal = value.ToString();
            Value = value;
            SystemType = Value.SystemType;
            Type = LexemType.Constant;
        }

        public override string ToString()
        {
            return Literal.ToString();
        }

        public string Serialize()
        {
            return $"{Type}\t{Literal}[{Address}]";
        }

    }

    /// <summary>
    /// Служебный символ
    /// </summary>
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

        public override string ToString()
        {
            return Literal;
        }

        public string Serialize()
        {
            return $"{Type}\t{Literal}";
        }
    }
}
