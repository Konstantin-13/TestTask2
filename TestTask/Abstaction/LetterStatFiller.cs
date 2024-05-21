using System.Collections.Generic;

namespace TestTask
{
    internal abstract class LetterStatFiller : ILetterStatFiller
    {
        public IList<LetterStats> FillStats(IReadOnlyStream stream)
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
                
                int indexOfBySymbol = IndexOfBySymbol(symbol, stats);
                indexOfBySymbol = ValidateIndex(indexOfBySymbol, symbol, stats);
                LetterStats letterStats = stats[indexOfBySymbol];
                IncStatistic(ref letterStats);
                stats[indexOfBySymbol] = letterStats;
            }
            
            return stats;
        }

        protected abstract int IndexOfBySymbol(char symbol, IReadOnlyList<LetterStats> statsList);
        
        private int ValidateIndex(int indexOfBySymbol, char symbol, List<LetterStats> stats)
        {
            if (indexOfBySymbol < 0)
            {
                stats.Add(new LetterStats
                {
                    Letter = symbol.ToString(),
                    Count = 0,
                });

                indexOfBySymbol = stats.Count - 1;
            }

            return indexOfBySymbol;
        }
        
        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}