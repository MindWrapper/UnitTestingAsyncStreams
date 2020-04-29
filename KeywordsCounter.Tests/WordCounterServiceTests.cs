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
        public async Task GetWordCountUpdates_NoWordsDataInStream_EmptyResult()
        {
            SetupDataSource(s_NoData);

            var result = await GetWordCountUpdate("foo", "bar");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetWordCountUpdates_WordMatchesInStream_ReturnsExpectedUpdates()
        {
            SetupDataSource("foo");

            var result = (await GetWordCountUpdate("foo")).First();

            Assert.That(result.OccurrencesCount, Is.EqualTo(1));
        }

        [TestCase("foo foo", "foo", 2)]
        [TestCase("foo.foo", "foo", 2)]
        [TestCase("foo;foo", "foo", 2)]
        public async Task GetWordCountUpdates_WordMatchesTwice_ReturnsExpectedUpdates(string streamContext, string word, int expectedOccurrencesCount)
        {
            SetupDataSource(streamContext);

            var result = await GetWordCountUpdate("foo", "bar");

            Assert.That(result.First().OccurrencesCount, Is.EqualTo(expectedOccurrencesCount));
        }
        
        [Test]
        public async Task GetWordCountUpdates_TwoWordMatch_ReturnsExpectedUpdates()
        {
            SetupDataSource("foo", "bar");

            var result = await GetWordCountUpdate("foo", "bar");

            result.Sort((x, y) => x.Word.CompareTo(y.Word));
            Assert.That(result.Select(x => x.Word), Is.EqualTo(new[] { "bar", "foo" }));
            Assert.That(result.Select(x => x.OccurrencesCount), Is.EqualTo(new[] { 1, 1 }));
        }

        [Test]
        public async Task GetWordCountUpdates_MultipleWordMatchOnMultipleLines_ReturnsExpectedUpdates()
        {
            SetupDataSource("foo", "bar", "foo;bar;baz foo", "baz;foo;bar");

            var result = await GetWordCountUpdate("foo", "bar");

            var sumFoo = result.Where(x => x.Word == "foo").Sum(x => x.OccurrencesCount);
            var sumBar = result.Where(x => x.Word == "bar").Sum(x => x.OccurrencesCount);
            Assert.That(sumFoo, Is.EqualTo(4));
            Assert.That(sumBar, Is.EqualTo(3));
        }

        void SetupDataSource(params string[] data)
        {
            m_DataSource.Setup(x => x.GetData()).Returns(data.ToAsyncEnumerable());
        }

        async Task<List<WordCountUpdate>> GetWordCountUpdate(params string[] wordsToCount)
        {
            var result = new List<WordCountUpdate>();
            await foreach (var update in m_WordCounterService.GetWordCountUpdates(wordsToCount))
            {
                result.Add(update);
            }
            return result.ToList();
        }
    }
}
