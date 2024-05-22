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
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream inputStream1 = GetInputStream(args[0]))
            {
                LetterStatFiller singleFiller = new SingleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(inputStream1);
            }
            
            IList<LetterStats> doubleLetterStats;
            using (IReadOnlyStream inputStream2 = GetInputStream(args[1]))
            {
                LetterStatFiller doubleFiller = new DoubleLetterStatFiller();
                doubleLetterStats = doubleFiller.FillStats(inputStream2);
            }
            
            singleLetterStats.RemoveCharStatsByType(CharType.Vowel);
            doubleLetterStats.RemoveCharStatsByType(CharType.Consonants);

            PrintStatistic(singleLetterStats);
            PrintStatistic(doubleLetterStats);

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
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            var sorted = new List<LetterStats>(letters);
            sorted.Sort((x, y) => String.Compare(x.Letter, y.Letter, StringComparison.Ordinal));
            Console.WriteLine("ИТОГО:");
            foreach (var letter in sorted)
            {
                Console.WriteLine($"{letter.Letter} : {letter.Count}");
            }
        }
    }
}
