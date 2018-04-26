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
    public class CFFileDescription : XmlModel
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
        public virtual CFFileOptions FileOptions
        {
            get
            {
                return GetChildModels(CFFileOptions.TagName).FirstOrDefault() as CFFileOptions;
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

        private void InitializeFileOptions(CFFileOptions fileOptions)
        {
            Data.Add(fileOptions.Data);
        }

        public CFFileDescription()
        {
            DataFile = new DataFile();
            Label = "";
            FileOptions = new CFFileOptions();
        }
    }
}
