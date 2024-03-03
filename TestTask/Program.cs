﻿using System;
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

            List<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            List<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
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
        private static List<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            var letterStats = new Dictionary<char, int>();

            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                if (char.IsLetter(c))
                {
                    if (!letterStats.ContainsKey(c))
                        letterStats.Add(c, 1);
                    else
                        letterStats[c]++;
                }
            }

            return letterStats.Select(kvp => new LetterStats(kvp.Key, kvp.Value)).ToList();
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static List<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            var letterStats = new Dictionary<char, LetterStats>();

            stream.ResetPositionToStart();
            char current = ' ';
            while (!stream.IsEof)
            {
                char prev = current;
                current = stream.ReadNextChar();

                if(char.IsLetter(current) && char.IsLetter(prev) && char.ToLower(current) == char.ToLower(prev)) 
                {
                    var lowerChar = char.ToLower(current);
                    LetterStats lowerCharStats;

                    if(letterStats.TryGetValue(lowerChar, out lowerCharStats))
                    {
                        lowerCharStats.IncrementCount();
                        letterStats[lowerChar] = lowerCharStats;
                    }
                    else
                    {
                        lowerCharStats = new LetterStats(lowerChar, 1);
                        letterStats.Add(lowerChar, lowerCharStats);
                    }
                }
            }

            return new List<LetterStats>(letterStats.Values);
        }

        private static char[] Vovels = new char[] { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я', 'a', 'e', 'i', 'o', 'u', 'y' };

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(List<LetterStats> letters, CharType charType)
        {
            if (letters is null)
                return;

            Func<char, bool> isVowel = x => Vovels.Contains(char.ToLower(x));

            switch (charType)
            {
                case CharType.Consonants:
                    letters.RemoveAll(x => !isVowel(x.Letter));
                    break;
                case CharType.Vowel:
                    letters.RemoveAll(x => isVowel(x.Letter));
                    break;
                default:
                    throw new ArgumentException($"Unknown CharType {charType}");
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
            int count = 0;
            if(letters != null)
            {
                foreach (var letterStat in letters.OrderBy(stat => stat.Letter))
                {
                    Console.WriteLine($"{letterStat.Letter} : {letterStat.Count}");
                    count++;
                }
            }
            Console.WriteLine($"Итого: {count}");
        }
    }
}
