using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
	{
	/// <summary>
	/// Симулирует память интерпретатора
	/// </summary>
	public static class Memory
		{
		public delegate bool MemoryActions (EvalObject obj);

		/// <summary>
		/// Возникает при удалении объекта в памяти
		/// </summary>
		public static MemoryActions Remove;

		/// <summary>
		/// Возникает при создании обхекта в памяти
		/// </summary>
		public static MemoryActions Create;

		/// <summary>
		/// Список объектов в памяти
		/// </summary>
		static Dictionary<uint, EvalObject> Objects = new Dictionary<uint, EvalObject>();
		public static void Clear ()
			{
			Objects.Clear();
			}

		static Memory ()
			{
			Create += CreateObj;
			Remove += RemoveObj;
			}
		static uint MemAddress = 0;

		/// <summary>
		/// Создаёт обхект в памяти
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		static bool CreateObj (EvalObject obj)
			{
			Objects.Add(MemAddress += (uint)obj.SizeOf(), obj);
			return true;
			}

		/// <summary>
		/// Удаляет объект из памяти
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		static bool RemoveObj (EvalObject obj)
			{
			var index = LocateObject(obj.ToString());
			if ( index != null )
				{
				Objects.Remove((uint)index);
				return true;
				}
			return false;
			}

		/// <summary>
		/// Определяет адрес объекта в памяти, на основании его строкового представления
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static uint? LocateObject (string value)
			{
			foreach ( var item in Objects )
				{
				if ( item.Value.ToString() == value.ToString() )
					return item.Key;
				}
			return null;
			}

		/// <summary>
		/// Создаёт объект из строкового представления
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static EvalObject CreateObjectFromValue (string value)
			{
			uint? index = LocateObject(value);
			if ( index != null )
				return Objects[(uint)index];
			else
				{
				EvalObject obj;
				try
					{
					// Попытка распознать тип объекта
					switch ( EvalObject.Recognise(value) )
						{
						case SystemTypes.Char:
						obj = new CharObject(value);
						break;
						case SystemTypes.String:
						obj = new StringObject(value);
						break;
						case SystemTypes.Float:
						obj = new FloatObject(float.Parse(value));
						break;
						case SystemTypes.Int:
						obj = new IntObject(int.Parse(value));
						break;
						case SystemTypes.Bool:
						obj = new BoolObject(( value == "True" ));
						break;
						default:
						throw new LexemException($"Не получается распознать тип у значения - {value}");
						}
					return obj;
					}
				catch ( Exception )
					{
					return null;
					}
				}
			}

		/// <summary>
		/// Пробует получить объект из памяти по адресу
		/// </summary>
		/// <param name="Address">Адрес объекта в памяти</param>
		/// <returns></returns>
		public static EvalObject GetObject (uint Address)
			{
			EvalObject obj;
			var result = Objects.TryGetValue(Address, out obj);
			if ( result )
				return obj;
			else
				return null;
			}
		}
	}
