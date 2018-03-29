using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class FileDescription : XmlModel
    {

        public static string TagName { get { return "file-description"; } }     
        public override string GetTagName() { return TagName; }

        [NotMapped]
        public string Label
        {
            get
            {
                XElement labelElement = Data.Element("label");
                if (labelElement != null)
                {
                    return labelElement.Value;
                }
                return null;
            }

            set
            {
                Data.SetElementValue("label", value);
            }
        }

        [NotMapped]
        public virtual DataFile DataFile
        {
            get
            {
                // We are expecting a single DataFile but we are using
                // GetChildModels method because this is what is available.
                List<DataFile> dataFiles = GetChildModels(DataFile.TagName, Data).Select(c => c as DataFile).ToList();
                if (dataFiles.Count > 0)
                {
                    return dataFiles[0];
                }

                return null;
            }

            set
            {
                RemoveAllElements("file", Data); ;
                InitializeDataFile(value);
            }
        }

        private void InitializeDataFile(DataFile dataFile)
        {
            Data.Add(dataFile.Data);
        }

        public FileDescription()
        {
            DataFile = new DataFile();
            Label = "";
        }

        FileDescription (DataFile dataFile, string label = "")
        {            
            DataFile = dataFile;
            Label = label;
        }
    }
}
