using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{
	/// <summary>
	/// Глагол
	/// </summary>
	class Verb : TerminateWord
		{
		protected static string[,] Finalizes;
		static Verb ()
			{
			// He, She, It, They
			// Past, Present, Future
			Finalizes = new string[4, 3] { { "ал", "ает", "ает" }, { "ала", "ает", "ает" }, { "ало", "ает", "ает" }, { "ли", "ют", "ают" } };
			}
		public Verb (string wordBase) : base(wordBase, Terminate.Глагол)
			{

			}

		public string GetVerb (WordGender gen, WordTime time)
			{
			return ( time == WordTime.Future ) ? "с" : "" + WordBase + Finalizes[(int)gen, (int)time];
			}

		}

	}
