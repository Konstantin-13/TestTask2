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
            IReadOnlyStream inputStream1 = GetInputStream(args[0]);
            IReadOnlyStream inputStream2 = GetInputStream(args[1]);

            IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            inputStream1.Dispose();
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);
            inputStream2.Dispose();

            RemoveCharStatsByType(singleLetterStats, CharType.Vowels);
            RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

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
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            var ls = new List<LetterStats>();

            while (!stream.IsEof)
            {
                string ch = stream.ReadNextChar().ToString();

                if (ch == " ") { continue; }

                int index = ls.FindIndex(item => item.Letter == ch);
                if (index != -1) { IncStatistic(ls[index]); }
                else { ls.Add(new LetterStats(ch, 1)); }
            }

            return ls;
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
            var ls = new List<LetterStats>();

            string ch_1 = "";
            while (!stream.IsEof)
            {
                string ch_2 = stream.ReadNextChar().ToString().ToUpper();

                if (ch_1 == " " || ch_1 != ch_2)
                {
                    if (ch_2 != " ")
                    {
                        ch_1 = ch_2;
                    }
                    continue;
                }

                int index = ls.FindIndex(item => item.Letter == ch_1 + ch_2);
                if (index != -1) { IncStatistic(ls[index]); }
                else { ls.Add(new LetterStats(ch_1 + ch_2, 1)); }

                ch_1 = stream.ReadNextChar().ToString().ToUpper();
            }

            return ls;
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
            const string vowels = "АаЕеЁёИиОоУуЫыЭэЮюЯя";
            const string consonants = "БбВвГгДдЖжЗзЙйКкЛлМмНнПпРрСсТтФфХхЦцЧчШшЩщ";

            switch (charType)
            {
                case CharType.Vowels:
                    RemoveMatches(vowels);
                    break;
                case CharType.Consonants:
                    RemoveMatches(consonants);
                    break;
            }
            
            void RemoveMatches(string chars)
            {
                for (int i = 0; i < letters.Count(); ++i)
                {
                    if (chars.Contains(letters[i].Letter[0].ToString()))
                    {
                        letters.RemoveAt(i);
                        i--;
                    }
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
        private static void PrintStatistic(IList<LetterStats> letters)
        {
            var lettersOrdered = from i in letters
                           orderby i.Letter
                           select i;

            int letters_ttl = 0;
            int count_ttl = 0;
            foreach (var i in lettersOrdered)
            {
                Console.Write(i.Letter + " : ");
                Console.WriteLine(i.Count);

                letters_ttl++;
                count_ttl += i.Count;
            }

            Console.WriteLine("ИТОГО : " + letters_ttl + " значений / " + count_ttl + " вхождений");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданному экземпляру.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
