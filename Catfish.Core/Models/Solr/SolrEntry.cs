using SolrNet.Attributes;
using SolrNet.Impl;
using System;
using SolrNet.Attributes;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrEntry
    {
        public enum eEntryType { Page = 1, Post, ItemTemplate, Item, CollectionTemplate, Collection, Other }

        [SolrField("id")]
        public Guid Id { get; set; }

        [SolrField("title_t")]
        public List<string> Title { get; set; } = new List<string>();

        [SolrField("title_id_ss")]
        public List<Guid?> TitleId { get; set; } = new List<Guid?>();

        [SolrField("permalink_s")]
        public string Permalink { get; set; }

        public eEntryType ObjectType { get; set; }

        [SolrField("object_type_i")]
        public int object_type_i { get { return (int)ObjectType; } set { ObjectType = (eEntryType)Enum.ToObject(typeof(eEntryType), value); } }

        [SolrField("containerId_ss")]
        public List<Guid?> ContainerIds { get; set; } = new List<Guid?>();

        [SolrField("content_t")]
        public List<string> Contents { get; set; } = new List<string>();

        [SolrField("imageContainerId_ss")]
        public List<Guid?> ImageContainerIds { get; set; } = new List<Guid?>();

        [SolrField("images_ss")]
        public List<string> Images { get; set; } = new List<string>();

        [SolrField("keywords_ss")]
        public List<string> Keywords { get; set; } = new List<string>();

        [SolrField("categories_ss")]
        public List<string> Categories { get; set; } = new List<string>();

        public List<string> Highlights { get; set; } = new List<string>();

        public void SetTitle(Guid titleId, string titleValue)
        {
            TitleId.Add(titleId);
            Title.Add(titleValue);
        }
        public void AddContent(Guid containerId, string content)
        {
            ContainerIds.Add(containerId);
            Contents.Add(content);
        }

        public void AddImage(Guid containerId, string url)
        {
            ImageContainerIds.Add(containerId);
            Images.Add(url);
        }

    }
}
