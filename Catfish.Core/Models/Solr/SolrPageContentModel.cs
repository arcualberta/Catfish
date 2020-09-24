using Markdig.Extensions.Tables;
using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrPageContentModel
    {
        public enum eContentType { Page, Post, Comment, Block }
        [SolrField("contenType")]
        public eContentType ContenType { get; set; }
        
        [SolrField("parentId")]
        public Guid? ParentId { get; set; } 

        [SolrField("pageId")]
        public Guid? PageId { get; set; }

        [SolrField("PostId")]
        public Guid? PostId { get; set; }

        [SolrField("blockId")]
        public Guid? BlockId { get; set; }

        [SolrField("title")]
        public string Title { get; set; }

        [SolrField("excerpt")]
        public string Excerpt { get; set; }

        [SolrField("content")]
        public string Content { get; set; }
    }
}


