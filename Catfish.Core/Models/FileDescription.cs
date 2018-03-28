using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models
{
    public class FileDescription : XmlModel
    {

        public DataFile DataFile;
        //public string Label;

        public static string TagName { get { return "file-description"; } }     
        public override string GetTagName() { return TagName; }

        [NotMapped]
        public string Label
        {
            get
            {
                return Data.Element("label").Value;
            }

            set
            {
                Data.SetElementValue("label", value);
            }
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
