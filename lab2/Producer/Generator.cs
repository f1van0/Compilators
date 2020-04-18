using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{
	class Pretext : TerminateWord
		{
		public Pretext (string word) : base(word, Terminate.Предлог)
			{
			}
		}

	class Union : TerminateWord
		{
		public Union (string word) : base(word, Terminate.Союз)
			{
			}
		}

	/// <summary>
	/// Генератор предложения
	/// </summary>
	static class Generator
		{
		//Члены предложения
		static List<Adjective> Adjectives;
		static List<Noun> Nouns;
		static List<Verb> Verbs;
		static List<Pretext> Pretexts;
		static List<Union> AndUnions;
		static List<Union> ButUnions;
		static Union Comma = new Union(",");
		static Union Dot = new Union(".");
		
		//Части речи
		static UnTerminateWord Сказуемое = new UnTerminateWord("Сказуемое", UnTerminate.Сказуемое);
		static UnTerminateWord Подлежащее = new UnTerminateWord("Подлежащее", UnTerminate.Подлежащее);
		static UnTerminateWord Дополнение = new UnTerminateWord("Дополнение", UnTerminate.Дополнение);
		static UnTerminateWord ПеречислениеСказ = new UnTerminateWord("Перечисление Сказуемых", UnTerminate.Перечисление);
		static UnTerminateWord ПеречислениеПодлеж = new UnTerminateWord("Перечисление Подлежащих", UnTerminate.Перечисление);

		static System.Security.Cryptography.RandomNumberGenerator gen = 
			System.Security.Cryptography.RandomNumberGenerator.Create();

		/// <summary>
		/// Получает случайное целое
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		static int Randomize (int from, int to)
			{
			byte[] arr = new byte[4];
			gen.GetNonZeroBytes(arr);
			var value = Math.Abs(BitConverter.ToInt32(arr, 0)) / ((float)int.MaxValue +1);
			return (int)(value*(to - from) + from);
			}


		/// <summary>
		/// Граф перехода состояний
		/// </summary>
		static ILookup<UnTerminateWord, Func<List<Word>>> StateGraph;

		static Generator ()
			{
			Adjectives = new List<Adjective>();
			Adjectives.Add(new Adjective("хорош"));
			Adjectives.Add(new Adjective("нормальн"));
			Adjectives.Add(new Adjective("плох"));
			Adjectives.Add(new Adjective("красив"));
			Adjectives.Add(new Adjective("быстр"));
			Adjectives.Add(new Adjective("умн"));
			Adjectives.Add(new Adjective("красн"));
			Adjectives.Add(new Adjective("добр"));
			Adjectives.Add(new Adjective("смешн"));

			Verbs = new List<Verb>();
			Verbs.Add(new Verb("дел"));
			Verbs.Add(new Verb("бег"));
			Verbs.Add(new Verb("смотр"));
			Verbs.Add(new Verb("готов"));
			Verbs.Add(new Verb("прыг"));
			Verbs.Add(new Verb("мяук"));
			Verbs.Add(new Verb("игр"));

			Nouns = new List<Noun>();
			Nouns.Add(new Noun("телевизор", WordGender.He));
			Nouns.Add(new Noun("работник", WordGender.He));
			Nouns.Add(new Noun("кошка", WordGender.She));
			Nouns.Add(new Noun("хозяйка", WordGender.She));
			Nouns.Add(new Noun("мороженое", WordGender.It));
			Nouns.Add(new Noun("событие", WordGender.It));
			Nouns.Add(new Noun("телевизоры", WordGender.They));
			Nouns.Add(new Noun("работники", WordGender.They));
			Nouns.Add(new Noun("кошки", WordGender.They));
			Nouns.Add(new Noun("хозяйка", WordGender.They));

			Pretexts = new List<Pretext>();
			Pretexts.Add(new Pretext("по"));
			Pretexts.Add(new Pretext("к"));
			Pretexts.Add(new Pretext("на"));

			AndUnions = new List<Union>();
			ButUnions = new List<Union>();
			AndUnions.Add(new Union("и"));
			ButUnions.Add(new Union("а"));

			// Подлежащее => Существительное
			// Подлежащее => Перечисление подлежащих
			// Подлежащее => Подлежащее
			//
			// Сказуемое => Глагол
			// Сказуемое => Глагол Прилагательное
			// Сказуемое => Прилагательное Глагол
			// Сказуемое => Прилагательное Перечисление Глаголов
			// Сказуемое => Сказуемое
			//
			// Перечисление Сказуемых => Глагол(2, 3)
			// Перечисление Сказуемых => Перечисление Сказуемых
			//
			// Перечисление Подлежащих => Подлежащее(2, 3)
			// Перечисление Подлежащих => Перечисление Подлежащих
			//
			// Дополнение => Предлог Существительное
			// Дополнение => Предлог Перечисление подлежащих
			// Дополнение => Дополнение

			List<KeyValuePair<UnTerminateWord, Func<List<Word>>>> Graph = new List<KeyValuePair<UnTerminateWord, Func<List<Word>>>>();
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Подлежащее, () => new List<Word> { GenerateNoun() }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Подлежащее, () => new List<Word> { ПеречислениеПодлеж }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Подлежащее, () => new List<Word> { Подлежащее }));

			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Сказуемое, () => new List<Word> { GenerateVerb() }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Сказуемое, () => new List<Word> { GenerateVerb(), GenerateAdjective() }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Сказуемое, () => new List<Word> { GenerateAdjective(), GenerateVerb() }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Сказуемое, () => new List<Word> { GenerateAdjective(), ПеречислениеСказ }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Сказуемое, () => new List<Word> { Сказуемое }));

			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(ПеречислениеСказ, () => GenerateVerbEnum()));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(ПеречислениеСказ, () => new List<Word> { ПеречислениеСказ }));

			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(ПеречислениеПодлеж, () => GenerateNounEnum()));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(ПеречислениеПодлеж, () => new List<Word> { ПеречислениеПодлеж }));

			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Дополнение, () => new List<Word> { GeneratePretext(), GenerateNoun() }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Дополнение, () => new List<Word> { GeneratePretext(), ПеречислениеПодлеж }));
			Graph.Add(new KeyValuePair<UnTerminateWord, Func<List<Word>>>(Дополнение, () => new List<Word> { Дополнение }));

			StateGraph = Graph.ToLookup(pair => pair.Key, pair => pair.Value);
			}

		static Noun GenerateNoun ()
			{
			int index = Randomize(0, Nouns.Count - 1);
			return Nouns[index];
			}

		static Verb GenerateVerb ()
			{
			int index = Randomize(0, Verbs.Count - 1);
			return Verbs[index];
			}

		static Adjective GenerateAdjective ()
			{
			int index = Randomize(0, Adjectives.Count - 1);
			return Adjectives[index];
			}

		static Pretext GeneratePretext ()
			{
			int index = Randomize(0, Pretexts.Count - 1);
			return Pretexts[index];
			}

		static List<Word> GenerateVerbEnum ()
			{
			int count = Randomize(2, 3);
			bool exst;
			int index;
			Word word = Dot;
			List<Word> words = new List<Word>();
			for ( int i = 1; i <= count; i++ )
				{
				exst = true;
				while ( exst )
					{
					index = Randomize(0, Verbs.Count-1);
					word = Verbs[index];
					exst = words.Contains(word);
					}
				words.Add(word);
				if ( count - i > 1 )
					words.Add(Comma);
				else if (i != count)
					words.Add(AndUnions[0]);
				}
			return words;
			}

		static List<Word> GenerateNounEnum ()
			{
			int count = Randomize(2, 3);
			bool exst;
			int index;
			Word word = Dot;
			List<Word> words = new List<Word>();
			for ( int i = 1; i <= count; i++ )
				{
				exst = true;
				while ( exst )
					{
					index = Randomize(0, Nouns.Count-1);
					word = Nouns[index];
					exst = words.Contains(word);
					}
				words.Add(word);
				if ( count - i > 1 )
					words.Add(Comma);
				else if ( i != count )
					words.Add(AndUnions[0]);
				}
			return words;
			}

		static List<Word> GenerateAdjEnum ()
			{
			int count = Randomize(2, 3);
			bool exst;
			int index;
			Word word = Dot;
			List<Word> words = new List<Word>();
			for ( int i = 1; i <= count; i++ )
				{
				exst = true;
				while ( exst )
					{
					index = Randomize(0, Adjectives.Count-1);
					word = Adjectives[index];
					exst = words.Contains(word);
					}
				words.Add(word);
				if ( count - i > 1 )
					words.Add(Comma);
				else if ( i != count )
					words.Add(AndUnions[0]);
				}
			return words;
			}

		/// <summary>
		/// Получает части речи
		/// </summary>
		/// <param name="parts"></param>
		/// <returns></returns>
		static List<Word> GetUnterminates(string[] parts)
			{
			List<Word> Sentence = new List<Word>();
			UnTerminate[] words;
			try
				{
				words = parts.Select<string, UnTerminate>(
					p => (UnTerminate)Enum.Parse(UnTerminate.Дополнение.GetType(), p)).ToArray();
				}
			catch ( ArgumentException )
				{
				throw new Exception("Parse error");
				}

			foreach ( var word in words )
				{
				switch ( word )
					{
					case UnTerminate.Подлежащее:
						Sentence.Add(Подлежащее);
					break;
					case UnTerminate.Сказуемое:
						Sentence.Add(Сказуемое);
					break;
					case UnTerminate.Дополнение:
						Sentence.Add(Дополнение);
					break;
					default:
					throw new Exception("Generator Error");
					}
				}
			return Sentence;

			}

		/// <summary>
		/// Переходит по случайному пути на графе
		/// </summary>
		/// <param name="word">Начальное состояние</param>
		/// <returns>Конечное состояние</returns>
		static List<Word> SelectPath(UnTerminateWord word)
			{
			var paths = StateGraph.Where(group => group.Key == word)
				.SelectMany(group => group).ToList();
			int index = Randomize(0, paths.Count()-1);
			var val = paths[index]();
			if ( val.Count == 0 )
				throw new Exception("Graph Error");
			else
				return val;
			}

		/// <summary>
		/// Генерирует предложение
		/// </summary>
		/// <param name="pattern">Структура предложения</param>
		/// <returns>Предложение</returns>
		public static string Generate(string pattern)
			{
			List<Word> Unterminates = GetUnterminates(pattern.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries));
			bool solved=false;
			List<Word> Sentence = Unterminates;
			while ( !solved )
				{
				solved = true;
				List<Word> newSentence = new List<Word>();
				newSentence.Add(Dot);
				for ( int i = Sentence.Count-1; i >= 0; i-- )
					if (Sentence[i].Type == WordType.Unterminate )
						{
						solved = false;
						var subwords = SelectPath(Sentence[i] as UnTerminateWord);
						for ( int k = 0; k < subwords.Count; k++ )
							if ( subwords[k].Type == WordType.Terminate )
								( (TerminateWord)subwords[k] ).SpellPart = ( (UnTerminateWord)Sentence[i] ).SpellPart;

						newSentence.InsertRange(0, subwords);
						}
					else if ( Sentence[i] != Dot)
						newSentence.Insert(0, Sentence[i]);

				if (!solved)
					Sentence = newSentence;
				}

			var TSentence = Sentence.Select(word => word as TerminateWord).ToList();
			StringBuilder builder = new StringBuilder();
			WordTime time = (WordTime)Randomize(0, 2);
			for ( int i = 0; i < TSentence.Count; i++ )
				{
				TerminateWord tword = TSentence[i];
				string wordStr = "";
				switch ( tword.SentencePart )
					{
					case Terminate.Существительное:
						wordStr = ( tword as Noun ).GetNoun();
					break;
					case Terminate.Глагол:
						{
						var verb = ( tword as Verb );
						var noun = TSentence.First(word => word.SentencePart == Terminate.Существительное);
						WordGender gen = ( noun as Noun ).Gender;
						gen = ( noun.SpellPart == UnTerminate.Перечисление ) ? WordGender.They : gen;
						wordStr = verb.GetVerb(gen, time);
						break;
						}
					case Terminate.Прилагательное:
						{
						var adj = ( tword as Adjective );
						var noun = TSentence.First(word => word.SentencePart == Terminate.Существительное);
						WordGender gen = ( noun as Noun ).Gender;
						gen = ( noun.SpellPart == UnTerminate.Перечисление ) ? WordGender.They : gen;
						wordStr = adj.GetAdjective(gen);
						break;
						}
					case Terminate.Предлог:
						{
						wordStr = ( tword as Pretext ).Name;
						break;
						}
					case Terminate.Союз:
						{
						wordStr = ( tword as Union ).Name;
						break;
						}
					default:
					break;
					}
				wordStr = ( i > 0 ) ? wordStr : wordStr.First().ToString().ToUpper() + wordStr.Substring(1);
				wordStr = ( tword == Dot || tword == Comma || i==0 ) ? wordStr : " " + wordStr;
				builder.Append(wordStr);
				}
			return builder.ToString();
			}
		}
	}
