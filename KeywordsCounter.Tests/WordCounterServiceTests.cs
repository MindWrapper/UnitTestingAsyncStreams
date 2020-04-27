using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace WordCounter.Tests
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
            Assert.That(result.First().OccurrencesCount, Is.EqualTo(0));
            Assert.That(result.Last().OccurrencesCount, Is.EqualTo(0));
        }

        [Test]
        public async Task GetWordCountUpdates_NoWordsDataInStream_ReturnsExpectedWords()
        {
            SetupDataSource(s_NoData);

            var result = await GetWordCountUpdate("foo", "bar");

            Assert.That(result.Select(x => x.Word), Is.EquivalentTo(new [] {"foo", "bar"}));
        }

        [Test]
        public async Task GetWordCountUpdates_WordMatchesInStream_ReturnsExpectedUpdates()
        {
            SetupDataSource("foo");

            var result = (await GetWordCountUpdate("foo")).First();

            Assert.That(result.OccurrencesCount, Is.EqualTo(1));
;        }

        void SetupDataSource(params string[] data)
        {
            m_DataSource.Setup(x => x.GetData()).Returns(data.ToAsyncEnumerable());
        }

        async Task<List<WordCountUpdate>> GetWordCountUpdate(params string[] wordsToCount)
        {
            var result = new ConcurrentBag<WordCountUpdate>();
            await foreach (var update in m_WordCounterService.GetWordCountUpdates(wordsToCount))
            {
                result.Add(update);
            }
            return result.ToList();
        }
    }
}
