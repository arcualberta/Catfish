using ElmahCore;
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

        public int ItemsPerPage { get; set; }

        private readonly ErrorLog _errorLog;
        public SearchResult(string response, ErrorLog errorLog)
        {
            _errorLog = errorLog;

            XElement resp = XElement.Parse(response);

            var result = resp.Element("result");
            TotalMatches = int.Parse(result.Attribute("numFound").Value);
            Offset = int.Parse(result.Attribute("start").Value);

            var highlightsContainer = resp.Elements("lst")
                .FirstOrDefault(el => el.Attribute("name").Value == "highlighting");

            ResultEntries = new List<ResultEntry>();
            foreach (var doc in result.Elements("doc"))
            {
                try
                {

                    //create a new entry for the doc (Item)
                    ResultEntry resultEntry = new ResultEntry(doc);
                    ResultEntries.Add(resultEntry);

                    //Setting field highlights
                    var highlightFieldList = highlightsContainer.Elements("lst")
                        .FirstOrDefault(ele => ele.Attribute("name").Value == resultEntry.Id.ToString());

                    if (highlightFieldList != null)
                        resultEntry.SetFieldHighlights(highlightFieldList);
                }
                catch (Exception ex)
                {
                    _errorLog.Log(new Error(ex));
                }
            }
        }

    }
}
