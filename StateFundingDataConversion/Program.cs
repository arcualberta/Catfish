using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using CsvHelper;
using System.Reflection;
//using Catfish.Core.Models;

namespace StateFundingDataConversion
{
    public class Program
    {
        private static List<string> FieldGuids = new List<string>();
        private static List<string> OptionGuids = new List<string>();
        private static string EntityTypeName = "Statefunding Entity Type";
        private static string[] headers;
        private static int TotAggregations = 10000;
        private static string inputFileName = "StateFundingDataFinalFall2018All.csv";

        public static void Main(string[] args)
        {
            string currDir = Environment.CurrentDirectory;
          //currDir = Path.GetFullPath(Path.Combine(currDir, @"..\..\"));
            string dataDir = currDir + "\\Data\\" + inputFileName;
            XDocument metadataSetStructure = null;

            // Use exisiting templates
            if(args.Length > 0)
            {
                metadataSetStructure = XDocument.Load(args[0]);
            }
           
            try
            {
                Console.WriteLine("Data conversion is starting ...");
                headers = File.ReadLines(dataDir).First().Split(',');
                using (TextReader reader = File.OpenText(@dataDir))
                {
                    // TODO: Use Excel reader
                    CsvReader csv = new CsvReader(reader);
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.MissingFieldFound = null;
                    CreateXmlFile(csv, currDir, metadataSetStructure);
                  
                }
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public static void CreateXmlFile(CsvReader csv/*string[] input*/, string currDir, XDocument metadataSetStructure)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8","yes"));
            string now = DateTime.Now.ToShortDateString();
          //  XNamespace xhtml = "http://www.w3.org/1999/xhtml";

            XElement ingestion = new XElement("ingestion");
            ingestion.Add(new XAttribute("overwrite", (metadataSetStructure != null).ToString()));
            doc.Add(ingestion);
            XAttribute xmlLang = new XAttribute(XNamespace.Xml + "lang", "en");

            XElement metadataSet;

            string msGuid;
            if (metadataSetStructure == null)
            {
                XElement metadataSets = new XElement("metadata-sets");
                ingestion.Add(metadataSets);

                metadataSet = new XElement("metadata-set");
                metadataSet.Add(new XAttribute("updated", now));
                metadataSet.Add(new XAttribute("created", now));
                metadataSet.Add(new XAttribute("model-type", "Catfish.Core.Models.CFMetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                metadataSet.Add(new XAttribute("IsRequired", "false"));
                msGuid = Guid.NewGuid().ToString();
                metadataSet.Add(new XAttribute("guid", msGuid));
                metadataSets.Add(metadataSet);

                XElement msName = new XElement("Name");
                metadataSet.Add(msName);

                XElement text = new XElement("text", "StateFundingMetadataSet");
                text.Add(xmlLang);
                msName.Add(text);

                XElement msDescription = new XElement("description");
                metadataSet.Add(msDescription);
                XElement textDesc = new XElement("text", "Metadata set that use by State Funding database");
                textDesc.Add(xmlLang);

                msDescription.Add(textDesc);
                //add metadata fields
                XElement fields = AddMetadataSetFields(doc, xmlLang); //header
                metadataSet.Add(fields);

                XElement entityTypes = AddEntityTypes(msGuid);

                ingestion.Add(entityTypes);
                doc.Save(currDir + "\\SFIngestionFinal2018-MSEntityType.xml");
            }
            else
            {
                metadataSet = metadataSetStructure.Root;
                msGuid = metadataSet.Attribute("guid").Value;
            }

            XElement aggregations = AddAggregations(msGuid, csv, currDir, metadataSet);

           // ingestion.Add(aggregations);
           
           // doc.Save(currDir + "\\StateFundingIngestion17Jan2018.xml");
        }

        public static XElement AddMetadataSetFields(XDocument doc, XAttribute xmlLang)
        {
           
            XElement fields = new XElement("fields");

            int i = 1;
            foreach (string m in headers)
            {
                //from col 11-15 -- movement
                if (i < 13 || i > 18)
                {//text
                    XElement field = new XElement("field");
                    field.Add(new XAttribute("updated", DateTime.Now.ToShortDateString()));
                    field.Add(new XAttribute("created", DateTime.Now.ToShortDateString()));
                    if(i == 2 || i == 4) //yearFunded and amount
                    {
                        field.Add(new XAttribute("model-type", "Catfish.Core.Models.Forms.NumberField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));

                    }
                    else
                    {
                        field.Add(new XAttribute("model-type", "Catfish.Core.Models.Forms.TextField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));

                    }
                    field.Add(new XAttribute("IsRequired", "false"));
                    string fguid = Guid.NewGuid().ToString();
                    FieldGuids.Add(fguid);
                    field.Add(new XAttribute("guid", fguid));
                    field.Add(new XAttribute("rank", i.ToString()));
                    field.Add(new XAttribute("page", "1"));


                    //set name/description
                    XElement xmlname = new XElement("name");

                    XElement _nameVal = new XElement("text", m);
                    _nameVal.Add(xmlLang);
                    xmlname.Add(_nameVal);
                    field.Add(xmlname);
                    

                    XElement _description = new XElement("description");

                    XElement _descVal = new XElement("text", "");
                    _descVal.Add(xmlLang);
                    
                    _description.Add(_descVal);
                    field.Add(_description);

                    fields.Add(field);

                }
                i++;
            }

            //add movement checkbox option
            //checkbox
            XElement chkField = new XElement("field");
            chkField.Add(new XAttribute("updated", DateTime.Now));
            chkField.Add(new XAttribute("created", DateTime.Now));
            chkField.Add(new XAttribute("model-type", "Catfish.Core.Models.Forms.CheckBoxSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            chkField.Add(new XAttribute("IsRequired", "false"));
            string chkGuid = Guid.NewGuid().ToString();
            FieldGuids.Add(chkGuid);
            chkField.Add(new XAttribute("guid", chkGuid));

            chkField.Add(new XAttribute("page", "1"));


            chkField.Add(new XAttribute("rank", i.ToString())); //need only set it once
                                                         //set name/description
            XElement name = new XElement("name");

            XElement nameVal = new XElement("text", "movement");
            nameVal.Add(xmlLang);

            name.Add(nameVal);
            chkField.Add(name);

            XElement description = new XElement("description");

            XElement descVal = new XElement("text");
            descVal.Add(xmlLang);
            description.Add(descVal);

            chkField.Add(description);


            XElement options = new XElement("options");
            chkField.Add(options);
            fields.Add(chkField);
            for (int j=0; j < 6; j++)
            {
                XElement option = new XElement("option");
                
                option.Add(new XAttribute("selected", "false"));
                string optGuid = Guid.NewGuid().ToString();
                OptionGuids.Add(optGuid);
                option.Add(new XAttribute("guid", optGuid));
                options.Add(option);
                XElement optionVal;
                if (j==0)
                  optionVal = new XElement("text","Aboriginal");
                else if(j==1)
                    optionVal = new XElement("text", "Environment");
                else if (j == 2)
                    optionVal = new XElement("text", "Rights");
                else if (j == 3)
                    optionVal = new XElement("text", "Women");
                else if (j == 4)
                    optionVal = new XElement("text", "Other");
                else
                    optionVal = new XElement("text", "Aboriginal Government");

                optionVal.Add(xmlLang);
                option.Add(optionVal);
            }
            return fields;
        }

        public static XElement AddEntityTypes(string msGuid)
        {
            XElement entityTypes = new XElement("entity-types");
            XElement eType = new XElement("entity-type");
            eType.Add(new XAttribute("id", "1"));
            entityTypes.Add(eType);
            XElement name = new XElement("name", EntityTypeName);
            eType.Add(name);
            XElement targetType = new XElement("target-type", "item");
            eType.Add(targetType);
            XElement mSets = new XElement("metadata-sets");
            XElement ms = new XElement("metadata-set");
            ms.Add(new XAttribute("ref", msGuid));
            mSets.Add(ms);
            eType.Add(mSets);

            XElement atrrMapping = new XElement("attribute-mapping");
            XElement mapName = new XElement("name", "Name Mapping");
            XElement fieldname = new XElement("field-name", "ministryAgency");
           
            atrrMapping.Add(mapName);
            atrrMapping.Add(fieldname);
            ms.Add(atrrMapping);

            XElement datrrMapping = new XElement("attribute-mapping");
            XElement mapDesc = new XElement("name", "Description Mapping");
            XElement fielddesc = new XElement("field-name", "recipient");
            
            datrrMapping.Add(mapDesc);
            datrrMapping.Add(fielddesc);
            ms.Add(datrrMapping);

            return entityTypes;
        }

         public static XElement AddAggregations(string msGuid, CsvReader csv, string currDir, XElement metadataSet)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
           
            XElement ingestion = new XElement("ingestion");
            ingestion.Add(new XAttribute("overwrite", "false"));
            doc.Add(ingestion);

            XAttribute xmlLang = new XAttribute(XNamespace.Xml + "lang", "en");
            XElement aggregations = new XElement("aggregations");

            int countAggregation = 1;
            int fileCount = 1;
            while (csv.Read())
            {
                StateFunding sf = csv.GetRecord<StateFunding>();
               
                try
                {
                   
                    XElement item = new XElement("item");
                    aggregations.Add(item);
                    string now = DateTime.Now.ToShortDateString();
                    item.Add(new XAttribute("created", now));
                    item.Add(new XAttribute("updated", now));
                    item.Add(new XAttribute("model-type", "Catfish.Core.Models.CFItem, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    item.Add(new XAttribute("IsRequired", "false"));
                    item.Add(new XAttribute("guid", Guid.NewGuid().ToString()));
                    item.Add(new XAttribute("entity-type", EntityTypeName));

                    XElement metadata = new XElement("metadata");
                    item.Add(metadata);
                    XElement ms = new XElement(metadataSet);
                    metadata.Add(ms);
                    ms.SetAttributeValue("created", now);
                    ms.SetAttributeValue("updated", now);
                    XElement fields = ms.Element("fields");

                    Action<XElement, string> setValue = (field, value) =>
                    {
                        XElement valueElement = field.Element("value");

                        if(valueElement == null)
                        {
                            valueElement = new XElement("value");
                            field.Add(valueElement);
                        }

                        XElement textValue = valueElement.Element("text");

                        if(textValue == null)
                        {
                            textValue = new XElement("text");
                            valueElement.Add(textValue);

                            textValue.Add(xmlLang);
                        }

                        textValue.Value = value;
                    };

                    foreach(XElement field in fields.Elements())
                    {
                        field.SetAttributeValue("updated", now);
                        string name = field.Element("name").Element("text").Value;

                        if(name == "jurisdiction")
                        {
                            setValue(field, sf.jurisdiction);
                        }
                        else if(name == "yearFunded")
                        {
                            setValue(field, sf.yearFunded);
                        }
                        else if (name == "recipient")
                        {
                            setValue(field, sf.recipient);
                        }
                        else if (name == "amount")
                        {
                            //TODO: add the inflation amount
                            setValue(field, sf.amount);
                        }
                        else if (name == "ministryAgency")
                        {
                            setValue(field, sf.ministryAgency);
                        }
                        else if (name == "city")
                        {
                            setValue(field, sf.city);
                        }
                        else if (name == "jurisdictionFederal")
                        {
                            setValue(field, sf.jurisdictionFederal);
                        }
                        else if (name == "source")
                        {
                            setValue(field, sf.source);
                        }
                        else if (name == "program")
                        {
                            setValue(field, sf.program);
                        }
                        else if (name == "masterProgram")
                        {
                            setValue(field, sf.masterProgram);
                        }
                        else if (name == "project")
                        {
                            setValue(field, sf.project);
                        }
                        else if (name == "recipientOriginal")
                        {
                            setValue(field, sf.recipientOriginal);
                        }
                        else if (name == "notes")
                        {
                            setValue(field, sf.notes);
                        }
                        else if (name == "movement")
                        {
                            var options = field.Element("options").Elements();

                            foreach (var option in options)
                            {
                                string optionName = option.Element("text").Value;

                                if(optionName == "Aboriginal Peoples")
                                {
                                    option.SetAttributeValue("selected", sf.movementAboriginal);
                                }
                                else if (optionName == "Environment")
                                {
                                    option.SetAttributeValue("selected", sf.movementEnvironment);
                                }
                                else if (optionName == "Human Rights")
                                {
                                    option.SetAttributeValue("selected", sf.movementRights);
                                }
                                else if (optionName == "Women")
                                {
                                    option.SetAttributeValue("selected", sf.movementWomen);
                                }
                                else if (optionName == "Other")
                                {
                                    option.SetAttributeValue("selected", sf.movementOther);
                                }
                                else if (optionName == "Aboriginal Government")
                                {
                                    option.SetAttributeValue("selected", sf.movementABgovt);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (countAggregation == TotAggregations) //save the file for every 10k items
                {
                    ingestion.Add(aggregations);
                    doc.Save(currDir + "\\SFundingIngestion-Aggregation-" + fileCount+ ".xml");
                    countAggregation = 1;
                    aggregations.RemoveAll();
                    ingestion.RemoveAll();
                    fileCount++;
                }
                else
                {
                   
                    countAggregation++;
                }
            }

            
            //write the reminding of the file
                ingestion.Add(aggregations);
                doc.Save(currDir + "\\SFundingIngestion-Aggregation-" + fileCount + ".xml");
                //countAggregation = 1;
               // aggregations.RemoveAll();
               // ingestion.RemoveAll();

            
            return aggregations;
        }
    }

    public class StateFunding
    {
        //the order is the same with the csv input headers

       // public string recordNumber { get; set; }
        public string jurisdiction { get; set; }
        public string yearFunded { get; set; }
        public string recipient { get; set; }
        public string amount { get; set; }
        public string ministryAgency { get; set; }
        public string city { get; set; }
        public string jurisdictionFederal { get; set; }
        public string source { get; set; }
        public string program { get; set; }
        public string masterProgram { get; set; }
        public string project { get; set; }
        public string recipientOriginal { get; set; }

        public string movementAboriginal { get; set; }
       
        public string movementEnvironment { get; set; }
        public string movementRights { get; set; }
        public string movementWomen { get; set; }
        public string movementOther { get; set; }
       
       
        public string movementABgovt{ get; set; }
        public string notes { get; set; }
    }
}
 