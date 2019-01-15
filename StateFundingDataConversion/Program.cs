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
using Excel = Microsoft.Office.Interop.Excel;
//using Catfish.Core.Models;

namespace StateFundingDataConversion
{
    public class Program
    {
        public const string ACCESS = @"<access>
            <access-group updated=""2018-12-24 8:59:48 AM"" created=""2018-12-24 8:59:48 AM"" model-type=""Catfish.Core.Models.Access.CFAccessGroup, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"" IsRequired=""false"" guid=""32fecbad-468b-4aa6-9b45-cbd98b6cff45"" created-by-guid=""EF098994-C688-4397-8D41-4AE3E5EC6E1C"" created-by-name=""Mark McKellar"">
              <access-guid>00000000-0000-0000-0000-000000000001</access-guid>
              <access-definition updated = ""2018-12-24 8:59:48 AM"" created=""2018-12-24 8:59:48 AM"" model-type=""Catfish.Core.Models.Access.CFAccessDefinition, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"" IsRequired=""false"" guid=""de1885fc-1f3c-47be-a45d-21bac5cdf9f4"" created-by-guid=""EF098994-C688-4397-8D41-4AE3E5EC6E1C"" created-by-name=""Mark McKellar"">
                <access-modes>1</access-modes>
                <name>
                  <text xml:lang=""en"">Read</text>
                </name>
              </access-definition>
            </access-group>
          </access>";

        private const int MAX_YEAR = 2014;
        private static List<string> FieldGuids = new List<string>();
        private static List<string> OptionGuids = new List<string>();
        private static string EntityTypeName = "Statefunding Entity Type";
        private static int TotAggregations = 10000;
        private static string inputFileName = "StateFundingDatabase.xlsx";
        private static string inflationFileName = "InflationCalc.xlsx";
        private static IDictionary<int, decimal> AverageInflation;

        public static void Main(string[] args)
        {
            string currDir = Environment.CurrentDirectory;
            //currDir = Path.GetFullPath(Path.Combine(currDir, @"..\..\"));
            string inflationDir = currDir + "\\Data\\" + inflationFileName;
            string dataDir = currDir + "\\Data\\" + inputFileName;
            XDocument metadataSetStructure = null;

            // Use exisiting templates
            if(args.Length > 0)
            {
                metadataSetStructure = XDocument.Load(args[0]);
            }

            Console.WriteLine("Data conversion is starting ...");
            var app = new Excel.Application();
            var workbook = app.Workbooks.Open(@dataDir, 0, true);
            var worksheet = (Excel.Worksheet)workbook.Sheets[1];

            AverageInflation = GetAverageInflationTable(app, inflationDir, MAX_YEAR);
            CreateXmlFile(worksheet, currDir, metadataSetStructure);
            workbook.Close();
            //using (TextReader reader = File.OpenText(@dataDir))
            //{
            //    // TODO: Use Excel reader
            //    CsvReader csv = new CsvReader(reader);
            //    csv.Configuration.Delimiter = ",";
            //    csv.Configuration.MissingFieldFound = null;
            //    CreateXmlFile(csv, currDir, metadataSetStructure);
            //}
            Console.WriteLine("Done!");
           
        }

        protected static Dictionary<int, decimal> GetAverageInflationTable(Excel.Application app, string inflationFile, int maxDate)
        {
            Dictionary<int, decimal> calculated = new Dictionary<int, decimal>();
            decimal totalValue = 0.0m;

            var workbook = app.Workbooks.Open(inflationFile, 0, true);
            var worksheet = (Excel.Worksheet)workbook.Sheets[1];

            Excel.Range range = worksheet.UsedRange;
            int rowsCount = range.Rows.Count;

            for(int i = 2; i <= rowsCount; ++i)
            {
                var cells = (System.Array)worksheet.get_Range("A" + i, "D" + i).Cells.Value;
                int index = int.Parse(cells.GetValue(1, 1).ToString());
                decimal value = (decimal)(cells.GetValue(1, 4) == null ? 0.0 : (double)cells.GetValue(1, 4));

                if (index <= maxDate)
                {
                    totalValue += value;

                    calculated.Add(index, value);
                }
            }

            for(int i = 0; i < calculated.Keys.Count; ++i)
            {
                int key = calculated.Keys.ElementAt(i);
                decimal currentVal = calculated[key];
                totalValue -= currentVal;

                if (maxDate - key != 0)
                {
                    calculated[key] = totalValue / (maxDate - key);
                }
            }

            // Due to the calculation, all of the keys are 1 year behind. We now need to move them up a value.
            Dictionary<int, decimal> result = new Dictionary<int, decimal>();
            foreach(int key in calculated.Keys)
            {
                result.Add(key + 1, calculated[key]);
            }

            return result;
        }

        public static void CreateXmlFile(Excel.Worksheet worksheet/*CsvReader csv*/, string currDir, XDocument metadataSetStructure)
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

            XElement aggregations = AddAggregations(msGuid, worksheet, currDir, metadataSet);

           // ingestion.Add(aggregations);
           
           // doc.Save(currDir + "\\StateFundingIngestion17Jan2018.xml");
        }

        public static XElement AddMetadataSetFields(XDocument doc, XAttribute xmlLang)
        {
            string[] headers =
            {
                "recordNumber",
                "jurisdiction",
                "yearFunded",
                "recipient",
                "amount",
                "ministryAgency",
                "city",
                "jurisdictionFederal",
                "source",
                "program",
                "masterProgram",
                "project",
                "recipientOriginal",
                "movementAboriginal",
                "movementEnvironment",
                "movementRights",
                "movementWomen",
                "movementOther",
                "movementABgovt",
                "notes",
                ""
            };

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

        protected static StateFunding ReadRow(Excel.Range row)
        {
            
            StateFunding result = new StateFunding();

            try
            {
                var cells = (System.Array)row.Cells.Value;
                result.recordNumber = cells.GetValue(1, 1) as string;
                result.jurisdiction = cells.GetValue(1, 2) as string;
                result.yearFunded = cells.GetValue(1, 3).ToString();
                result.recipient = cells.GetValue(1, 4) as string;
                result.amount = cells.GetValue(1, 5).ToString();
                result.ministryAgency = cells.GetValue(1, 6) as string;
                result.city = cells.GetValue(1, 7) as string;
                result.jurisdictionFederal = cells.GetValue(1, 8) as string;
                result.source = cells.GetValue(1, 9) as string;
                result.program = cells.GetValue(1, 10) as string;
                result.masterProgram = cells.GetValue(1, 11) as string;
                result.project = cells.GetValue(1, 12) as string;
                result.recipientOriginal = cells.GetValue(1, 13) as string;
                result.movementAboriginal = cells.GetValue(1, 14) as string;
                result.movementEnvironment = cells.GetValue(1, 15) as string;
                result.movementRights = cells.GetValue(1, 16) as string;
                result.movementWomen = cells.GetValue(1, 17) as string;
                result.movementOther = cells.GetValue(1, 18) as string;
                result.movementABgovt = cells.GetValue(1, 19) as string;
                result.notes = cells.GetValue(1, 20) as string;

                // calculate inflation
                int year = int.Parse(result.yearFunded);
                double average = 1.0 + decimal.ToDouble(AverageInflation[year]) / 100.0;
                double inflatedAmount = double.Parse(result.amount) * Math.Pow(average, (double)(MAX_YEAR - year));
                result.amountInflation = string.Format("{0:0.00}", inflatedAmount);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading data:" + ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }

            return result;
        }

         public static XElement AddAggregations(string msGuid, Excel.Worksheet worksheet/*CsvReader csv*/, string currDir, XElement metadataSet)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
           
            XElement ingestion = new XElement("ingestion");
            ingestion.Add(new XAttribute("overwrite", "false"));
            doc.Add(ingestion);

            XAttribute xmlLang = new XAttribute(XNamespace.Xml + "lang", "en");
            XElement aggregations = new XElement("aggregations");

            int countAggregation = 1;
            int fileCount = 1;
            Excel.Range range = worksheet.UsedRange;
            int rowsCount = range.Rows.Count;

            string inflationField = "amount" + MAX_YEAR + "Inflation";

            for(int i = 2; i <= rowsCount; ++i)
            {
                StateFunding sf = ReadRow(worksheet.get_Range("A" + i, "T" + i));
               
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

                        if (valueElement == null)
                        {
                            valueElement = new XElement("value");
                            field.Add(valueElement);
                        }

                        XElement textValue = valueElement.Element("text");

                        if (textValue == null)
                        {
                            textValue = new XElement("text");
                            valueElement.Add(textValue);

                            textValue.Add(xmlLang);
                        }
                            
                        textValue.Value = value ?? "";
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
                        else if (name == inflationField)
                        {
                            setValue(field, sf.amountInflation);
                        }
                        else if (name == "movement")
                        {
                            var options = field.Element("options").Elements();

                            foreach (var option in options)
                            {
                                string optionName = option.Element("text").Value;

                                if(optionName == "Aboriginal Peoples")
                                {
                                    option.SetAttributeValue("selected", sf.movementAboriginal == "1");
                                }
                                else if (optionName == "Environment")
                                {
                                    option.SetAttributeValue("selected", sf.movementEnvironment == "1");
                                }
                                else if (optionName == "Human Rights")
                                {
                                    option.SetAttributeValue("selected", sf.movementRights == "1");
                                }
                                else if (optionName == "Women")
                                {
                                    option.SetAttributeValue("selected", sf.movementWomen == "1");
                                }
                                else if (optionName == "Other")
                                {
                                    option.SetAttributeValue("selected", sf.movementOther == "1");
                                }
                                else if (optionName == "Aboriginal Government")
                                {
                                    option.SetAttributeValue("selected", sf.movementABgovt == "1");
                                }
                            }
                        }
                    }

                    XElement access = XElement.Parse(ACCESS);
                    item.Add(access);
                }
                catch (Exception ex)
                {
                    if (sf != null)
                    {
                        Console.WriteLine(string.Format("An error occured while reading enty {0}: {1}", sf.recordNumber, ex.Message));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("An error occured reading line {0} of the excel file:", i, ex.Message));
                    }

                    Console.WriteLine(ex.StackTrace);
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

        public string recordNumber { get; set; }
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

        public string amountInflation { get; set; }
    }
}
 