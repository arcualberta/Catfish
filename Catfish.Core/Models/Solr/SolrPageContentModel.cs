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
        public eContentType ContenType { get; set; }

        [SolrField("id")]
        public Guid Id { get; set; }

        [SolrField("parentId")]
        public Guid? ParentId { get; set; }

        [SolrField("title")]
        public string Title { get; set; }

        [SolrField("excerpt")]
        public string Excerpt { get; set; }

        [SolrField("permalink")]
        public string Permalink { get; set; }


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


