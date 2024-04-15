using System;
using System.Collections.Generic;
using System.IO;
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
        private static void Main(string[] args)
        {
            using (var inputStream1 = GetInputStream(args[0]))
            {
                var singleLetterStats = FillSingleLetterStats(inputStream1);
                RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
                PrintStatistic(singleLetterStats);
            }

            using (var inputStream2 = GetInputStream(args[1]))
            {
                var doubleLetterStats = FillDoubleLetterStats(inputStream2);
                RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);
                PrintStatistic(doubleLetterStats);
            }
            
            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы.
            Console.ReadLine();
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
            var result = new Dictionary<string, LetterStats>();

            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                try
                {
                    // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый.
                    var c = stream.ReadNextChar();

                    if (result.TryGetValue(c.ToString(), out var stats))
                    {
                        IncStatistic(ref stats);
                        result[c.ToString()] = stats;
                    }
                    else
                    {
                        result[c.ToString()] = new LetterStats { Letter = c.ToString(), Count = 1 };
                    }
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }

            return result.Select((pair, _) => pair.Value).ToList();
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
            var result = new Dictionary<string, LetterStats>();

            
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                try
                {
                    // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - НЕ регистрозависимый.
                    // NOTE : не совсем понятно что считать за пару, ниже пример
                    /*
                     * fffBBf - оригинальная строка
                     * ff fB Bf - первый вариант (текущий)
                     * ff ff fB BB Bf
                     */
                    var firstC = stream.ReadNextChar().ToString().ToUpper();
                    var secondC = stream.ReadNextChar().ToString().ToUpper();
                    
                    if (firstC != secondC) continue;
                    
                    var pair = firstC + secondC;
                
                    if (result.TryGetValue(pair, out var stats))
                    {
                        IncStatistic(ref stats);
                        result[pair] = stats;
                    }
                    else
                    {
                        result[pair] = new LetterStats { Letter = pair, Count = 1 };
                    }
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }

            return result.Select((pair, _) => pair.Value).ToList();
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
            // TODO : Удалить статистику по запрошенному типу букв.
            const string vowels = "aeiouауоыиэяюёе";

            foreach (var stats in letters.ToList())
            {
                switch (charType)
                {
                    case CharType.Consonants:
                        if (!vowels.Contains(stats.Letter.ToLower()[0])) letters.Remove(stats);
                        break;
                    case CharType.Vowel:
                        if (vowels.Contains(stats.Letter.ToLower()[0])) letters.Remove(stats);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(charType), charType, null);
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
            var total = 0;
            foreach (var stat in letters.OrderBy(stats => stats.Letter))
            {
                Console.WriteLine($"{stat.Letter}: {stat.Count}");
                total += stat.Count;
            }
            
            Console.WriteLine($"ИТОГО: {total}\n");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
