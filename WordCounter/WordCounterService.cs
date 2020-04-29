using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WordCounter 
{
    public class WordCounterService
    {
        readonly IDataSource m_DataSource;

        public WordCounterService(IDataSource dataSource)
        {
            m_DataSource = dataSource;
        }

        public async IAsyncEnumerable<WordCountUpdate> GetWordCountUpdates(string[] words, CancellationToken cancellationToken = default, IProgress<int> progress = default)
        {
            await foreach (var text in m_DataSource.GetData())
            {
                progress?.Report(1);
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var word in words)
                {
                    var occurrencesCount = text.Split(' ', '.', ';').Count(x => x.Equals(word, StringComparison.OrdinalIgnoreCase));
                    if (occurrencesCount > 0)
                    {
                        yield return new WordCountUpdate(word, occurrencesCount);
                    }
                }
            }
        }
    }
}