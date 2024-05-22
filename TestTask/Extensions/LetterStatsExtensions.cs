using System;
using System.Collections.Generic;

namespace TestTask
{
    public static class LetterStatsExtensions
    {
        public static int IndexOfByLetter(this IList<LetterStats> statsList, string letter)
        {
            for (int i = statsList.Count - 1; i >= 0; i--)
            {
                LetterStats letterStats = statsList[i];
                if (letterStats.Letter == letter)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int AddLetter(this IList<LetterStats> statsList, string letter)
        {
            statsList.Add(new LetterStats
            {
                Letter = letter,
                Count = 0,
            });

            return statsList.Count - 1;
        }
        
        public static void IncStatistic(this  IList<LetterStats> statsList, int index)
        {
            LetterStats letterStats = statsList[index];
            letterStats.IncStatistic();
            statsList[index] = letterStats;
        }
        
        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        public static void RemoveCharStatsByType(this IList<LetterStats> letters, CharType charType)
        {
            for (int i = letters.Count - 1; i >= 0; i--)
            {
                LetterStats letterStats = letters[i];
                if (CanRemove(letterStats, charType))
                {
                    letters.RemoveAt(i);
                }
            }
            
            switch (charType)
            {
                case CharType.Consonants:
                    for (int i = letters.Count - 1; i >= 0; i--)
                    {
                        LetterStats letterStats = letters[i];
                        if (!letterStats.Letter[0].IsVowel())
                        {
                            letters.RemoveAt(i);
                        }
                    }
                    break;
                
                case CharType.Vowel:
                    for (int i = letters.Count - 1; i >= 0; i--)
                    {
                        LetterStats letterStats = letters[i];
                        if (letterStats.Letter[0].IsVowel())
                        {
                            letters.RemoveAt(i);
                        }
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        public static void IncStatistic(this ref LetterStats letterStats)
        {
            letterStats.Count++;
        }

        private static bool CanRemove(LetterStats stats, CharType charType)
        {
            switch (charType)
            {
                case CharType.Vowel:
                    return stats.Letter[0].IsVowel();
                case CharType.Consonants:
                    return !stats.Letter[0].IsVowel();
            }
            
            throw new ArgumentOutOfRangeException(nameof(charType), charType, null);
        }
    }
}