using System.Collections.Generic;

namespace WordCounter 
{
    public class WordCounterService
    {
        public WordCounterService(IDataSource dataSource)
        {
        }

        public async IAsyncEnumerable<KeyValuePair<string, int>> GetWordCountUpdates(string[] words)
        {
            foreach (var word in words)
            {
                yield return new KeyValuePair<string, int>(word, 0);
            }
        }
    }
}