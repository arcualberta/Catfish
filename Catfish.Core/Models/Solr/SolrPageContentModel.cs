using Markdig.Extensions.Tables;
using SolrNet.Attributes;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrPageContentModel
    {
        //[SolrField("contenType")]
        //public List<SolrEntry.eEntryType> ContenType { get; set; } = new List<SolrEntry.eEntryType>();

        [SolrField("cf_object_type")]
        public SolrEntry.eEntryType ContenType { get; set; }

        [SolrField("id")]
        public Guid Id { get; set; }

        [SolrField("parentId")]
        public List<Guid?> ParentId { get; set; } = new List<Guid?>();

        [SolrField("title")]
        public List<string> Title { get; set; } = new List<string>();

        [SolrField("excerpt")]
        public List<string> Excerpt { get; set; } = new List<string>();

        [SolrField("permalink")]
        public List<string> Permalink { get; set; } = new List<string>();


        [SolrField("blockGuid")]
        public List<Guid> BlockGuids { get; set; } = new List<Guid>();

        [SolrField("blockParentGuid")]
        public List<Guid> BlockParentGuids { get; set; } = new List<Guid>();

        [SolrField("blockContent")]
        public List<string> BlockContents { get; set; } = new List<string>();

        public SolrPageContentModel()
        {

        }

        public SolrPageContentModel(KeyValuePair<string, HighlightedSnippets> highlights)
        {
            InitFromSolrSearchResultHighlights(highlights);
        }

        public void AddBlockContent(Guid blockId, Guid blockParentId, string content)
        {
            BlockGuids.Add(blockId);
            BlockParentGuids.Add(blockParentId);
            BlockContents.Add(content);
        }

        public void InitFromSolrSearchResultHighlights(KeyValuePair<string, HighlightedSnippets> highlights)
        {
            Id = Guid.Parse(highlights.Key);
            foreach(var hl in highlights.Value)
            {
                if(hl.Value.Count > 0)
                {
                    if (hl.Key == "title")
                        Title.AddRange(hl.Value);
                    else if (hl.Key == "excerpt")
                        Excerpt.AddRange(hl.Value);
                    else if (hl.Key == "blockContent")
                        BlockContents.AddRange(hl.Value);
                }
            }
        }
    }
}


