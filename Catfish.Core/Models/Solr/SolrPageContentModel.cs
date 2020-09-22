using Markdig.Extensions.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrPageContentModel
    {
        public enum eContentType { Page, Post, Comment, Block }

        public eContentType ContenType { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? PageId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? BlockId { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
    }
}


