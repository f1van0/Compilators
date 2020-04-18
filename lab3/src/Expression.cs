using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
	{
	public class Expression
		{
		public State Type { get; }
		public string Code { get; }
		public int Hash { get { return GetHashCode(); } }
		public List<Expression> Children { get; }

		public Expression (string value, State type)
			{
			Code = value;
			Type = type;
			Children = new List<Expression>();
			}

		public override string ToString ()
			{
			return string.Format("[{0}] {1}", Type, Code);
			}
		public override int GetHashCode ()
			{
			return ToString().GetHashCode();
			}
		public override bool Equals (object obj)
			{
			Expression second = obj as Expression;
			if ( second?.Code == Code && second?.Type == Type )
				return true;
			return false;
			}
		}
	}
