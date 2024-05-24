using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTask
{
    public static class Program
    {
        private const string Vowels = "AaEeIiOoUuYyАаИиЕеЁёОоУуЫыЭэЮюЯя";

        private const string Consonants =
            "BbCcDdFfGgHhJjKkLlMmNnPpQqRrSsTtVvWwXxZzБбВвГгДдЖжЗзЙйКкЛлМмНнПпРрСсТтФфХхЦцЧчШшЩщЪъЬь";
        
        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        public static void Main(string[] args)
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

            Console.WriteLine("Нажите любую кнопку для выхода...");
            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath) 
            => new ReadOnlyStream(fileFullPath);

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();

            var statistics = new Dictionary<char, LetterStats>();
            
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                
                if (!char.IsLetter(c)) continue;

                if (!statistics.ContainsKey(c))
                {
                    statistics.Add(c, new LetterStats { Letter = c.ToString(), Count = 1 });
                }
                else
                {
                    var letterStat = statistics[c];
                    IncStatistic(ref letterStat);
                    statistics[c] = letterStat;
                }
            }

            return statistics.Values.ToList();
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
            stream.ResetPositionToStart();

            var statistics = new Dictionary<string, LetterStats>();
            char? prev = null;
            
            while (!stream.IsEof)
            {
                char curr = char.ToLower(stream.ReadNextChar());

                if (prev == curr)
                {
                    string pair = $"{prev}{curr}";

                    if (!statistics.ContainsKey(pair))
                    {
                        statistics.Add(pair, new LetterStats { Letter = pair, Count = 1 });
                    }
                    else
                    {
                        var letterStat = statistics[pair];
                        IncStatistic(ref letterStat);
                        statistics[pair] = letterStat;
                    }
                }
                else
                {
                    prev = curr;
                }
            }

            return statistics.Values.ToList();
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
            switch (charType)
            {
                case CharType.Consonants:
                {
                    for (int i = 0; i < letters.Count; i++)
                    {
                        var stat = letters[i];
                        char sym = stat.Letter[0];
                        
                        if (!Consonants.Contains(sym)) continue;
                        
                        letters.RemoveAt(i--);
                    }
                    break;
                }
                case CharType.Vowel:
                {
                    for (int i = 0; i < letters.Count; i++)
                    {
                        var stat = letters[i];
                        char sym = stat.Letter[0];
                        
                        if (!Vowels.Contains(sym)) continue;
                        
                        letters.RemoveAt(i--);
                    }
                    break;
                }
                default: throw new ArgumentOutOfRangeException(nameof(charType), charType, null);
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
            letters = letters.OrderBy(l => l.Letter);

            int total = 0;
            
            foreach (var stat in letters)
            {
                Console.WriteLine($"{stat.Letter} : {stat.Count}");
                total += stat.Count;
            }
            
            Console.WriteLine($"ИТОГО: {total}");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        /// Добавлен ref, так как структуры - значимый тип, поэтому после выхода из функции
        /// результат не сохранится
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
