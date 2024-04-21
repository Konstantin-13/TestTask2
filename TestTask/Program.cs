using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTask
{
	public class Program
	{
		/// <summary>
		/// Программа принимает на входе 2 пути до файлов.
		/// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
		/// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
		/// По окончанию работы - выводит данную статистику на экран.
		/// </summary>
		/// <param name="args">Первый параметр - путь до первого файла.
		/// Второй параметр - путь до второго файла.</param>
		static void Main(string[] args)
		{
			HandleStream(GetInputStream(args[0]), GetSingleLetter, CharType.Vowel);
			HandleStream(GetInputStream(args[1]), GetDoubleLetter, CharType.Consonants);

			Console.ReadLine();
		}

		private static void HandleStream(ReadOnlyStream stream, Func<ReadOnlyStream, string> analyseLetter, CharType removeCharType)
		{
			var letterStats = FillLetterStats(stream, analyseLetter);

			letterStats = RemoveCharStatsByType(letterStats, removeCharType);

			PrintStatistic(letterStats);
		}

		/// <summary>
		/// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
		/// </summary>
		/// <param name="fileFullPath">Полный путь до файла для чтения</param>
		/// <returns>Поток для последующего чтения.</returns>
		private static ReadOnlyStream GetInputStream(string fileFullPath)
		{
			return new ReadOnlyStream(fileFullPath);
		}

		/// <summary>
		/// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения символов
		/// в зависимости от выбраноого алгоритма анализа
		/// </summary>
		/// <param name="stream">Стрим для считывания символов для последующего анализа</param>
		/// <param name="analyseLetter">Алгоритм анализа</param>>
		/// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
		private static Dictionary<string, int> FillLetterStats(ReadOnlyStream stream, Func<ReadOnlyStream, string> analyseLetter)
		{
			stream.ResetPositionToStart();

			var letterStats = new Dictionary<string, int>();

			while (!stream.IsEof)
			{
				var letter = analyseLetter(stream);

				if (letter == string.Empty)
				{
					continue;
				}

				if (letterStats.ContainsKey(letter))
				{
					IncStatistic(letterStats, letter);
				}
				else
				{
					letterStats.Add(letter, 1);
				}
			}

			return letterStats;
		}

		/// <summary>
		/// Ф-ция считывающая из входящего потока букву, и возвращающая её.
		/// Статистика РЕГИСТРОЗАВИСИМАЯ!
		/// </summary>
		/// <param name="stream">Стрим для считывания символов</param>
		/// <returns>Буква</returns>
		private static string GetSingleLetter(ReadOnlyStream stream)
		{
			return stream.ReadNextChar().ToString();
		}

		/// <summary>
		/// Ф-ция считывающая из входящего потока буквы, и возвращающая пару одинаковых букв,
		/// например АА, СС, УУ, ЕЕ и т.д.
		/// Статистика - НЕ регистрозависимая!
		/// </summary>
		/// <param name="stream">Стрим для считывания символов</param>
		/// <returns>Пара букв</returns>
		private static string GetDoubleLetter(ReadOnlyStream stream)
		{
			return FindDoubleLetter();

			string FindDoubleLetter(string previousLetter = "")
			{
				if (stream.IsEof)
				{
					return string.Empty;
				}

				var letter = stream.ReadNextChar().ToString();

				if (string.Equals(previousLetter, letter, StringComparison.CurrentCultureIgnoreCase))
				{
					return (previousLetter + letter).ToLower();
				}

				return FindDoubleLetter(letter);
			}
		}

		/// <summary>
		/// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
		/// (Тип букв для перебора определяется параметром charType)
		/// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
		/// </summary>
		/// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
		/// <param name="charType">Тип букв для анализа</param>
		private static Dictionary<string, int> RemoveCharStatsByType(Dictionary<string, int> letters, CharType charType)
		{
			IReadOnlyCollection<char> charTypeCollection;

			switch (charType)
			{
				case CharType.Consonants:
					charTypeCollection = CharTypeCollection.Consonants;

					break;
				case CharType.Vowel:
					charTypeCollection = CharTypeCollection.Vowels;

					break;
				default:
					throw new ArgumentException();
			}

			return letters
				.Where(kv => kv.Key.All(c => charTypeCollection.Contains(Char.ToLower(c))))
				.ToDictionary(kv => kv.Key, kv => kv.Value);
		}

		/// <summary>
		/// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
		/// Каждая буква - с новой строки.
		/// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
		/// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
		/// </summary>
		/// <param name="letterStats">Коллекция со статистикой</param>
		private static void PrintStatistic(IReadOnlyDictionary<string, int> letterStats)
		{
			var resultStats = letterStats.OrderBy(pair => pair.Key);

			foreach (var pair in resultStats)
			{
				Console.WriteLine($"{pair.Key} : {pair.Value}");
			}

			Console.WriteLine($"ИТОГО = {resultStats.Count()}");
		}

		/// <summary>
		/// Метод увеличивает счётчик вхождений по переданному ключу.
		/// </summary>
		/// <param name="letterStats">Словарь со статистикой вхождений букв</param>
		/// <param name="key">Ключ, значение которого нужно увеличить</param>>
		private static void IncStatistic(Dictionary<string, int> letterStats, string key)
		{
			letterStats[key]++;
		}
	}
}