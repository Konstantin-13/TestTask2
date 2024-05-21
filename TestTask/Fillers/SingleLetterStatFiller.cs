using System.Collections.Generic;

namespace TestTask
{
    internal sealed class SingleLetterStatFiller : LetterStatFiller
    {
        protected override int IndexOfBySymbol(char symbol, IReadOnlyList<LetterStats> statsList)
        {
            for (int i = statsList.Count - 1; i >= 0; i--)
            {
                LetterStats letterStats = statsList[i];
                if (letterStats.Letter[0] == symbol)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}