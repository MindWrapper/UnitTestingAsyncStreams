using System.Collections.Generic;

namespace WordCounter 
{
    public class WordCounterService
    {
        readonly IDataSource m_DataSource;

        public WordCounterService(IDataSource dataSource)
        {
            m_DataSource = dataSource;
        }

        public async IAsyncEnumerable<WordCountUpdate> GetWordCountUpdates(string[] words)
        {
            await foreach (var text in m_DataSource.GetData())
            {
                foreach (var word in words)
                {
                    if (text.Contains(word))
                    {
                        yield return new WordCountUpdate(word, 1);
                    }
                }
            }
        }
    }
}