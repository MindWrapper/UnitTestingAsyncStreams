using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WordCounter;

namespace KeywordsCounter.Tests
{
    [TestFixture]
    public class WordCounterServiceTests
    {
        [Test]
        public async Task GetWordCountUpdates_NoWordsToCount_ReturnsEmptyResult()
        {
            var dataSource = new Mock<IDataSource>();
            var wordsToCount = new string[] { };
            var streamData = new string[] { };
            dataSource.Setup(x => x.GetData()).Returns(streamData.ToAsyncEnumerable());
            var service = new WordCounterService(dataSource.Object);
            var result = new ConcurrentBag<KeyValuePair<string, int>>();

            await foreach (var update in service.GetWordCountUpdates(wordsToCount))
            {
                result.Add(update);
            }
           
            Assert.That(result.IsEmpty);
        }
    }
}
