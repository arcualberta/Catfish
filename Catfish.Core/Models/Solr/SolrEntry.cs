using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrEntry
    {
        public enum eEntryType { Item = 1, Collection, Page, Post }
        public eEntryType ObjectType { get; set; }
        public Guid ObjectId { get; set; }
        public SolrPageContentModel PageContent { get; set; }
        public List<string> Highlights { get; set; } = new List<string>();

    }
}
