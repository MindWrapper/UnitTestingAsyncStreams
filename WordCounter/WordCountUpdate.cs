namespace WordCounter
{
    public class WordCountUpdate
    {
        public WordCountUpdate(string word, int occurrencesCount)
        {
            Word = word;
            OccurrencesCount = occurrencesCount;
        }

        public string Word { get; }

        public int OccurrencesCount { get; }
    }
}
