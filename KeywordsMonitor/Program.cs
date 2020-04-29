using System;
using System.Threading;
using System.Threading.Tasks;
using WordCounter;

namespace KeywordsMonitor
{
    class Program
    {
        static async Task Main()
        {
            var wordsToCount = new[] { "war", "peace" };
            var dataSource = new FileDataSource("WarAndPeace.txt");
            var wordCountService = new WordCounterService(dataSource);
            var warCount = 0;
            var peaceCount = 0;
            var linesProcessed = 0;

            var progress = new Progress<int>(n => linesProcessed += n);
            await foreach (var update in wordCountService.GetWordCountUpdates(wordsToCount, CancellationToken.None, progress))
            {
                if (update.Word == "war")
                {
                    warCount += update.OccurrencesCount;
                }
                else
                {
                    peaceCount += update.OccurrencesCount;
                }
               
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Lines processed: {linesProcessed} war: {warCount} peace {peaceCount}");
            }
        }
    }
}
