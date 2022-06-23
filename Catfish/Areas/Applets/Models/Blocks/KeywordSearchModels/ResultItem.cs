using System;
using System.Collections.Generic;

namespace Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels
{
    public class ResultItem
    {
        public Guid Id { get; set; }
        public Guid RootFormInstaceId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Date { get; set; }
        public string DetailedViewUrl { get; set; }
        public List<string> Categories { get; set; } = new List<string>();

        public Dictionary<string, string[]> SolrFields { get; set; } = new Dictionary<string, string[]>();
    }
}
