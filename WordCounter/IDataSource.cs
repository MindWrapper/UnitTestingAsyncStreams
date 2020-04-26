using System.Collections.Generic;

namespace WordCounter 
{
    public interface IDataSource
    {
        IAsyncEnumerable<string> GetData();
    }
}
