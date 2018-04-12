using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Access
{
    public class AccessGroup : XmlModel
    {
        string TagName {
            get
            {
                return GetTagName();
            }
        }

        public override string GetTagName() { return "access-group"; }

        //AccessDefinition AccessDefinition;
        //List<string> AccessGuids;

        [NotMapped]
        public AccessDefinition AccessDefinition
        {
            get
            {
                List<AccessDefinition> accessDefinition = GetChildModels(AccessDefinition.TagName, Data).Select(c => c as AccessDefinition).ToList();
                if (accessDefinition.Count > 0)
                {
                    return accessDefinition[0];
                }

                return null;
            }
            set
            {

                RemoveAllElements("access-group", Data);                
                InitializeAccessDefinition(value);
            }
        }

        [NotMapped]
        public List<string> AccessGuids
        {
            get
            {
                try
                {
                    XElement accessGuidsElement = Data.Element("access-guids");
                 
                    List<XElement> accessGuidsElements = accessGuidsElement.Elements("access-guid").ToList();
                    List<string> accessGuids = new List<string>();

                    foreach (XElement accessGuidElement in accessGuidsElements)
                    {
                        accessGuids.Add(accessGuidsElement.Value);
                    }

                    return accessGuids;
                }
                catch
                {
                    return null;
                }

            }

            set
            {
                XElement accessGuids = Data.Element("access-guids");
                if (accessGuids == null)
                {
                    accessGuids = new XElement("access-guids");
                    Data.Add(accessGuids);
                }
                foreach (string accessGuid in value)
                {
                    XElement accessGuidElement = new XElement("access-guid");
                    accessGuidElement.Value = accessGuid;
                    accessGuids.Add(accessGuidElement);
                }
            }
        }

        private void InitializeAccessDefinition(AccessDefinition accessDefinition)
        {
            Data.Add(accessDefinition.Data);
        }

    }
}
