using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Access
{
    public enum AccessMode
    {
        None     = 0,
        Read     = 1,
        Write    = 2,
        Append   = 4,
        Control  = 8,
        Discover = 16
    }

    public class AccessDefinition : XmlModel
    {

        public AccessDefinition()
        {
            AccessModes = AccessMode.None;
        }

        public static string TagName
        {
            get
            {
                return "access-definition";
            }
        }

        public override string GetTagName() {
            return TagName;
        }

        [NotMapped]
        public AccessMode AccessModes
        {
            get
            {
                int modes = (int)AccessMode.None;
                XElement accessModesElement = Data.Element("access-modes");
                if (accessModesElement != null 
                    && Int32.TryParse(accessModesElement.Value, out modes))
                {                    
                    return (AccessMode)modes;                   
                }
                return AccessMode.None;
            }

            set
            {
                Data.SetElementValue("access-modes", (int) value);
            }
        }
        
        public List<AccessMode> AccessModesList
        {
            get
            {
                List<AccessMode> accessModes = new List<AccessMode>();

                foreach (AccessMode accessMode in Enum.GetValues(typeof(AccessMode)))
                {
                    if (HasMode(accessMode))
                    {
                        accessModes.Add((AccessMode)accessMode);
                    }
                }

                //XXX If we need to check agains AccessMode.None add here
                //if (accessModes.Count == 0)
                //{
                //    accessModes.Add(AccessMode.None);
                //}

                return accessModes;
            }            
        }
        
        public bool HasMode(AccessMode accessMode)
        {
            return (accessMode & AccessModes) == accessMode;
        }

        // Convenient synonym for HasMode method to make it clear it can test 
        // for multiple modes
        public bool HasModes(AccessMode accessMode)
        {
            return HasMode(accessMode);
        }
    }
}
