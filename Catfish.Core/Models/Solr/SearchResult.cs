using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Solr
{
    public class SearchResult
    {
        public List<ResultEntry> ResultEntries { get; set; }

        public int Offset { get; set; }

        /// <summary>
        /// The total number of matches found, which may be larger than the page size.
        /// </summary>
        public int TotalMatches { get; set; }


        public SearchResult(string response)
        {
            XElement resp = XElement.Parse(response);

            var result = resp.Element("result");
            TotalMatches = int.Parse(result.Attribute("numFound").Value);
            Offset = int.Parse(result.Attribute("start").Value);

            var highlightsContainer = resp.Elements("lst")
                .Where(el => el.Attribute("name").Value == "highlighting")
                .FirstOrDefault();

            ResultEntries = new List<ResultEntry>();
            foreach (var doc in result.Elements("doc"))
            {
                //create a new entry for the doc (Item)
                ResultEntry resultEntry = new ResultEntry();
                ResultEntries.Add(resultEntry);

                //set the item ID
                resultEntry.Id = doc.Elements("str")
                    .Where(ele => ele.Attribute("name").Value == "id")
                    .Select(ele => Guid.Parse(ele.Value))
                    .First();

                //set the item template ID
                resultEntry.TemplateId = doc.Elements("str")
                    .Where(ele => ele.Attribute("name").Value == "template_s")
                    .Select(ele => Guid.Parse(ele.Value))
                    .First();

                //Pupulate the highlight snippets
                var highlightFieldList = highlightsContainer.Elements("lst")
                    .Where(ele => ele.Attribute("name").Value == resultEntry.Id.ToString())
                    .FirstOrDefault();
                if(highlightFieldList != null)
                {
                    foreach(var highlightFieldEntry in highlightFieldList.Elements("arr"))
                    {
                        var fieldKey = highlightFieldEntry.Attribute("name").Value;
                        string[] fieldKeyParts = fieldKey.Split("_");
                        var filedContainerType = SearchFieldConstraint.Str2Scope(fieldKeyParts[0]);
                        var filedContainerId = Guid.Parse(fieldKeyParts[1]);
                        var filedId = Guid.Parse(fieldKeyParts[2]);

                        var resultSnippet = new ResultEntrySnippet()
                        {
                            Scope = filedContainerType,
                            ContainerId = filedContainerId,
                            FieldId = filedId
                        };
                        resultEntry.Snippets.Add(resultSnippet);

                        //Select the corresponding field-content from the document
                        var selectedFieldContents = doc.Elements("arr")
                            .Where(ele => ele.Attribute("name").Value == fieldKey)
                            .SelectMany(ele => ele.Elements("str"))
                            .Select(str => str.Value);
                        resultSnippet.FieldContent.AddRange(selectedFieldContents);

                        //add the highlight snippets
                        var snippets = highlightFieldEntry.Elements("str")
                            .Select(str => str.Value);
                        resultSnippet.Highlights.AddRange(snippets);



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

    }
}
