using System;
using System.Collections.Generic;
using System.IO;
using WordCounter;

namespace WordCounterConsoleApp
{
    class FileDataSource : IDataSource
    {
        readonly string m_FileName;

        public FileDataSource(string fileName)
        {
            m_FileName = fileName;
        }

        public async IAsyncEnumerable<string> GetData()
        {
            using var reader = File.OpenText(m_FileName);
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                yield return line;
            }
        }
    }
}