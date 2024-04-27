namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public class LetterStats // Изменено struct -> class
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count { get; set; }

        public LetterStats(string letter, int count) // Добавлен конструктор
        {
            this.Letter = letter;
            this.Count = count;
        }
    }
}
