﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Services;
using Catfish.Core.Models;
using Catfish.Models.Regions;
using System.IO;
using System.Text;
using Catfish.Core.Models.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class ItemQueryService //: ServiceBase
    {
        public CatfishDbContext Db { get; protected set; }

        private SolrService mSolrSrv { get; set; }
        public SolrService SolrSrv
        {
            get
            {
                if(mSolrSrv == null)
                {
                    mSolrSrv = new SolrService();
                }

                return mSolrSrv;
            }
        }

        private MetadataService mMetadataSrv { get; set; }
        public MetadataService MetadataSrv
        {
            get
            {
                if(mMetadataSrv == null)
                {
                    mMetadataSrv = new MetadataService(Db);
                }

                return mMetadataSrv;
            }
        }

        public enum eFunctionMode
        {
            SUM = 0, COUNT, MEAN, MAX, MIN, STANDARD_DEVIATION, MEDIAN
        }

        public ItemQueryService(CatfishDbContext db)
        {
            Db = db;
        }

        public struct GroupByObject
        {
            public string GroupByName;
            public int Count;
            public decimal Total;/* for example Total Amount -- yField that passed as parameter */
        }

        public decimal GetCalculatedField(string query, eFunctionMode function, string SelectedFieldMetadataSet, string SelectedField, string SelectedGroupByFieldMetadataSet, string SelectedGroupByField, string languageCode = "en")
        {
            string metadataGuid = SelectedFieldMetadataSet.Replace('-', '_');
            string fieldGuid = SelectedField.Replace('-', '_');
            bool isOptionField = false;
            string resultType = "ds"; //TODO: check the field type. At the moment, we can only do stats on number fields.

            if (!string.IsNullOrEmpty(SelectedFieldMetadataSet) && !string.IsNullOrEmpty(SelectedField))
            {
                isOptionField = IsOptionField(SelectedFieldMetadataSet, SelectedField) ? true : false;
                resultType = IsNumberField(SelectedFieldMetadataSet, SelectedField) ? "ds" : "txts_" + languageCode;
            }


            string fieldString = string.Format("value_{0}_{1}_{2}", metadataGuid, fieldGuid, resultType);

            //adding group by
            string groupByFieldString = string.Empty;
            if (!string.IsNullOrEmpty(SelectedGroupByFieldMetadataSet) && !string.IsNullOrEmpty(SelectedGroupByField))
            {
                string groupByMetadataGuid = string.IsNullOrEmpty(SelectedGroupByFieldMetadataSet) ? string.Empty : SelectedGroupByFieldMetadataSet.Replace('-', '_');
                string groupByFieldGuid = string.IsNullOrEmpty(SelectedGroupByField) ? string.Empty : SelectedGroupByField.Replace('-', '_');

                resultType = IsNumberField(SelectedGroupByFieldMetadataSet, SelectedGroupByField) ? "ds" : languageCode + "_ss"; // This last bit is for full text groups.
                isOptionField = IsOptionField(SelectedGroupByFieldMetadataSet, SelectedGroupByField) ? true : false;

                groupByFieldString = string.Format("value_{0}_{1}_{2}",
                    groupByMetadataGuid, groupByFieldGuid,
                    resultType);
            }
           

            switch (function)
            {
                case eFunctionMode.SUM:
                    return SolrSrv.SumField(fieldString, query);

                case eFunctionMode.COUNT:
                    return SolrSrv.CountField(fieldString, query, groupByFieldString);

                case eFunctionMode.MEAN:
                    return SolrSrv.MeanField(fieldString, query);

                case eFunctionMode.MAX:
                    return SolrSrv.MaxField(fieldString, query);

                case eFunctionMode.MIN:
                    return SolrSrv.MinField(fieldString, query);

                case eFunctionMode.STANDARD_DEVIATION:
                    return SolrSrv.StandardDeviationField(fieldString, query);
                case eFunctionMode.MEDIAN:
                    return SolrSrv.MedianField(fieldString, query);
            }

            return -1m;
        }


        public bool IsNumberField(string metadataSetGuid, string fieldGuid)
        {
            //Sept 12 2018 -- check if the field is a numberField otherwise text field
            CFMetadataSet metadataSet = (new MetadataService(Db)).GetMetadataSet(metadataSetGuid);
            foreach (FormField formField in metadataSet.Fields)
            {
                if (formField.Guid.Equals(fieldGuid))
                {
                    if (typeof(NumberField).IsAssignableFrom(formField.GetType()))
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public bool IsOptionField(string metadataSetGuid, string fieldGuid)
        {
            //Sept 12 2018 -- check if the field is a numberField otherwise text field
            CFMetadataSet metadataSet = (new MetadataService(Db)).GetMetadataSet(metadataSetGuid);
            foreach (FormField formField in metadataSet.Fields)
            {
                if (formField.Guid.Equals(fieldGuid))
                {
                    if (typeof(OptionsField).IsAssignableFrom(formField.GetType()))
                    {
                        return true;
                    }
                }
            }

            return false;

        }
        /*public IEnumerable<QueryResultObject> GetCalculatedField_old(string functionName, string SelectedFieldMetadataSet, string SelectedField, string SelectedFilterMetadataSet, string selectedFilterField, int min, int max)
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
        }*/

        private IEnumerable<GraphQueryObject> ReadFacet(System.Xml.XmlReader reader, IDictionary<string, string> categories)
        {
            int level = 1;
            int xVal = 0;
            decimal yVal = 0;
            int count = 0;
            string category = string.Empty;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "lst")
                    {
                        ++level;
                    }
                    else if ((reader.Name == "int" || reader.Name == "long" || reader.Name == "double") && level == 3)
                    {
                        string name = reader.GetAttribute("name");
                        reader.Read();
                        string value = reader.ReadContentAsString();

                        if (name == "val")
                        {
                            xVal = int.Parse(value);
                        }else if(name == "count" && categories == null)
                        {
                            count = int.Parse(value);
                        }else if(name == "sumYValues" && categories == null)
                        {
                            try
                            {
                                yVal = Convert.ToDecimal(double.Parse(value));
                            }
                            catch(Exception fex)
                            {
                                throw new FormatException(string.Format("Unable to parse string \"{0}\" into decimal.", value), fex);
                            }
                        }
                    }else if (level == 5 && category != null)
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
                        } else if (level == 2)
                        {
                            if(categories == null)
                            {
                                yield return new GraphQueryObject()
                                {
                                    XValue = yVal,
                                    YValue = xVal,
                                    Category = null,
                                    Count = count
                                };

                                yVal = 0.0m;
                                count = 0;
                            }

                            xVal = 0;
                        } else if (level == 4 && categories != null)
                        {
                            yield return new GraphQueryObject()
                            {
                                XValue = yVal,
                                YValue = xVal,
                                Category = categories.ContainsKey(category) ? categories[category] : category,
                                Count = count
                            };

                            yVal = 0.0m;
                            category = string.Empty;
                            count = 0;
                        }
                    }
                }
            }
        }

        private IEnumerable<GraphQueryObject> ConvertSolrXml(string solrXml, IDictionary<string, string> categories)
        {
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
                        foreach (GraphQueryObject facet in ReadFacet(reader, categories))
                        {
                            // We add another yield here since we are inside a 'using' method. We do not want out reader to close before we are done itterating over the data. 
                            yield return facet;
                        }
                    }
                }
            }
        }

        private IEnumerable<GroupByObject> ConvertSolrXml2(string solrXml)
        {
            MemoryStream memStream = new MemoryStream();
            byte[] data = Encoding.Default.GetBytes(solrXml);
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;
            XmlDocument doc = new XmlDocument();
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new StringReader(solrXml)))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "lst" && reader.GetAttribute("name") == "facets")
                    {
                        foreach (GroupByObject facet in ReadFacet2(reader))
                        {
                            // We add another yield here since we are inside a 'using' method. We do not want out reader to close before we are done itterating over the data. 
                            yield return facet;
                        }
                    }
                }
            }
        }

        private IEnumerable<GroupByObject> ReadFacet2(System.Xml.XmlReader reader /*, IDictionary<string, string> categories*/)
        {
            int level = 1;
            decimal yVal = 0;
            int count = 0;
            string category = string.Empty;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "lst")
                    {
                        ++level;
                    }
                    else if ((reader.Name == "int" || reader.Name == "long" || reader.Name == "double") && level == 3)
                    {
                        string name = reader.GetAttribute("name");
                        reader.Read();
                        string value = reader.ReadContentAsString();

                       if (name == "count")
                        {
                            count = int.Parse(value);
                        }
                        else if (name == "sumYValues" )
                        {
                            try
                            {
                                yVal = Convert.ToDecimal(double.Parse(value));
                            }
                            catch (Exception fex)
                            {
                                throw new FormatException(string.Format("Unable to parse string \"{0}\" into decimal.", value), fex);
                            }
                        }
                    }
                    else if (level == 5 && category != null)
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
                else if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.Name == "lst")
                    {
                        --level;

                        if (level < 1)
                        {
                            break;
                        }
                       
                        else if (level == 4)// && category != null)
                        {
                           
                            yield return new GroupByObject()
                            {
                                Total = yVal,
                                GroupByName = category,//categories.ContainsKey(category) ? categories[category] : category,
                                Count = count
                            };

                            yVal = 0.0m;
                            category = string.Empty;
                            count = 0;
                        }
                    }
                }
            }
        }

        private string GetGraphFieldString(string metadataSetGuid, string fieldGuid, string languageCode = "en", bool wrapInFunction = false)
        {
            string baseSearch = wrapInFunction ? "\"unique(value_{0}_{1}_{2}_ss)\"" : "value_{0}_{1}_{2}_ss";

            CFMetadataSet metadataSet = MetadataSrv.GetMetadataSet(metadataSetGuid);
            FormField field = metadataSet.Fields.Where(f => f.Guid.Equals(fieldGuid)).FirstOrDefault();

            if(field != null)
            {
                if (typeof(NumberField).IsAssignableFrom(field.GetType()))
                {
                    baseSearch = wrapInFunction ? "\"sum(field(value_{0}_{1}_is, max))\"" : "value_{0}_{1}_is";
                }
            }

            return string.Format(baseSearch, metadataSetGuid.Replace('-', '_'), fieldGuid.Replace('-', '_'), languageCode);
        }

        public IEnumerable<GraphQueryObject> GetGraphData(string q, string xMetadataSet, string xField, string yMetadataSet, string yField, string catMetadataSet, string catField, string languageCode = "en")
        {
            string xIndexId = GetGraphFieldString(xMetadataSet, xField, languageCode, false);
            string yIndexId = GetGraphFieldString(yMetadataSet, yField, languageCode, true);
            string catIndexId = string.IsNullOrEmpty(catField) ? null : GetGraphFieldString(catMetadataSet, catField, languageCode);

            string result = SolrSrv.GetGraphData(q, xIndexId, yIndexId, catIndexId);

            if (string.IsNullOrEmpty(result)) { return null; }

            IDictionary<string, string> categories = null;
            if (catIndexId != null)
            {
                categories = SolrSrv.GetSolrCategories(q, catIndexId, 0);
            }

            return ConvertSolrXml(result, categories);
        }

        public IEnumerable<GraphQueryObject> GetGraphData_old(string xMetadataSet, string xField, string yMetadataSet, string yField, string catMetadataSet, string catField, int xmin = 0, int xmax = 0)
        {
            CatfishDbContext db = new CatfishDbContext();
            xmin = xmin == 0 ? DateTime.MinValue.Year : xmin;
            xmax = xmax == 0 ? DateTime.Now.Year : xmax;
            //string xQuerySelect = "SELECT CAST(a.Year as VARCHAR(MAX)), CAST(SUM(a.Amount) as VARCHAR(MAX)), CAST(COUNT(*) as VARCHAR(MAX)), Category "
            //                 + " FROM( "
            //                 + " SELECT fields.value('(/item/metadata/metadata-set/fields/field[@guid=\"" + xField + "\"]/value/text/text())[1]', 'INT') AS Year,"
            //                 + " fields.value('(/item/metadata/metadata-set/fields/field[@guid=\"" + yField + "\"]/value/text/text())[1]', 'DECIMAL') AS Amount,"
            //                 + " fields.value('(/item/metadata/metadata-set/fields/Field[@guid=\"" + catField + "\"]/options/option[@selected=\"true\"]/text/text())[1]', 'VARCHAR(25)') AS Category "
            //                 + " FROM[dbo].[CFXmlModels] AS c "
            //                  + " CROSS APPLY c.Content.nodes('(/item/metadata/metadata-set[@guid=\"" + xMetadataSet + "\"]/fields/field[@guid=\"" + xField + "\" or @guid=\"" + yField + "\" or @guid=\"" + catField + "\"])') as T(fields) "
            //                  + " WHERE c.Discriminator = 'CFItem' "
            //                  + " AND fields.exist('number((/item/metadata/metadata-set/fields/field[@guid=\"" + xField + "\"]/value/text/text())[1])') = 1 "

            //                  + " ) as a "
            //                  + " GROUP BY a.Year, a.Category "
            //                  + " ORDER BY a.Year";
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


        public List<GroupByObject> GetGroupData(string q, string xMetadataSet, string xField, string yMetadataSet, string yField, string grpMetadataSet, string grpField, string languageCode = "en")
        {
            string xIndexId = GetGraphFieldString(xMetadataSet, xField, languageCode, false);
            string yIndexId = GetGraphFieldString(yMetadataSet, yField, languageCode, true);
            string catIndexId = string.IsNullOrEmpty(grpField) ? null : GetGraphFieldString(grpMetadataSet, grpField, languageCode);
           
            string solrResult = SolrSrv.GetGraphData(q, xIndexId, yIndexId, catIndexId, 50000);//xindex == catIndex -- if the catIndex is null sol don't return the 2nd grouping
                                                                                     //forexample 1st group by "Year", 2nd group by "Recipient"
                                                                                     //yIndexId -- is the field that can be sum() -- number field
                                                                                     //50000 -- limit item searched returned
                                                                                      
            List<GroupByObject> results = ConvertSolrXml2(solrResult).ToList();
            return results;
        }
    }
}