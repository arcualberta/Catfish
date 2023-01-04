using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Catfish.API.Repository.Solr
{
    public class SolrResultEntry
    {
        public string Id { get; set; }
      
        public List<KeyValuePair<string, object>> Data { get; set; } = new List<KeyValuePair<string, object>>();
        public long Version { get; set; }
        public SolrResultEntry(JToken token)
        {
            string strToken = token.ToString();
            //Data = JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(strToken);

            foreach (JProperty item in token.Children())
            {
                if(item.Name == "id")
                { Id = item.Value.ToString();
                    continue;
                }
                Data.Add(new KeyValuePair<string, object>(item.Name, item.Value));
            }
        }

            public SolrResultEntry(XElement doc)
        {
            //set the item ID
            //string valStr = doc.Elements("str")
            //    .Where(ele => ele.Attribute("name").Value == "id")
            //    .Select(ele => ele.Value)
            //    .FirstOrDefault();
            //Id = string.IsNullOrEmpty(valStr) ? "" : valStr;

            string versionStr = doc.Elements("long")
                .Where(ele => ele.Attribute("name").Value == "version")
                .Select(ele => ele.Value)
                .FirstOrDefault();
            Version = Convert.ToInt64(versionStr);


            var strNodes = doc.Elements("str").ToList();
            var arrNodes = doc.Elements("arr").ToList();
            var dateNodes = doc.Elements("date").ToList();
            var doubleNodes = doc.Elements("double").ToList();
            var intNodes = doc.Elements("int").ToList();

            GetStrValuePair(strNodes);
            GetArrValuePair(arrNodes);
            GetDateValuePair(dateNodes);
            GetDoubleValuePair(doubleNodes);
            GetIntValuePair(intNodes);

        }

        public void GetStrValuePair (List<XElement> elements)
        {
            foreach(XElement el in elements)
            {
                string nodeName = el.Attribute("name").Value;
                string nodeValue = el.Value;
                if (nodeName == "id")
                {
                    Id = nodeValue;
                    continue;
                } 
                    
               
                Data.Add(new KeyValuePair<string, object>(nodeName, nodeValue));
            }
        }
        public void GetArrValuePair(List<XElement> elements)
        {
            foreach (XElement el in elements)
            {
                string nodeName = el.Attribute("name").Value;
               

                if(nodeName.EndsWith("_ts") || nodeName.EndsWith("_ss"))
                {
                    List<string> values = new List<string>();
                    foreach(XElement node in el.Nodes())
                    {
                        values.Add(node.Value);
                        
                    }
                    Data.Add(new KeyValuePair<string, object>(nodeName,values));
                }
                else if(nodeName.EndsWith("_is"))
                {
                    List<int> intValues = new List<int>();
                    foreach (XElement node in el.Nodes())
                    {
                        intValues.Add(Convert.ToInt32(node.Value));

                    }
                    Data.Add(new KeyValuePair<string, object>(nodeName, intValues));
                }
                else if (nodeName.EndsWith("_ds"))
                {
                    List<double> doubleValues = new List<double>();
                    foreach (XElement node in el.Nodes())
                    {
                        doubleValues.Add(Convert.ToDouble(node.Value));

                    }
                    Data.Add(new KeyValuePair<string, object>(nodeName, doubleValues));
                }
                else if (nodeName.EndsWith("_dts"))
                {
                    List<DateTime> dtValues = new List<DateTime>();
                    foreach (XElement node in el.Nodes())
                    {
                        dtValues.Add(DateTime.Parse(node.Value));

                    }
                    Data.Add(new KeyValuePair<string, object>(nodeName, dtValues));
                }

            }
        }

        public void GetDoubleValuePair(List<XElement> elements)
        {
            foreach (XElement el in elements)
            {
                string nodeName = el.Attribute("name").Value;
                double nodeValue = Convert.ToDouble(el.Value.ToString());
                Data.Add(new KeyValuePair<string, object>(nodeName, nodeValue));
            }
        }
        public void GetDateValuePair(List<XElement> elements)
        {
            foreach (XElement el in elements)
            {
                string nodeName = el.Attribute("name").Value;
                string nodeValue = el.Value;
                Data.Add(new KeyValuePair<string, object>(nodeName, nodeValue));
            }
        }

        public void GetIntValuePair(List<XElement> elements)
        {
            foreach (XElement el in elements)
            {
                string nodeName = el.Attribute("name").Value;
                int nodeValue =Convert.ToInt32(el.Value.ToString());
                Data.Add(new KeyValuePair<string, object>(nodeName, nodeValue));
            }
        }
        public void SetFieldHighlights(XElement highlights)
        {
            foreach (var highlightFieldEntry in highlights.Elements("arr").Where(ele => ele.Attribute("name").Value != "doc_type_ss"))
            {
                var fieldKey = highlightFieldEntry.Attribute("name").Value;

              //  if (!(fieldKey.StartsWith("data_") || fieldKey.StartsWith("metadata_")))
              //      continue;

                string[] fieldKeyParts = fieldKey.Split("_");
                var containerType = SearchFieldConstraint.Str2Scope(fieldKeyParts[0]);
                var containerId = Guid.Parse(fieldKeyParts[1]);
                var feildId = Guid.Parse(fieldKeyParts[2]);

                //????
               // var field = Fields.Where(f => f.Scope == containerType && f.ContainerId == containerId && f.FieldId == feildId).FirstOrDefault();
               // field.SetHighlights(highlightFieldEntry)
            }
        }
    }
}
