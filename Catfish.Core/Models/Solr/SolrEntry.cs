using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrEntry
    {
        public enum eEntryType { Item = 1, Collection, Page, Post }

        [SolrField("id")]
        public Guid Id { get; set; }

        [SolrField("title")]
        public List<string> Title { get; set; } = new List<string>();

        [SolrField("excerpt")]
        public List<string> Excerpt { get; set; } = new List<string>();

        [SolrField("parentId")]
        public List<Guid?> ParentId { get; set; } = new List<Guid?>();

        [SolrField("permalink")]
        public List<string> Permalink { get; set; } = new List<string>();

        public eEntryType ObjectType { get; set; }
        public SolrPageContentModel PageContent { get; set; }
        public List<string> Highlights { get; set; } = new List<string>();

    }
}
