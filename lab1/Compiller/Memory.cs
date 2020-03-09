using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
	{



	public static class Memory
		{
		public delegate bool MemoryActions (EvalObject obj);
		public static MemoryActions Remove;
		public static MemoryActions Create;
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

		static bool CreateObj (EvalObject obj)
			{
			Objects.Add(MemAddress += (uint)obj.SizeOf(), obj);
			return true;
			}

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

		public static uint? LocateObject (string value)
			{
			foreach ( var item in Objects )
				{
				if ( item.Value.ToString() == value.ToString() )
					return item.Key;
				}
			return null;
			}

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
						throw new LexemException($"Can't recognize {value} type");
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
