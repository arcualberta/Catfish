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

            foreach (var itemEntry in highlightsContainer.Elements("lst"))
            {
                ResultEntry resultEntry = new ResultEntry();
                ResultEntries.Add(resultEntry);

                resultEntry.Id = Guid.Parse(itemEntry.Attribute("name").Value);
                foreach (var fieldEntry in itemEntry.Elements("arr"))
                {
                    var fieldKey = fieldEntry.Attribute("name").Value.Split("_");
                    var filedContainerType = SearchFieldConstraint.Str2Scope(fieldKey[0]);
                    var filedContainerId = Guid.Parse(fieldKey[1]);
                    var filedId = Guid.Parse(fieldKey[2]);

                    var resultSnippet = new ResultEntrySnippet()
                    {
                        Scope = filedContainerType,
                        ContainerId = filedContainerId,
                        FieldId = filedId
                    };

                    foreach (var snippet in fieldEntry.Elements("str"))
                    {
                        resultSnippet.Highlights.Add(snippet.Value);
                    }

                    resultEntry.Snippets.Add(resultSnippet);
                }
            }
        }

    }
}
