using System.Collections.Generic;

namespace WordCounter 
{
    public class WordCounterService
    {
        public WordCounterService(IDataSource dataSource)
        {
        }

        public async IAsyncEnumerable<WordCountUpdate> GetWordCountUpdates(string[] words)
        {
            foreach (var word in words)
            {
                yield return new WordCountUpdate(string.Empty, 0);
            }
        }
    }
}