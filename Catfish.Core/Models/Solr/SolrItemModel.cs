using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SolrItemModel
    {
        public SolrItemModel() { }
        public SolrItemModel(Item entity) { }

        [SolrField("entityGuid")]
        public List<Guid> EntityGuid { get; set; } = new List<Guid>();
        ////////public Guid EntityGuid { get; set; }

        [SolrField("entityType")]
        public List<string> EntityType { get; set; } = new List<string>();

        [SolrField("metadataSetGuid")]
        public List<Guid> MetadataSetGuid { get; set; } = new List<Guid>();

        [SolrField("fieldGuid")]
        public List<Guid> FieldGuid { get; set; } = new List<Guid>();

        [SolrField("valueGuid")]
        public List<Guid> ValueGuid { get; set; } = new List<Guid>();

        [SolrUniqueKey("textGuid")]
        public List<Guid> TextGuid { get; set; } = new List<Guid>();

        [SolrField("lang")]
        public List<string> Lang { get; set; } = new List<string>();

        [SolrField("content")]
        public List<string> Content { get; set; } = new List<string>();


        ////public SolrItemModel(Item model)
        ////{
        ////    Id = model.Id;
        ////    Name = model.Name;
        ////    Description = model.Description;
        ////    Content = model.Content;
        ////    Created = model.Created;
        ////    Updated = model.Updated;
        ////    MetadataSets = model.MetadataSets;
        ////    PrimaryCollectionId = model.PrimaryCollectionId;
        ////}
        ////[SolrUniqueKey("id")]
        ////public Guid Id { get; set; }

        ////[SolrField("content")]
        ////[Column(TypeName = "xml")]
        ////public string Content { get; set; }

        ////[SolrField("name")]
        ////public MultilingualText Name { get; set; }

        ////[SolrField("description")]
        ////public MultilingualText Description { get; set; }

        ////[SolrField("metadataSets")]
        ////public XmlModelList<MetadataSet> MetadataSets { get; set; }

        ////[SolrField("created")]
        ////public DateTime? Created { get; set; }

        ////[SolrField("updated")]
        ////public DateTime? Updated { get; set; }
                
        ////[SolrField("primaryCollectionId")]
        ////public Guid? PrimaryCollectionId { get; set; }
    }
}
