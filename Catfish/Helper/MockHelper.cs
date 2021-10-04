using Catfish.Models.Blocks.TileGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Helper
{
    public static class MockHelper
    {
        public static string LoremIpsum(int minWords, int maxWords,
            int minSentences, int maxSentences,
            int numParagraphs)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                if (numParagraphs > 1)
                    result.Append("<p>");
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
                if (numParagraphs > 1)
                    result.Append("</p>");
            }

            return result.ToString();
        }

        public static List<Tile> GetMockupTileGridData()
        {
            List<Tile> tiles = new List<Tile>();
            int n = 250;
            string[] keywords = new string[] { "Age-appropriate tasks", "Assessment", "Culture", "Games", "Grammar", "Interaction",
                                                "Listening", "Materials", "Reading", "Real-life tasks", "Speaking", "Teacher training",
                                                "Technology", "Vocabulary", "Writing" };

            Random rand = new Random(0);

            for (int i = 0; i < n; ++i)
            {
                tiles.Add(new Tile()
                {
                    Id = Guid.NewGuid(),
                    Title = "Item " + (i + 1),
                    Content = Helper.MockHelper.LoremIpsum(4, 8, 1, 5, 1),
                    Date = DateTime.Now.AddDays(i),
                    Thumbnail = "https://www.almanac.com/sites/default/files/styles/primary_image_in_article/public/image_nodes/dahlia-3598551_1920.jpg?itok=XZfJlur2",
                    DetailedViewUrl = "https://www.ualberta.ca/",
                    Categories = rand.Next(0, 2) < 1
                    ? new string[] { keywords[rand.Next(0, keywords.Length)], keywords[rand.Next(0, keywords.Length)] }
                    : new string[] { keywords[rand.Next(0, keywords.Length)] }
                });
            }

            return tiles;
        }

        public static SearchResult FilterMockupTileGridData(string[] slectedKeywords, int offset = 0, int max = 0)
        {
            List<Tile> dbData = GetMockupTileGridData();

            List<Tile> tiles = new List<Tile>();
            if (slectedKeywords.Length > 0)
            {
                foreach (var t in dbData)
                {
                    foreach (var cat in t.Categories)
                    {
                        if (slectedKeywords.Contains(cat))
                        {
                            tiles.Add(t);
                            break;
                        }
                    }
                }
            }
            else
                tiles = dbData;

            SearchResult result = new SearchResult() { Count = tiles.Count };

            if (offset > 0)
                tiles = tiles.Skip(offset).ToList();

            if (max == 0)
                max = 25;
            tiles = tiles.Take(max).ToList();

            result.Items = tiles;
            result.First = offset + 1;
            result.Last = result.First + result.Items.Count() - 1;
            return result;
        }

    }
}
