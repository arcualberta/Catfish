using Markdig.Extensions.Tables;
using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrPageContentModel
    {
        public enum eContentType { Page, Post }
        [SolrField("contenType")]
        public List<eContentType> ContenType { get; set; } = new List<eContentType>();

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


        public void AddBlockContent(Guid blockId, Guid blockParentId, string content)
        {
            BlockGuids.Add(blockId);
            BlockParentGuids.Add(blockParentId);
            BlockContents.Add(content);
        }
    }
}


