using System;
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
            yield break;
        }
    }
}