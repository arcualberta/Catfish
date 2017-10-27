using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests
{
    public class TestData
    {
        private static Random Rand = new Random();

        public static string LoremIpsum(int minWords = 5, int maxWords = 25, int minSentences = 1, int maxSentences = 1, int numParagraphs = 1)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            int numSentences = Rand.Next(maxSentences - minSentences) + minSentences + 1;
            int numWords = Rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                if (numParagraphs > 1)
                    result.Append("<p>");
                for (int s = 0; s < numSentences; s++)
                {
                    if (s > 0)
                        result.Append(" ");

                    for (int w = 0; w < numWords; w++)
                    {
                        string word = words[Rand.Next(words.Length)];
                        if (w == 0)
                            word = word.First().ToString().ToUpper() + String.Join("", word.Skip(1));

                        if (w > 0)
                            result.Append(" ");
                        result.Append(word);
                    }
                    result.Append(".");
                }
                if (numParagraphs > 1)
                    result.Append("</p>");
            }

            return result.ToString();
        }
    }
}
