using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace WordCounter.Tests
{
    [TestFixture(TestOf = typeof(WordCounterService))]
    public class ProgressTests
    {
        [Test]
        public async Task GetWordCountUpdates_ReportsProgressForEachElementInStream()
        {
            var dataSource = new Mock<IDataSource>();
            static async IAsyncEnumerable<string> StreamData()
            {
                yield return "foo";
                yield return "bar";
                yield return "baz";
                await Task.CompletedTask;
            }
            dataSource.Setup(x => x.GetData()).Returns(StreamData);
            var service = new WordCounterService(dataSource.Object);
            var progressMock = new Mock<IProgress<int>>();

            await foreach (var unused in service.GetWordCountUpdates(new[] { "foo" }, CancellationToken.None, progressMock.Object))
            {
                // do nothing
            }

            progressMock.Verify(x => x.Report(1), Times.Exactly(3));
        }

        // exercise: if task is cancelled before reading of first element - progress is never invoked.
        // exercise: progress is reported for each 10 elements in the stream
    }
}
