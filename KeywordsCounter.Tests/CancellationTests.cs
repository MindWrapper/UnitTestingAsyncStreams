using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WordCounter;

namespace KeywordsCounter.Tests
{
    public class CancellationTests
    {
        [Test]
        public void GetWordCountUpdates_CanBeCancelledAfterAnItemInStream()
        {
            var dataSource = new Mock<IDataSource>();
            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;
            async IAsyncEnumerable<string> StreamData()
            {
                yield return "foo";
                await Task.CompletedTask;
            }
            dataSource.Setup(x => x.GetData()).Returns(StreamData);
            var service = new WordCounterService(dataSource.Object);
            var enumerator = service.GetWordCountUpdates(new[] { "foo" }, cancellationToken).GetAsyncEnumerator(cancellationToken);
     
            source.Cancel();

            Assert.That(async () => await enumerator.MoveNextAsync(), Throws.TypeOf<OperationCanceledException>());
        }
    }
}
