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
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
            RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

            Console.WriteLine("Single letter stats");
            PrintStatistic(singleLetterStats);
            Console.WriteLine("Double letter stats");
            PrintStatistic(doubleLetterStats);

            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы. (DONE)
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
                string substr = stream.ReadNextChar().ToString();

                if (substr == " ") { continue; }

                int index = ls.FindIndex(item => item.Letter == substr);
                if (index != -1) { IncStatistic(ls[index]); }
                else { ls.Add(new LetterStats(substr, 1)); }

                // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый. (DONE)
            }
            stream.EnsureFileDisposed();

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

            string substr_1 = "";
            while (!stream.IsEof)
            {
                string substr_2 = stream.ReadNextChar().ToString().ToUpper();

                if (substr_1 == " " || substr_1 != substr_2) { if (substr_2 != " ") { substr_1 = substr_2; } continue; }

                int index = ls.FindIndex(item => item.Letter == substr_1 + substr_2);
                if (index != -1) { IncStatistic(ls[index]); }
                else { ls.Add(new LetterStats(substr_1 + substr_2, 1)); }

                substr_1 = stream.ReadNextChar().ToString().ToUpper();

                // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - НЕ регистрозависимый. (DONE)
            }
            stream.EnsureFileDisposed();

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
            // TODO : Удалить статистику по запрошенному типу букв. (DONE)
            const string vowels = "АаЕеЁёИиОоУуЫыЭэЮюЯя";
            const string consonants = "БбВвГгДдЖжЗзЙйКкЛлМмНнПпРрСсТтФфХхЦцЧчШшЩщ";

            switch (charType)
            {
                case CharType.Vowel:
                    Compare(vowels);
                    break;
                case CharType.Consonants:
                    Compare(consonants);
                    break;
            }
            
            void Compare(string chType) // Локальная функция проверки на соответствие CharType и удаления ненужных элементов
            {
                string ch;

                for (int i = 0; i < letters.Count(); i++)
                {
                    ch = letters[i].Letter[0].ToString();
                    if (chType.Contains(ch))
                    {
                        letters.RemoveAt(i);
                        i = 0;
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
            // TODO : Выводить на экран статистику. Выводить предварительно отсортировав по алфавиту! (DONE)
            var letters_ = from i in letters
                           orderby i.Letter
                           select i;

            foreach (LetterStats i in letters_)
            {
                Console.Write(i.Letter + " : ");
                Console.WriteLine(i.Count);
            }
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
