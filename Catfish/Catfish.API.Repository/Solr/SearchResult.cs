using Catfish.API.Repository.DTOs;
using Catfish.API.Repository.Models.BackgroundJobs;
using System.Xml.Linq;

namespace Catfish.API.Repository.Solr
{
    public class SearchResult: PaginatedSearchResult
    {
        public List<SolrResultEntry> ResultEntries { get; set; }

        public SearchResult() { }
        public void InitFromXml(string response)
        {
            //_errorLog = errorLog;

            XElement resp = XElement.Parse(response);

            var result = resp.Element("result");
            TotalMatches = int.Parse(result.Attribute("numFound").Value);
            Offset = int.Parse(result.Attribute("start").Value);

            var highlightsContainer = resp.Elements("lst")
                .FirstOrDefault(el => el.Attribute("name").Value == "highlighting");

            ResultEntries = new List<SolrResultEntry>();
            foreach (var doc in result.Elements("doc"))
            {
                try
                {

                    //create a new entry for the doc (Item)
                    SolrResultEntry resultEntry = new SolrResultEntry(doc);
                    ResultEntries.Add(resultEntry);

                    //Setting field highlights
                    var highlightFieldList = highlightsContainer?.Elements("lst")
                        ?.FirstOrDefault(ele => ele?.Attribute("name")?.Value == resultEntry?.Id?.ToString());

                    if (highlightFieldList != null)
                        resultEntry.SetFieldHighlights(highlightFieldList);
                }
                catch (Exception ex)
                {
                    //_errorLog.Log(new Error(ex));
                }
            }
        }

        public void InitFromJson(string response)
        {
            ResultEntries = new List<SolrResultEntry>();

            Newtonsoft.Json.Linq.JObject jspnResp = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(response);
            foreach(Newtonsoft.Json.Linq.JToken child in jspnResp.Children())
            {
                if(child.Path == "response")
                {
                    var numFound = ((Newtonsoft.Json.Linq.JProperty)child).Value["numFound"].Value<string>;
                    TotalMatches = Convert.ToInt32(numFound.Target.ToString());

                    var childTokens = ((Newtonsoft.Json.Linq.JProperty)child).Value["docs"].Children();
                    foreach(var token in childTokens)
                    {
                        SolrResultEntry resultEntry = new SolrResultEntry(token);
                        ResultEntries.Add(resultEntry);
                    }
                }
                
            }
           
         }
    }
}
