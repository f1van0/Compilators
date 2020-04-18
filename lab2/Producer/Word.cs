using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Producer
	{
	/// <summary>
	/// Терминальные слова
	/// </summary>
	enum Terminate
		{
		Существительное,
		Глагол,
		Прилагательное,
		Предлог,
		Союз
		}

	/// <summary>
	/// Не терминальные слова
	/// </summary>
	enum UnTerminate
		{
		Подлежащее,
		Сказуемое,
		Перечисление,
		Дополнение
		}

	/// <summary>
	/// Род слова
	/// </summary>
	enum WordGender
		{
		He,
		She,
		It,
		They
		}

	/// <summary>
	/// Тип слова
	/// </summary>
	enum WordType
		{
		Terminate,
		Unterminate
		}

	/// <summary>
	/// Время
	/// </summary>
	enum WordTime
		{
		Past, Current, Future
		}

	/// <summary>
	/// Абстрактный член предложения
	/// </summary>
	abstract class Word
		{
		public virtual string Name { get; protected set; }
		public WordType Type { get; protected set; }

		public Word (string word, WordType type)
			{
			Name = word;
			Type = type;
			}

		public string Serialize () => $"{Type} {Name}";

		public override string ToString () => Name;

		}
	}
