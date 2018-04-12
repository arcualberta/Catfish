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
        public virtual FileOptions FileOptions
        {
            get
            {
                return GetChildModels(FileOptions.TagName).FirstOrDefault() as FileOptions;
            }
            set
            {
                RemoveAllElements("file-option", Data);
                InitializeFileOptions(value);
            }
        }

        [NotMapped]
        public virtual DataFile DataFile
        {
            get
            {
                return GetChildModels(DataFile.TagName).FirstOrDefault() as DataFile;
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

        private void InitializeFileOptions(FileOptions fileOptions)
        {
            Data.Add(fileOptions.Data);
        }

        public FileDescription()
        {
            DataFile = new DataFile();
            Label = "";
            FileOptions = new FileOptions();
        }
    }
}
