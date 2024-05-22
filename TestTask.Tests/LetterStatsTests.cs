using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TestTask.Tests
{
    [TestFixture]
    public sealed class LetterStatsTests
    {
        [Test, TestCase("/Volumes/SamsungSSD/Projects/Dotnet/TestTask/TestFile.txt")]
        public void Write_Single_Letter_Stats_And_Count_Should_Be_Equal_22(string filePath)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream stream = new ReadOnlyStream(filePath))
            {
                LetterStatFiller singleFiller = new SingleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(stream);
            }

            singleLetterStats.Should().NotBeNull();
            singleLetterStats.Count.Should().Be(22);
        }
        
        [Test, TestCase("/Volumes/SamsungSSD/Projects/Dotnet/TestTask/TestFile.txt")]
        public void Write_Single_Letter_Stats_With_Remove_Consonants_And_Count_Should_Be_Equal_3(string filePath)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream stream = new ReadOnlyStream(filePath))
            {
                LetterStatFiller singleFiller = new SingleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(stream);
            }
            
            singleLetterStats.RemoveCharStatsByType(CharType.Consonants);
            singleLetterStats.Count.Should().Be(3);
        }
        
        [Test, TestCase("/Volumes/SamsungSSD/Projects/Dotnet/TestTask/TestFile.txt")]
        public void Write_Single_Letter_Stats_With_Remove_Vowel_And_Count_Should_Be_Equal_19(string filePath)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream stream = new ReadOnlyStream(filePath))
            {
                LetterStatFiller singleFiller = new SingleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(stream);
            }

            singleLetterStats.RemoveCharStatsByType(CharType.Vowel);
            singleLetterStats.Count.Should().Be(19);
        }
        
        [Test, TestCase("/Volumes/SamsungSSD/Projects/Dotnet/TestTask/TestFile.txt")]
        public void Write_Double_Letter_Stats_And_Count_Should_Be_Equal_3(string filePath)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream stream = new ReadOnlyStream(filePath))
            {
                LetterStatFiller singleFiller = new DoubleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(stream);
            }

            singleLetterStats.Should().NotBeNull();
            singleLetterStats.Count.Should().Be(3);
        }
        
        [Test, TestCase("/Volumes/SamsungSSD/Projects/Dotnet/TestTask/TestFile.txt")]
        public void Write_Double_Letter_Stats_With_Remove_Consonants_And_Count_Should_Be_Equal_1(string filePath)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream stream = new ReadOnlyStream(filePath))
            {
                LetterStatFiller singleFiller = new DoubleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(stream);
            }

            singleLetterStats.RemoveCharStatsByType(CharType.Consonants);
            singleLetterStats.Count.Should().Be(1);
        }
        
        [Test, TestCase("/Volumes/SamsungSSD/Projects/Dotnet/TestTask/TestFile.txt")]
        public void Write_Double_Letter_Stats_With_Remove_Vowel_And_Count_Should_Be_Equal_2(string filePath)
        {
            IList<LetterStats> singleLetterStats;
            using (IReadOnlyStream stream = new ReadOnlyStream(filePath))
            {
                LetterStatFiller singleFiller = new DoubleLetterStatFiller();
                singleLetterStats = singleFiller.FillStats(stream);
            }

            singleLetterStats.RemoveCharStatsByType(CharType.Vowel);
            singleLetterStats.Count.Should().Be(2);
        }
    }
}