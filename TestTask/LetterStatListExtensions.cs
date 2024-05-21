using System.Collections.Generic;

namespace TestTask
{
    public static class LetterStatListExtensions
    {
        private const string Vowel = "aeiouyAEIOUY";
        
        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        public static void RemoveCharStatsByType(this IList<LetterStats> letters, CharType charType)
        {
            switch (charType)
            {
                case CharType.Consonants:
                    for (int i = letters.Count - 1; i >= 0; i--)
                    {
                        LetterStats letterStats = letters[i];
                        if (Vowel.IndexOf(letterStats.Letter[0]) < 0)
                        {
                            letters.RemoveAt(i);
                        }
                    }
                    break;
                
                case CharType.Vowel:
                    for (int i = letters.Count - 1; i >= 0; i--)
                    {
                        LetterStats letterStats = letters[i];
                        if (Vowel.IndexOf(letterStats.Letter[0]) >= 0)
                        {
                            letters.RemoveAt(i);
                        }
                    }
                    break;
            }
        }
    }
}