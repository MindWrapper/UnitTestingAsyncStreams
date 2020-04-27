using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WordCounter 
{
    public class WordCounterService
    {
        readonly IDataSource m_DataSource;

        public WordCounterService(IDataSource dataSource)
        {
            m_DataSource = dataSource;
        }

        public async IAsyncEnumerable<WordCountUpdate> GetWordCountUpdates(string[] words, CancellationToken cancellationToken = default)
        {
            await foreach (var text in m_DataSource.GetData())
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var word in words)
                {
                    var occurrencesCount = text.Split(' ', '.', ';').Count(x => x.Equals(word));
                    if (occurrencesCount > 0)
                    {
                        yield return new WordCountUpdate(word, occurrencesCount);
                    }
                }
            }
        }
    }
}