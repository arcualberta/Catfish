using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.DataMockupHelpers
{
    public class KeywordSearchMockupHelper
    {
        protected List<KeywordFieldContainer> _keywords;
        protected int _dataSetSize;
        protected Random _rand = new Random(0);
        public KeywordSearchMockupHelper(List<KeywordFieldContainer> keywords, int dataSetSize)
        {
            _keywords = keywords;
            _dataSetSize = dataSetSize;
        }

        public List<ResultItem> GetMockupData()
        {
            List<ResultItem> tiles = new List<ResultItem>();

            Random rand = new Random(0);

            for (int i = 0; i < _dataSetSize; ++i)
            {
                tiles.Add(new ResultItem()
                {
                    Id = Guid.NewGuid(),
                    Title = "Item " + (i + 1),
                    Subtitle = Helper.MockHelper.LoremIpsum(2, 3, 1, 1, 1),
                    Content = Helper.MockHelper.LoremIpsum(5, 10, 5, 50, 1),
                    Date = new DateTime(rand.Next(2020, 2021), rand.Next(1, 12), rand.Next(1, 28)),
                    Thumbnail = "https://www.almanac.com/sites/default/files/styles/primary_image_in_article/public/image_nodes/dahlia-3598551_1920.jpg?itok=XZfJlur2",
                    DetailedViewUrl = i%2 == 1 ? "/" : "",
                    Categories = rand.Next(0, 2) < 1
                    ? new List<string>(new string[] { GetKeywordAtRandom(), GetKeywordAtRandom() })
                    : new List<string>(new string[] { GetKeywordAtRandom() }),
                });
            }

            return tiles;
        }

        public SearchOutput FilterMockupData(KeywordQueryModel queryModel, int offset = 0, int max = 0)
        {
            List<ResultItem> dbData = GetMockupData();

            List<ResultItem> tiles = new List<ResultItem>();
            List<string> slectedKeywords = new List<string>();
            if (queryModel != null)
            {
                foreach (var cont in queryModel.Containers)
                {
                    foreach (var field in cont.Fields)
                    {
                        for (int i = 0; i < field.Values.Count; ++i)
                        {
                            if (field.Selected[i])
                                slectedKeywords.Add(field.Values[i]);
                        }
                    }
                }
            }

            if (slectedKeywords.Count > 0)
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

            SearchOutput result = new SearchOutput() { Count = tiles.Count };

            if (offset > 0)
                tiles = tiles.Skip(offset).ToList();

            if (max == 0)
                max = 25;
            tiles = tiles.Take(max).ToList();

            result.Items = tiles;
            result.First = offset + 1;
            result.Last = result.First + result.Items.Count - 1;
            return result;
        }

        protected string GetKeywordAtRandom()
        {
            KeywordFieldContainer container = _keywords[_rand.Next(0, _keywords.Count)];
            KeywordField field = container.Fields[_rand.Next(0, container.Fields.Count)];
            string keyword = field.Values[_rand.Next(0, field.Values.Count)];
            return keyword;
        }
    }
}
