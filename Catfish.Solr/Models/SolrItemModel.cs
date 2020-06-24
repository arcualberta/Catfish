using Catfish.Core.Models;
using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Solr.Models
{
    public class SolrItemModel
    {
        public SolrItemModel() { }

        public SolrItemModel(Item model)
        {
            Id = model.Id;
            Content = model.Content;
            Created = model.Created;
            Updated = model.Updated;
            PrimaryCollectionId = model.PrimaryCollectionId;
        }
        [SolrUniqueKey("id")]
        public Guid Id { get; set; }

        [SolrField("content")]
        [Column(TypeName = "xml")]
        public string Content { get; set; }

        [SolrField("created")]
        public DateTime? Created { get; set; }

        [SolrField("updated")]
        public DateTime? Updated { get; set; }
                
        [SolrField("primaryCollectionId")]
        public Guid? PrimaryCollectionId { get; set; }
    }
}
