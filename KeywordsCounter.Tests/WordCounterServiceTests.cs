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
        Mock<IDataSource> m_DataSource;
        WordCounterService m_WordCounterService;
        static string[] s_NoData = new string[] { };

        [SetUp]
        public void BeforeEachTest()
        {
            m_DataSource = new Mock<IDataSource>();
            m_WordCounterService = new WordCounterService(m_DataSource.Object);
            m_WordCounterService = new WordCounterService(m_DataSource.Object);
        }

        [Test]
        public async Task GetWordCountUpdates_NoWordsToCount_ReturnsEmptyResult()
        {
            SetupDataSource(s_NoData);
           
            var result = await GetWordCountUpdate(s_NoData);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetWordCountUpdates_NoWordsDataInStream_ReturnsZeroForEachWordToCount()
        {
            SetupDataSource(s_NoData);

            var result = await GetWordCountUpdate("foo", "bar");

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Value, Is.EqualTo(0));
            Assert.That(result.Last().Value, Is.EqualTo(0));
        }

        void SetupDataSource(params string[] data)
        {
            m_DataSource.Setup(x => x.GetData()).Returns(data.ToAsyncEnumerable());
        }

        async Task<List<KeyValuePair<string, int>>> GetWordCountUpdate(params string[] wordsToCount)
        {
            var result = new ConcurrentBag<KeyValuePair<string, int>>();
            await foreach (var update in m_WordCounterService.GetWordCountUpdates(wordsToCount))
            {
                result.Add(update);
            }
            return result.ToList();
        }
    }
}
