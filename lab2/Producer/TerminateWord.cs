using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{
	/// <summary>
	/// Терминальное слово, член предложения
	/// </summary>
	abstract class TerminateWord : Word
		{
		/// <summary>
		/// Член предложения
		/// </summary>
		public Terminate SentencePart { get; protected set; }

		/// <summary>
		/// Часть речи
		/// </summary>
		public UnTerminate SpellPart { get; set; }

		protected string WordBase;

		public TerminateWord (string wordBase, Terminate sType) : base(wordBase, WordType.Terminate)
			{
			WordBase = wordBase;
			SentencePart = sType;
			}
		}

	}
