using System.Collections.Generic;

namespace TestTask
{
    internal sealed class SingleLetterStatFiller : LetterStatFiller
    {
        public override IList<LetterStats> FillStats(IReadOnlyStream stream)
        {
            var stats = new List<LetterStats>();
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char symbol = stream.ReadNextChar();
                if (!char.IsLetter(symbol))
                {
                    continue;
                }
                
                int indexOfBySymbol = GetIndexBySymbol(symbol.ToString(), stats);
                stats.IncStatistic(indexOfBySymbol);
            }
            
            return stats;
        }
    }
}