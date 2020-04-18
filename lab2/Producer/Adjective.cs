using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{
	/// <summary>
	/// Прилагательное
	/// </summary>
	class Adjective : TerminateWord
		{
		protected static string[] Finalizes;
		static Adjective ()
			{
			//Gender
			// He, She, It, They
			Finalizes = new string[4] { "ый", "ая", "ое", "ые" };
			}

		public Adjective (string wordBase) : base(wordBase, Terminate.Прилагательное)
			{
			}

		public string GetAdjective (WordGender gen)
			{
			return WordBase + Finalizes[(int)gen];
			}
		}
	}
