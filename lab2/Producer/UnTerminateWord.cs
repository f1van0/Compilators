using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{

	class UnTerminateWord : Word
		{
		/// <summary>
		/// Часть речи
		/// </summary>
		public UnTerminate SpellPart { get; protected set; }

		public UnTerminateWord (string word, UnTerminate sType) : base(word, WordType.Unterminate)
			{
			SpellPart = sType;
			}


		}
	}
