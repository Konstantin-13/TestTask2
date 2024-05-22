using System.Collections.Generic;

namespace TestTask
{
    public abstract class LetterStatFiller
    {
        public abstract IList<LetterStats> FillStats(IReadOnlyStream stream);
        
        protected static int GetIndexBySymbol(string letter, IList<LetterStats> stats)
        {
            int indexOfBySymbol = stats.IndexOfByLetter(letter);
            if (indexOfBySymbol < 0)
            {
                indexOfBySymbol = stats.AddLetter(letter);
            }

            return indexOfBySymbol;
        }
    }
}