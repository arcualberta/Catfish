using SolrNet.Attributes;
using SolrNet.Impl;
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

        [SolrField("language_s")]
        public string Language { get; set; }

        [SolrField("title_s")]
        public string Title { get; set; }

        [SolrField("excerpt_s")]
        public string Excerpt { get; set; }

        [SolrField("permalink_s")]
        public string Permalink { get; set; }

        public eEntryType ObjectType { get; set; }

        [SolrField("object_type_i")]
        public int object_type_i { get { return (int)ObjectType; } set { ObjectType = (eEntryType)Enum.ToObject(typeof(eEntryType), value); } }

        [SolrField("containerId")]
        public List<Guid?> ContainerIds { get; set; } = new List<Guid?>();

        [SolrField("content")]
        public List<string> Contents { get; set; } = new List<string>();

        public SolrPageContentModel PageContent { get; set; }
        public List<string> Highlights { get; set; } = new List<string>();

        public void AddContent(Guid containerId, string content)
        {
            ContainerIds.Add(containerId);
            Contents.Add(content);
        }
    }
}
