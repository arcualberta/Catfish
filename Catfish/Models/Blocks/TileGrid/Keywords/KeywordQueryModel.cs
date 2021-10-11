using Catfish.Core.Models.Contents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid.Keywords
{
    public class KeywordQueryModel
    {
        public eAggregation Aggregation { get; set; }

        [JsonProperty("containers")]
        public List<KeywordFieldContainer> Containers { get; set; } = new List<KeywordFieldContainer>();

        public int Offset { get; set; }
        public int Max { get; set; }

        public KeywordQueryModel() { /*Aggregation = eAggregation.Intersection; */}
        public KeywordQueryModel(eAggregation fieldContainerAggregation)
        { 
            Aggregation = fieldContainerAggregation;
        }
        public KeywordQueryModel(FieldContainer src, eAggregation fieldContainerAggregation, eAggregation fieldAggregation, eAggregation fieldValueAggregation) 
        {
            Aggregation = fieldContainerAggregation;
            AddContainer(src, fieldAggregation, fieldValueAggregation); 
        }

        public void AddContainer(FieldContainer src, eAggregation fieldAggregation, eAggregation fieldValueAggregation)
        {
            Containers.Add(new KeywordFieldContainer(src, fieldAggregation, fieldValueAggregation));
        }

        public void SortKeywordsInFields()
        {
            foreach (var container in Containers)
                container.SortKeywordsInFields();
        }
    }
}
