using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Solr
{
    public class ResultEntry
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public List<ResultEntryField> Fields { get; set; } = new List<ResultEntryField>();

        public ResultEntry(XElement doc)
        {
            //set the item ID
            Id = doc.Elements("str")
                .Where(ele => ele.Attribute("name").Value == "id")
                .Select(ele => Guid.Parse(ele.Value))
                .First();

            //set the item template ID
            TemplateId = doc.Elements("str")
                .Where(ele => ele.Attribute("name").Value == "template_s")
                .Select(ele => Guid.Parse(ele.Value))
                .First();

            //Populating result fields
            Fields = doc.Elements("arr").Select(arr => new ResultEntryField(arr)).ToList();

        }

        public void SetFieldHighlights(XElement highlights)
        {
            foreach (var highlightFieldEntry in highlights.Elements("arr"))
            {
                var fieldKey = highlightFieldEntry.Attribute("name").Value;
                string[] fieldKeyParts = fieldKey.Split("_");
                var containerType = SearchFieldConstraint.Str2Scope(fieldKeyParts[0]);
                var containerId = Guid.Parse(fieldKeyParts[1]);
                var feildId = Guid.Parse(fieldKeyParts[2]);

                var field = Fields.Where(f => f.Scope == containerType && f.ContainerId == containerId && f.FieldId == feildId).FirstOrDefault();
                field.SetHighlights(highlightFieldEntry);

                //////Select the corresponding field-content from the document
                ////var selectedFieldContents = doc.Elements("arr")
                ////    .Where(ele => ele.Attribute("name").Value == fieldKey)
                ////    .SelectMany(ele => ele.Elements("str"))
                ////    .Select(str => str.Value);
                ////resultSnippet.FieldContent.AddRange(selectedFieldContents);

                //////add the highlight snippets
                ////var snippets = highlightFieldEntry.Elements("str")
                ////    .Select(str => str.Value);
                ////resultSnippet.Highlights.AddRange(snippets);



                //////add the highlight snippets
                //////foreach(var snippet in highlightFieldEntry.Elements("str"))
                //////{
                //////    resultSnippet.Highlights.Add(snippet.Value);
                //////}
                ////var snippets = highlightFieldEntry.Elements("str")
                ////    .Select(str => str.Value)
                ////    .ToArray();

            }
        }

    }
}
