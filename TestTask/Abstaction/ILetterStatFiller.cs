using System.Collections.Generic;

namespace TestTask
{
    public interface ILetterStatFiller
    {
        IList<LetterStats> FillStats(IReadOnlyStream stream);
    }
}