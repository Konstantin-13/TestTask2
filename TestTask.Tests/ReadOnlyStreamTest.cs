using System;
using FluentAssertions;
using NUnit.Framework;

namespace TestTask.Tests
{
    [TestFixture]
    public class ReadOnlyStreamTest
    {
        private const string PathToTestFile = "/Volumes/SamsungSSD/Projects/Dotnet/TestTask/README.md";
        
        [Test]
        public void Read_TestFile_And_Result_Should_Not_Be_Empty()
        {
            string result = string.Empty;
            using (IReadOnlyStream stream = new ReadOnlyStream(PathToTestFile))
            {
                stream.ResetPositionToStart();
                while (!stream.IsEof)
                {
                    result += stream.ReadNextChar();
                }
            }

            result.Should().NotBeEmpty();
        }

        [Test]
        public void After_Dispose_Stream_Should_Throw_Exception()
        {
            IReadOnlyStream stream = new ReadOnlyStream(PathToTestFile);
            stream.Dispose();
            Assert.Throws<ObjectDisposedException>(() => stream.ReadNextChar());
        }
    }
}