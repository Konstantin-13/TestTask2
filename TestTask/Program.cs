using System;
using System.Collections.Generic;

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
        /// <param name="args">/Users/kamranimranli/desktop/testtask/testtask/sample.txt
        /// /Users/kamranimranli/desktop/testtask/testtask/sample.txt</param>
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide paths for two files.");
                return;
            }

            try
            {
                IReadOnlyStream inputStream1 = GetInputStream(args[0]);
                IReadOnlyStream inputStream2 = GetInputStream(args[1]);

                IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
                IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

                RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
                RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

                PrintStatistic(singleLetterStats);
                PrintStatistic(doubleLetterStats);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы.
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            var stats = new Dictionary<char, LetterStats>();
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                if (char.IsLetter(c))
                {
                    if (!stats.ContainsKey(c))
                    {
                        stats[c] = new LetterStats { Letter = c.ToString(), Count = 0 };
                    }

                    IncStatistic(stats[c]);
                }
            }

            return new List<LetterStats>(stats.Values);
        }


        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            var stats = new Dictionary<string, LetterStats>();

            stream.ResetPositionToStart();
            char? previousChar = null;

            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                if (char.IsLetter(c))
                {
                    c = char.ToLower(c);

                    if (previousChar.HasValue && previousChar.Value == c)
                    {
                        string pair = new string(new[] { previousChar.Value, c });
                        if (!stats.ContainsKey(pair))
                        {
                            stats[pair] = new LetterStats { Letter = pair, Count = 0 };
                        }
                        IncStatistic(stats[pair]);
                    }

                    previousChar = c;
                }
                else
                {
                    previousChar = null;
                }
            }

            return new List<LetterStats>(stats.Values);
        }


        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
            var consonants = new HashSet<char>("bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщ");

            // TODO : Удалить статистику по запрошенному типу букв.

            bool ShouldRemove(char c) =>
                (charType == CharType.Vowel && vowels.Contains(char.ToLower(c))) ||
                (charType == CharType.Consonants && consonants.Contains(char.ToLower(c)));

            for (int i = letters.Count - 1; i >= 0; i--)
            {
                var letter = letters[i].Letter;
                if (letter.Length == 1 && ShouldRemove(letter[0]))
                {
                    letters.RemoveAt(i);
                }
                else if (letter.Length == 2 && ShouldRemove(letter[0]) && ShouldRemove(letter[1]))
                {   
                    letters.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            // TODO : Выводить на экран статистику. Выводить предварительно отсортировав по алфавиту!
            var sortedLetters = new List<LetterStats>(letters);
            sortedLetters.Sort((x, y) => string.Compare(x.Letter, y.Letter, StringComparison.Ordinal));

            int total = 0;
            foreach (var letter in sortedLetters)
            {
                Console.WriteLine($"{letter.Letter} : {letter.Count}");
                total += letter.Count;
            }
            Console.WriteLine($"ИТОГО: {total}");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
