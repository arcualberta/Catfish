using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Solr
{
    public class ResultEntryField
    {
        public SearchFieldConstraint.eScope Scope { get; set; }
        public Guid ContainerId { get; set; }
        public Guid FieldId { get; set; }
        public string FieldName { get; set; }
        public List<string> FieldContent { get; set; } = new List<string>();

        public List<string> Highlights = new List<string>();
        public string FieldKey { get; set; }

        public ResultEntryField(XElement arr, Dictionary<string, string> fieldNameDictionary)
        {
            FieldKey = arr.Attribute("name").Value;
            if (!FieldKey.StartsWith("_"))
            {
                string[] fieldKeyParts = FieldKey.Split("_");
                var filedContainerType = SearchFieldConstraint.Str2Scope(fieldKeyParts[0]);
                var filedContainerId = Guid.Parse(fieldKeyParts[1]);
                var filedId = Guid.Parse(fieldKeyParts[2]);

                Scope = filedContainerType;
                ContainerId = filedContainerId;
                FieldId = filedId;
            }

            FieldName = fieldNameDictionary?.FirstOrDefault(ele => ele.Key == FieldKey).Value;
            FieldContent = arr.Elements("str").Select(str => str.Value).ToList();
        }

        public void SetHighlights(XElement highlights)
        {
            var snippets = highlights.Elements("str")
                .Select(str => str.Value);
            Highlights.AddRange(snippets);
        }
    }
}
