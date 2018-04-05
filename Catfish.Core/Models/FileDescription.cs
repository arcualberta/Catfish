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
                List<FileOptions> fileOptions = GetChildModels(FileOptions.TagName, Data).Select(c => c as FileOptions).ToList();
                if (fileOptions.Count > 0)
                {
                    return fileOptions[0];
                }

                return null;
            }
            set
            {
                //XXX change to remove all children                
                RemoveAllElements("file-option", Data);
                InitializeFileOptions(value);
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

        //public FileDescription (DataFile dataFile, string label = "")
        //{            
        //    DataFile = dataFile;
        //    Label = label;
        //    FileOptions = new FileOptions();
        //}
    }
}
