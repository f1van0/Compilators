using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Compiller
	{
	public static class IdentifierManager
		{

		public static void Clear ()
			{
			Identifiers.Clear();
			}

		static List<Identifier> Identifiers = new List<Identifier>();

		public static void ReAssign (ref Identifier id, EvalObject obj)
			{
			if ( Identifiers.Contains(id) )
				{
				int index = Identifiers.IndexOf(id);
				Identifiers[index].Value = obj;
				id = Identifiers[index];
				}
			else
				{
				id.Value = obj;
				Identifiers.Add(id);
				}
			}

		public static void Refresh (ref Identifier id)
			{
			if ( Identifiers.Contains(id) )
				{
				int index = Identifiers.IndexOf(id);
				id = Identifiers[index];
				}
			}

		public static Identifier ReferrenceIdentifier(string value, SystemTypes? type = null, EvalObject obj = null)
			{
			Identifier item = ( obj == null ) ? 
				item = new Identifier(value, type) : 
				item = new Identifier(value, obj);

			for ( int i = 0; i < Identifiers.Count; i++ )
				if ( Identifiers[i].Literal == item.Literal )
					{
					if ( item.Value != null )
						Identifiers[i].Value = item.Value;
					return Identifiers[i];
					}
			Identifiers.Add(item);
			return item;
			}
		}
	}
