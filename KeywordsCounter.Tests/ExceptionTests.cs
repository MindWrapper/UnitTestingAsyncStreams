using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WordCounter;

namespace KeywordsCounter.Tests
{
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void GetWordCountUpdates_DataSourceThrowsAndException_RethrowsTheException()
        {
           
            async IAsyncEnumerable<string> StreamData()
            {
                await Task.FromException(new IOException("hard disk is corrupted"));
                yield break; // otherwise compiler will complain "not all path return a value"
            }
            var dataSource = new Mock<IDataSource>();
            dataSource.Setup(x => x.GetData()).Returns(StreamData);
            var service = new WordCounterService(dataSource.Object);

            var firstUpdate = service.GetWordCountUpdates(new[] { "foo" }).FirstAsync();

            Assert.That(async () => await firstUpdate, Throws.InstanceOf<IOException>());
        }
    }
}
