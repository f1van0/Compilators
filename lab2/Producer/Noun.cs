using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{
	/// <summary>
	/// Существительное
	/// </summary>
	class Noun : TerminateWord
		{
		protected static string[] Finalizes;
		public WordGender Gender { get; protected set; }
		static Noun ()
			{
			//Gender
			// He, She, It, They
			Finalizes = new string[4] { "", "а", "ие", "ы" };
			}

		public Noun (string wordBase, WordGender gen) : base(wordBase, Terminate.Существительное)
			{
			Gender = gen;
			}

		public string GetNoun ()
			{
			return WordBase;
			}
		}

	}
