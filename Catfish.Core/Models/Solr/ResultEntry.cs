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
        public Guid RootFormInstaceId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid StatusId { get; set; }

        public List<ResultEntryField> Fields { get; set; } = new List<ResultEntryField>();

        public ResultEntry(XElement doc)
        {
            //set the item ID
            string valStr = doc.Elements("str")
                .Where(ele => ele.Attribute("name").Value == "id")
                .Select(ele => ele.Value)
                .FirstOrDefault();
            Id = string.IsNullOrEmpty(valStr) ? Guid.Empty : Guid.Parse(valStr);

            //set the item template ID
            valStr = doc.Elements("str")
                .Where(ele => ele.Attribute("name").Value == "template_s")
                .Select(ele => ele.Value)
                .FirstOrDefault();
            TemplateId = string.IsNullOrEmpty(valStr) ? Guid.Empty : Guid.Parse(valStr);

            //set the root form instance ID
            valStr = doc.Elements("str")
                .Where(ele => ele.Attribute("name").Value == "root_form_instance_id_s")
                .Select(ele => ele.Value)
                .FirstOrDefault();
            RootFormInstaceId = string.IsNullOrEmpty(valStr) ? Guid.Empty : Guid.Parse(valStr);

            //set the created date
            valStr = doc.Elements("date")
                .Where(ele => ele.Attribute("name").Value == "created_dt")
                .Select(ele => ele.Value)
                .FirstOrDefault();
            if (!string.IsNullOrEmpty(valStr))
                Created = DateTime.Parse(valStr);

            //set the updated date
            valStr = doc.Elements("date")
                .Where(ele => ele.Attribute("name").Value == "updated_dt")
                .Select(ele => ele.Value)
                .FirstOrDefault();
            if (!string.IsNullOrEmpty(valStr))
                Updated = DateTime.Parse(valStr);

            //set the status
            valStr = doc.Elements("str")
                .First(f => f.Attribute("name").Value == "status_s")
                .Value;
            if (!string.IsNullOrEmpty(valStr))
                StatusId = Guid.Parse(valStr);

            //Creating a dictionary of field names
            Dictionary<string, string> fieldNameDictionary = new Dictionary<string, string>();
            foreach(var ele in doc.Elements("str").Where(ele => ele.Attribute("name").Value.StartsWith("cf-fn_")))
            {
                var key = ele.Attribute("name").Value;
                fieldNameDictionary.Add(key.Substring(6, key.Length - 8), ele.Value); //exclude cf-fn_ at the begining and _s at the end
            }

            //Populating result fields
            Fields = doc.Elements("arr")
                .Where(arr =>
                    arr.Attribute("name").Value.StartsWith("data_") || 
                    arr.Attribute("name").Value.StartsWith("metadata_") ||
                    arr.Attribute("name").Value.StartsWith("_"))
                .Select(arr => new ResultEntryField(arr, fieldNameDictionary))
                .ToList();
        }

        public void SetFieldHighlights(XElement highlights)
        {
            foreach (var highlightFieldEntry in highlights.Elements("arr").Where(ele => ele.Attribute("name").Value != "doc_type_ss"))
            {
                var fieldKey = highlightFieldEntry.Attribute("name").Value;

                if (!(fieldKey.StartsWith("data_") || fieldKey.StartsWith("metadata_")))
                    continue;

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
