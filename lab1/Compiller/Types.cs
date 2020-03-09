using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
	{

	public enum SystemTypes
		{
		Char,
		String,
		Float,
		Int,
		Bool
		}

	public abstract class EvalObject
		{
		public static SystemTypes? Recognise (string value)
			{
			int temp_int;
			float temp_float;
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
		public abstract SystemTypes SystemType { get; }
		public object Value { get; protected set; }
		//public SystemTypes Type { get; protected set; }
		public EvalObject (object value)
			{
			Value = value;
			}
		public abstract int SizeOf ();
		public abstract dynamic GetValue ();

		#region Operators

		public virtual EvalObject Add (EvalObject obj)
			{
			if ( SystemType != obj.SystemType )
				throw new TypeMismatchException("Can't use operator + on two different types");
			var var1 = GetValue();
			var var2 = obj.GetValue();
			return Memory.CreateObjectFromValue(( var1 + var2 ).ToString());
			}

		public virtual EvalObject Subtract (EvalObject obj)
			{
			if ( SystemType != obj.SystemType )
				throw new TypeMismatchException("Can't use operator - on two different types");
			var var1 = GetValue();
			var var2 = obj.GetValue();
			return Memory.CreateObjectFromValue(( var1 - var2 ).ToString());
			}

		public virtual EvalObject Devide (EvalObject obj)
			{
			if ( SystemType != obj.SystemType )
				throw new TypeMismatchException("Can't use operator / on two different types");
			var var1 = GetValue();
			var var2 = obj.GetValue();
			return Memory.CreateObjectFromValue(( var1 / var2 ).ToString());
			}

		public virtual EvalObject Multiply (EvalObject obj)
			{
			if ( SystemType != obj.SystemType )
				throw new TypeMismatchException("Can't use operator * on two different types");
			var var1 = GetValue();
			var var2 = obj.GetValue();
			return Memory.CreateObjectFromValue(( var1 * var2 ).ToString());
			}

		public static EvalObject operator + (EvalObject obj1, EvalObject obj2)
			{
			return obj1.Add(obj2);
			}

		public static EvalObject operator - (EvalObject obj1, EvalObject obj2)
			{
			return obj1.Subtract(obj2);
			}

		public static EvalObject operator / (EvalObject obj1, EvalObject obj2)
			{
			return obj1.Devide(obj2);
			}

		public static EvalObject operator * (EvalObject obj1, EvalObject obj2)
			{
			return obj1.Multiply(obj2);
			}
		#endregion

		public override string ToString ()
			{
			return Value.ToString();
			}
		}

	public class IntObject : EvalObject
		{
		public IntObject (int value) : base(value)
			{
			Memory.Create(this);
			}

		public override SystemTypes SystemType
			{
			get
				{
				return SystemTypes.Int;
				}
			}

		public override dynamic GetValue ()
			{
			return (int)Value;
			}

		public override int SizeOf ()
			{
			return 4;
			}
		}

	public class CharObject : EvalObject
		{
		public CharObject (string value) : base(value)
			{
			Memory.Create(this);
			}

		public override SystemTypes SystemType
			{
			get
				{
				return SystemTypes.Char;
				}
			}

		public override dynamic GetValue ()
			{
			return (string)Value;
			}

		public override int SizeOf ()
			{
			return 1;
			}
		}

	public class StringObject : EvalObject
		{
		public override SystemTypes SystemType
			{
			get
				{
				return SystemTypes.String;
				}
			}

		public StringObject (string value) : base(value)
			{
			Memory.Create(this);
			}

		public override EvalObject Add (EvalObject obj)
			{
			string sstr1 = GetValue();
			string sstr2 = obj.GetValue();
			string sub1 = sstr1.Substring(1, sstr1.Length - 2);
			string sub2;
			if ( obj.SystemType == SystemTypes.Char )
				sub2 = $"{sstr2[1]}";
			else
				if ( obj.SystemType == SystemTypes.String)
				sub2 = sstr2.Substring(1, sstr2.Length - 2);
			else
				throw new TypeMismatchException();
			return (StringObject)Memory.CreateObjectFromValue($"\"{sub1}{sub2}\"");
			}

		public static StringObject operator + (StringObject str1, CharObject chr)
			{
			string sstr1 = str1.GetValue();
			string sstr2 = chr.GetValue();
			var sub1 = sstr1.Substring(1, sstr1.Length - 2);
			var sub2 = sstr2.Substring(1, 1);
			return (StringObject)Memory.CreateObjectFromValue($"\"{sub1}{sub2}\"");
			}

		public override dynamic GetValue ()
			{
			return (string)Value;
			}

		public override int SizeOf ()
			{
			return ( (string)Value ).Length;
			}
		}

	public class BoolObject : EvalObject
		{
		public override SystemTypes SystemType
			{
			get
				{
				return SystemTypes.Bool;
				}
			}

		public BoolObject (bool value) : base(value)
			{
			Memory.Create(this);
			}
		public override EvalObject Add (EvalObject obj)
			{
			if ( SystemType != obj.SystemType )
				return Memory.CreateObjectFromValue(( GetValue() | obj.GetValue() ).ToString());
			else
				throw new TypeMismatchException();
			}

		public override EvalObject Multiply (EvalObject obj)
			{
			if ( SystemType != obj.SystemType )
				return Memory.CreateObjectFromValue(( GetValue() & obj.GetValue() ).ToString());
			else
				throw new TypeMismatchException();
			}

		public override dynamic GetValue ()
			{
			return (bool)Value;
			}

		public override int SizeOf ()
			{
			return 1;
			}
		}

	public class FloatObject : EvalObject
		{
		public FloatObject (float value) : base(value)
			{
			Memory.Create(this);
			}

		public override SystemTypes SystemType
			{
			get
				{
				return SystemTypes.Float;
				}
			}

		public override dynamic GetValue ()
			{
			return (float)Value;
			}

		public override int SizeOf ()
			{
			return 4;
			}
		}

	}
