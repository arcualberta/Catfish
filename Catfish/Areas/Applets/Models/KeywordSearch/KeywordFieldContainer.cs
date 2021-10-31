using Catfish.Areas.Applets.Models.Solr;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.KeywordSearch
{
    public class KeywordFieldContainer : SolrQueryModel
    {
        public FieldContainerReference.eRefType ContainerType { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<KeywordField> Fields { get; set; } = new List<KeywordField>();

        public KeywordFieldContainer() { Aggregation = eAggregation.Intersection; }
        public KeywordFieldContainer(Guid srcContainerId, FieldContainerReference.eRefType srcContainerType, eAggregation fieldAggregation) 
        {
            Id = srcContainerId;
            ContainerType = srcContainerType;
            Aggregation = fieldAggregation;
        }
        public KeywordFieldContainer(FieldContainer src, eAggregation fieldAggregation, eAggregation fieldValueAggregation, Guid[] keywordFields = null)
        {
            Id = src.Id;
            ContainerType = src is MetadataSet ? FieldContainerReference.eRefType.metadata : FieldContainerReference.eRefType.data;
            Aggregation = fieldAggregation;
            Name = src.Name.GetConcatenatedContent(" / ");

            var optionsFields = src.Fields.OfType< OptionsField>();
            if (keywordFields == null)
                AddFields(optionsFields, fieldValueAggregation);
            else
                AddFields(optionsFields.Where(field => keywordFields.Contains(field.Id)), fieldValueAggregation);
        }

        public void AddFields(IEnumerable<OptionsField> srcFields, eAggregation fieldValueAggregation)
        {
            foreach (var field in srcFields)
                Fields.Add(new KeywordField(field, fieldValueAggregation));
        }

        public void SortKeywordsInFields()
        {
            foreach (var field in Fields)
                field.Values.Sort();
        }

        public string BuildSolrQuery()
        {
            string fieldNamePrefix = ContainerType == FieldContainerReference.eRefType.data ? "data" : "metadata";
            var queryParts = Fields.Select(c => c.BuildSolrQuery(fieldNamePrefix, Id)).ToList();
            string query = JoinQueryParts(queryParts);
            return query;
        }
    }
}
