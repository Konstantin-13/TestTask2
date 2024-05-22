using System.Collections.Generic;

namespace TestTask
{
    internal sealed class DoubleLetterStatFiller : LetterStatFiller
    {
        public override IList<LetterStats> FillStats(IReadOnlyStream stream)
        {
            var stats = new List<LetterStats>();
            stream.ResetPositionToStart();
            var chars = new char[2];
            int symbolIndex = 0;
            while (!stream.IsEof)
            {
                char symbol = char.ToUpperInvariant(stream.ReadNextChar());
                if (!char.IsLetter(symbol))
                {
                    continue;
                }

                chars[symbolIndex++] = symbol;
                if (symbolIndex >= chars.Length)
                {
                    if (TryWriteStat(chars, stats))
                    {
                        symbolIndex = 0;
                    }
                    else
                    {
                        chars[0] = chars[1];
                        symbolIndex = 1;
                    }
                }
            }
            
            return stats;
        }

        private bool TryWriteStat(char[] chars, IList<LetterStats> stats)
        {
            bool canWrite = chars[0].Equals(chars[1]);
            if (canWrite)
            {
                string letter = new string(chars);
                int indexBySymbol = GetIndexBySymbol(letter, stats);
                LetterStats letterStats = stats[indexBySymbol];
                letterStats.IncStatistic();
                stats[indexBySymbol] = letterStats;
            }
            
            return canWrite;
        }
    }
}