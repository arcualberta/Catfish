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
        public static void Main(string[] args)
        {
            string currDir = Environment.CurrentDirectory;
            currDir = Path.GetFullPath(Path.Combine(currDir, @"..\..\"));
            string dataDir =currDir + "Data\\StateFundingData17Jan2018.csv";
           
            try
            {
                Console.WriteLine("Data conversion is starting ...");
                headers = File.ReadLines(dataDir).First().Split(',');
                using (TextReader reader = File.OpenText(@dataDir))
                {
                    CsvReader csv = new CsvReader(reader);
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.MissingFieldFound = null;
                    CreateXmlFile(csv, currDir);
                  
                }
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public static void CreateXmlFile(CsvReader csv/*string[] input*/, string currDir)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8","yes"));
            string now = DateTime.Now.ToShortDateString();
          //  XNamespace xhtml = "http://www.w3.org/1999/xhtml";

            XElement ingestion = new XElement("ingestion");
            ingestion.Add(new XAttribute("overwrite", "false"));
            doc.Add(ingestion);
            XAttribute xmlLang = new XAttribute(XNamespace.Xml + "lang", "en"); 
             XAttribute xmlLangFr = new XAttribute(XNamespace.Xml + "lang", "fr"); 
            XAttribute xmlLangEs = new XAttribute(XNamespace.Xml + "lang", "es");
            XElement frEmpty = new XElement("text");
            frEmpty.Add(xmlLangFr);

            XElement esEmpty = new XElement("text");
            esEmpty.Add(xmlLangEs);

            XElement metadataSets = new XElement("metadata-sets");
            ingestion.Add(metadataSets);
           
            XElement metadataSet = new XElement("metadata-set");
            metadataSet.Add(new XAttribute("updated", now));
            metadataSet.Add(new XAttribute("created", now));
            metadataSet.Add(new XAttribute("model-type", "Catfish.Core.Models.MetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            metadataSet.Add(new XAttribute("IsRequired", "false"));
            string msGuid = Guid.NewGuid().ToString();
            metadataSet.Add(new XAttribute("guid", msGuid));
            metadataSets.Add(metadataSet);
   
            XElement msName = new XElement("Name");
            metadataSet.Add(msName);
          
            XElement text = new XElement("text", "StateFundingMetadataSet");
            text.Add(xmlLang);
            msName.Add(text);
           
            msName.Add(frEmpty);
           // EmptyText.re;
            msName.Add(esEmpty);

            XElement msDescription = new XElement("description");
            metadataSet.Add(msDescription);
            XElement textDesc =new XElement("text", "Metadata set that use by State Funding database");
            textDesc.Add(xmlLang);

            msDescription.Add(textDesc);
            msDescription.Add(frEmpty);
            msDescription.Add(esEmpty);
            //add metadata fields
            XElement fields = AddMetadataSetFields(doc, xmlLang, frEmpty, esEmpty); //header
            metadataSet.Add(fields);

            XElement entityTypes = AddEntityTypes(msGuid);

            ingestion.Add(entityTypes);

            XElement aggregations = AddAggregations(msGuid, csv);

            ingestion.Add(aggregations);
            
            doc.Save(currDir + "\\StateFundingIngestion17Jan2018.xml");
        }

        public static XElement AddMetadataSetFields(XDocument doc, XAttribute xmlLang, XElement frEmpty, XElement esEmpty)
        {
           
            XElement fields = new XElement("fields");

            int i = 1;
            foreach (string m in headers)
            {
                //from col 11-15 -- movement
                if (i < 11 || i > 15)
                {//text
                    XElement field = new XElement("field");
                    field.Add(new XAttribute("updated", DateTime.Now.ToShortDateString()));
                    field.Add(new XAttribute("created", DateTime.Now.ToShortDateString()));
                    if(i == 2 || i == 5)
                    {
                        field.Add(new XAttribute("model-type", "Catfish.Core.Models.NumberField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));

                    }
                    else
                    {
                        field.Add(new XAttribute("model-type", "Catfish.Core.Models.TextField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));

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
                    xmlname.Add(frEmpty);
                    xmlname.Add(esEmpty);
                    xmlname.Add(_nameVal);
                    field.Add(xmlname);
                    

                    XElement _description = new XElement("description");

                    XElement _descVal = new XElement("text", "");
                    _descVal.Add(xmlLang);
                    
                    _description.Add(_descVal);
                    _description.Add(frEmpty);
                    _description.Add(esEmpty);
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
            chkField.Add(new XAttribute("model-type", "Catfish.Core.Models.CheckBoxSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
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
            name.Add(frEmpty);
            name.Add(esEmpty);
            chkField.Add(name);

            XElement description = new XElement("description");

            XElement descVal = new XElement("text");
            descVal.Add(xmlLang);
            description.Add(descVal);
            description.Add(frEmpty);
            description.Add(esEmpty);

            chkField.Add(description);


            XElement options = new XElement("options");
            chkField.Add(options);
            fields.Add(chkField);
            for (int j=0; j < 5; j++)
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
                    optionVal = new XElement("text", "Other");
                else if (j == 3)
                    optionVal = new XElement("text", "Rights");
                else
                    optionVal = new XElement("text", "Women");

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
            XElement fieldname = new XElement("field-name", "governmentAgency");
           
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

         public static XElement AddAggregations(string msGuid, CsvReader csv)
        {
            XAttribute xmlLang = new XAttribute(XNamespace.Xml + "lang", "en");
            XAttribute xmlLangFr = new XAttribute(XNamespace.Xml + "lang", "fr");
            XAttribute xmlLangEs = new XAttribute(XNamespace.Xml + "lang", "es");
            XElement aggregations = new XElement("aggregations");

            //string[] headers = input[0].Split(',');
            while (csv.Read())
            {
                StateFunding sf = csv.GetRecord<StateFunding>();


                //for (int j = 1; j < input.Length; j++) //num of rows -- items --skip the header
                //{
                try
                {
                    //debug
                    //if (!string.IsNullOrEmpty(input[j]))
                    //{
                    XElement item = new XElement("item");
                    aggregations.Add(item);
                    string now = DateTime.Now.ToShortDateString();
                    item.Add(new XAttribute("created", now));
                    item.Add(new XAttribute("updated", now));
                    item.Add(new XAttribute("model-type", "Catfish.Core.Models.Item, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    item.Add(new XAttribute("IsRequired", "false"));
                    item.Add(new XAttribute("guid", Guid.NewGuid().ToString()));
                    item.Add(new XAttribute("entity-type", EntityTypeName));

                    XElement metadata = new XElement("metadata");
                    item.Add(metadata);
                    XElement ms = new XElement("metadata-set");
                    metadata.Add(ms);
                    ms.Add(new XAttribute("created", now));
                    ms.Add(new XAttribute("updated", now));
                    ms.Add(new XAttribute("model-type", "Catfish.Core.Models.MetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    ms.Add(new XAttribute("IsRequired", "false"));
                    ms.Add(new XAttribute("guid", msGuid));
                    XElement fields = new XElement("fields");
                    ms.Add(fields);



                    int i = 1;
                    int headerIdx = 0;
                    int fieldGuidIdx = 0;
                    //foreach (string m in contents)
                    List<string> movements = new List<string>();
                    foreach (PropertyInfo prop in typeof(StateFunding).GetProperties())
                    {

                        //from col 11-15 -- movement
                        if (i < 11 || i > 15)
                        {//te
                            XElement field = new XElement("field");
                            fields.Add(field);

                            field.Add(new XAttribute("updated", now));
                            field.Add(new XAttribute("created", now));
                            if (i == 2 || i == 5) //Year and amount --set to Number Field
                            {
                                field.Add(new XAttribute("model-type", "Catfish.Core.Models.NumberField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));

                            }
                            else
                            {
                                field.Add(new XAttribute("model-type", "Catfish.Core.Models.TextField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                            }
                            field.Add(new XAttribute("guid", FieldGuids.ElementAt(fieldGuidIdx)));
                            field.Add(new XAttribute("rank", i.ToString()));
                            field.Add(new XAttribute("page", "1"));

                            XElement name = new XElement("name");
                            field.Add(name);
                            XElement nameVal = new XElement("text", headers[headerIdx]);
                            nameVal.Add(xmlLang);
                            name.Add(nameVal);

                            XElement desc = new XElement("description");
                            field.Add(desc);
                            XElement descVal = new XElement("text", "");
                            descVal.Add(xmlLang);
                            desc.Add(descVal);

                            XElement value = new XElement("value");
                            field.Add(value);
                            XElement valtext = new XElement("text", prop.GetValue(sf, null));
                            valtext.Add(xmlLang);
                            value.Add(valtext);
                            fieldGuidIdx++;
                        }
                        else
                        {
                            movements.Add(prop.GetValue(sf, null).ToString());
                        }
                        i++;
                        headerIdx++;
                    }

                    //get the movement checkboxes
                    XElement chkEl = new XElement("Field");
                    chkEl.Add(new XAttribute("created", now));
                    chkEl.Add(new XAttribute("updated", now));
                    chkEl.Add(new XAttribute("model-type", "Catfish.Core.Models.Forms.CheckBoxSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    chkEl.Add(new XAttribute("IsReguired", "false"));
                    chkEl.Add(new XAttribute("guid", FieldGuids.Last()));
                    chkEl.Add(new XAttribute("rank", i.ToString()));
                    chkEl.Add(new XAttribute("page", "1"));
                    XElement movement = new XElement("name");
                    chkEl.Add(movement);
                    XElement movementVal = new XElement("text", "Movement");
                    movementVal.Add(xmlLang);
                    movement.Add(movementVal);

                    XElement moveDesc = new XElement("description");
                    XElement moveDescVal = new XElement("text");
                    moveDescVal.Add(xmlLang);
                    moveDesc.Add(moveDescVal);
                    chkEl.Add(moveDesc);

                    XElement options = new XElement("options");

                    chkEl.Add(options);
                    fields.Add(chkEl);
                    int optionInd = 0;
                    int k = 10;


                    foreach (string s in movements)//for (int k = 10; k < 15; k++) //col 11 to 15 
                    {

                        XElement option = new XElement("option");
                        string selected = string.IsNullOrEmpty(s) == true ? "false" : "true";
                        option.Add(new XAttribute("selected", selected));

                        option.Add(new XAttribute("guid", OptionGuids.ElementAt(optionInd)));
                        options.Add(option);
                        XElement optionVal;
                        if (k == 10)
                            optionVal = new XElement("text", "Aboriginal");
                        else if (k == 11)
                            optionVal = new XElement("text", "Environment");
                        else if (k == 12)
                            optionVal = new XElement("text", "Other");
                        else if (k == 13)
                            optionVal = new XElement("text", "Rights");
                        else
                            optionVal = new XElement("text", "Women");

                        optionVal.Add(xmlLang);
                        option.Add(optionVal);
                        optionInd++;
                        k++;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return aggregations;
        }
    }

    public class StateFunding
    {
        public string jurisdiction { get; set; }
        public string yearFunded { get; set; }
        public string recipientPA { get; set; }
        public string city { get; set; }
        public string amount { get; set; }
        public string governmentAgency { get; set; }
        public string FederalToJurisdiction { get; set; }
        public string program { get; set; }
        public string masterProgram { get; set; }
        public string project { get; set; }
        public string movementAboriginal { get; set; }
       
        public string movementEnvironment { get; set; }
        public string movementOther { get; set; }
        public string movementRights { get; set; }
        public string movementWomen { get; set; }

        public string source { get; set; }

        public string originalRecipientPA { get; set; }
    }
}
