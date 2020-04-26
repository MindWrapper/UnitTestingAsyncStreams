using System;
using System.Collections.Generic;

namespace WordCounter 
{
    public class WordCounterService
    {
        public WordCounterService(IDataSource dataSource)
        {
        }

        public IAsyncEnumerable<KeyValuePair<string, int>> GetWordCountUpdates(string[] words)
        {
            throw new NotImplementedException();
        }
    }
}