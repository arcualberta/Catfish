using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Services;
using Catfish.Core.Models;
using Catfish.Models.Regions;
using System.IO;
using System.Text;

namespace Catfish.Services
{
    public class ItemQueryService //: ServiceBase
    {
        
       // public ItemQueryService(CatfishDbContext db) : base(db) { }

        public IEnumerable<QueryResultObject> GetCalculatedField(string functionName, string SelectedFieldMetadataSet, string SelectedField, string SelectedFilterMetadataSet, string selectedFilterField, int min, int max)
        {
            CatfishDbContext db = new CatfishDbContext();
            if(functionName == "COUNT")
            {
                functionName = "SUM"; //when it's only asking for count, this function operation is not matter
            }
            string xQuerySelect = "SELECT " + functionName + "(a.Amount) AS calculatedValue, COUNT(*) as 'count' " +
                             " FROM(" +
                             " SELECT  Content.value('(/item/metadata/metadata-set[@guid=\"" + SelectedFieldMetadataSet + "\"]/fields/field[@guid=\"" + SelectedField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount, " +
                              " Content.value('(/item/metadata/metadata-set[@guid=\"" + SelectedFilterMetadataSet + "\"]/fields/field[@guid=\"" + selectedFilterField + "\"]/value/text/text())[1]', 'INT') AS Year " +

                              " FROM [dbo].[CFXmlModels]" +
                             
                              " WHERE Discriminator = 'CFItem' AND Content.exist('number((/item/metadata/metadata-set[@guid=\"" + SelectedFieldMetadataSet + "\"]/fields/field[@guid=\"" + SelectedField + "\"]/value/text)[1])') = 1 " +
                              " ) as a" +
                               " WHERE a.Year >= " + min + " AND a.Year <= " + max;


            var result = db.Database.SqlQuery<QueryResultObject>(xQuerySelect, new object[] { functionName, SelectedFieldMetadataSet, SelectedField, SelectedFilterMetadataSet, selectedFilterField, SelectedFieldMetadataSet });
            return result;
        }

        private IEnumerable<GraphQueryObject> ReadFacet(System.Xml.XmlReader reader)
        {
            int level = 1;
            int xVal = 0;
            decimal yVal = 0;
            int count = 0;
            string category = string.Empty;

            IList<GraphQueryObject> result = new List<GraphQueryObject>();

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if(reader.Name == "lst")
                    {
                        ++level;
                    }else if(reader.Name == "int" && level == 3 && reader.GetAttribute("name") == "val")
                    {
                        reader.Read();
                        xVal = reader.ReadContentAsInt();
                    }else if(level == 5)
                    {
                        string name = reader.GetAttribute("name");
                        reader.Read();
                        string value = reader.ReadContentAsString();

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (name == "val")
                            {
                                category = value;
                            }
                            else if (name == "count")
                            {
                                count = int.Parse(value);
                            }
                            else if (name == "sumYValuesArg")
                            {
                                yVal = Convert.ToDecimal(double.Parse(value));
                            }
                        }
                    }
                }
                else if(reader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.Name == "lst")
                    {
                        --level;

                        if (level < 1)
                        {
                            break;
                        }else if(level == 2)
                        {
                            xVal = 0;
                        }else if(level == 4)
                        {
                            result.Add(new GraphQueryObject()
                            {
                                XValue = yVal,
                                YValue = xVal,
                                Category = category,
                                Count = count
                            });

                            yVal = 0.0m;
                            category = string.Empty;
                            count = 0;
                        }
                    }
                }
            }

            return result;
        }

        private IEnumerable<GraphQueryObject> ConvertSolrXml(string solrXml)
        {
            IEnumerable<GraphQueryObject> result = null;

            MemoryStream memStream = new MemoryStream();
            byte[] data = Encoding.Default.GetBytes(solrXml);
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;
            
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(memStream))
            {
                while (reader.Read())
                {
                    if(reader.IsStartElement() && reader.Name == "lst" && reader.GetAttribute("name") == "facets")
                    {
                        result = ReadFacet(reader);
                    }
                }
            }

            return result;
        }

        public IEnumerable<GraphQueryObject> GetGraphData(string q, string xMetadataSet, string xField, string yMetadataSet, string yField, string catMetadataSet, string catField, bool isCatDropdown = false, string languageCode = "en")
        {
            string xIndexId = string.Format("value_{0}_{1}_i", xMetadataSet.Replace('-', '_'), xField.Replace('-', '_'));
            string yIndexId = string.Format("value_{0}_{1}_i", yMetadataSet.Replace('-', '_'), yField.Replace('-', '_'));
            string catIndexId = string.Format("{2}value_{0}_{1}_txt_{3}", catMetadataSet.Replace('-', '_'), catField.Replace('-', '_'), isCatDropdown ? "option_" : "", languageCode);

            SolrService solrSrv = new SolrService();
            string result = solrSrv.GetGraphData(q, xIndexId, yIndexId, catIndexId);

            if (string.IsNullOrEmpty(result)) { return null; }

            return ConvertSolrXml(result);
        }

        public IEnumerable<GraphQueryObject> GetGraphData_old(string xMetadataSet, string xField, string yMetadataSet, string yField, string catMetadataSet, string catField, int xmin = 0, int xmax = 0)
        {
            CatfishDbContext db = new CatfishDbContext();
            xmin = xmin == 0 ? DateTime.MinValue.Year : xmin;
            xmax = xmax == 0 ? DateTime.Now.Year : xmax;
            //string xQuerySelect = "SELECT a.Year as YValue, SUM(a.Amount) AS XValue, COUNT(*) as 'Count', a.Category" +
            //                      " FROM(" +
            //                      " SELECT  Content.value('(/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]/fields/field[@guid=\"" + xField + "\"]/value/text/text())[1]', 'INT') AS Year ," +
            //                       " Content.value('(/item/metadata/metadata-set[@guid=\"" + yMetadataSet + "\"]/fields/field[@guid=\"" + yField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount," +
            //                       " Content.value('(/item/metadata/metadata-set[@guid=\"" + catMetadataSet + "\"]/fields/Field[@guid=\"" + catField + "\"]/options/option[@selected=\"true\"]/text/text())[1]', 'VARCHAR(25)') AS Category" +
            //                       " FROM[dbo].[CFXmlModels]" +
            //                       " WHERE Discriminator = 'CFItem' AND Content.exist('/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]') = 1" +
            //                       " ) as a" +
            //                        " WHERE a.Year >= " + xmin + " AND a.Year <= " + xmax +
            //                        " GROUP BY a.Year, a.Category" +
            //                        " ORDER BY a.Year";
            string xQuerySelect = "SELECT a.Year as YValue, SUM(a.Amount) AS XValue, COUNT(*) as 'Count' , a.Category" +
                                   " FROM(" +
                                   " SELECT Content.value('(/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]/fields/field[@guid=\"" + xField + "\"]/value/text/text())[1]', 'INT') AS Year ," +
                                    " Content.value('(/item/metadata/metadata-set[@guid=\"" + yMetadataSet + "\"]/fields/field[@guid=\"" + yField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount " +
                                     " , Content.value('(/item/metadata/metadata-set[@guid=\"" + catMetadataSet + "\"]/fields/Field[@guid=\"" + catField + "\"]/options/option[@selected=\"true\"]/text/text())[1]', 'VARCHAR(25)') AS Category" +
                                    " FROM[dbo].[CFXmlModels] " +
                                    " WHERE Discriminator = 'CFItem' AND Content.exist('number((/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]/fields/field[@guid=\"" + xField + "\"]/value/text)[1])') = 1 AND Content.exist('number((/item/metadata/metadata-set[@guid=\"" + yMetadataSet + "\"]/fields/field[@guid=\"" + yField + "\"]/value/text)[1])') = 1" +
                                    " ) as a" +
                                     " WHERE a.Year >= " + xmin + " AND a.Year <= " + xmax +
                                     " GROUP BY a.Year , a.Category" +
                                     " ORDER BY a.Year";

            var result = db.Database.SqlQuery<GraphQueryObject>(xQuerySelect, new object[] { xMetadataSet, xField, yMetadataSet, yField, catMetadataSet, catField, xMetadataSet });

            return result;

        }
    }
}