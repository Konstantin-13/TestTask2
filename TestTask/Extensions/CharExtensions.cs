namespace TestTask
{
    internal static class CharExtensions
    {
        private const string Vowel = "aeiouyAEIOUYаеёиоуыэюяАЕЁИОУЫЭЮЯ";
        
        public static bool IsVowel(this char symbol)
        {
            return Vowel.IndexOf(symbol) >= 0;
        }
    }
}