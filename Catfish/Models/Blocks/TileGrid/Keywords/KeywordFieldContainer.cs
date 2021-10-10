using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid.Keywords
{
    public class KeywordFieldContainer
    {
        public eAggregation Aggregation { get; set; } = eAggregation.Intersection;
        public FieldContainerReference.eRefType ContainerType { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<KeywordField> Fields { get; set; } = new List<KeywordField>();

        public KeywordFieldContainer() { }
        public KeywordFieldContainer(FieldContainer src, eAggregation fieldAggregation, eAggregation fieldValueAggregation)
        {
            Aggregation = fieldAggregation;
            Name = src.Name.GetConcatenatedContent(" / ");
            AddFields(src, fieldValueAggregation);
        }

        public void AddFields(FieldContainer src, eAggregation fieldValueAggregation)
        {
            foreach (var field in src.Fields.Where(f => f is OptionsField))
                Fields.Add(new KeywordField(field as OptionsField, fieldValueAggregation));
        }

    }
}
