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
        // Use 1 to add read modes to rest of values
        Write    = 2 | 1,
        Append   = 4 | 1,
        Control  = 8 | 1
    }

    public static class AccessModeMethods
    {
        public static bool HasMode(this AccessMode current, AccessMode accessMode)
        {
            return (accessMode & current) == accessMode;
        }

        public static List<AccessMode> AsList(this AccessMode mode)
        {
            List<AccessMode> accessModes = new List<AccessMode>();

            foreach (AccessMode accessMode in Enum.GetValues(typeof(AccessMode)))
            {
                if (accessMode != AccessMode.None && mode.HasMode(accessMode))
                {
                    accessModes.Add((AccessMode)accessMode);
                }
            }            

            return accessModes;
        }

        public static List<string> AsStringList(this AccessMode mode)
        {
            return mode.AsList().Select(m => m.ToString()).ToList();
        }
    }

    public class CFAccessDefinition : CFXmlModel
    {

        public CFAccessDefinition()
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

        public string StringAccessModesList
        {
            get
            {
                //List<AccessMode> accessModes = new List<AccessMode>();
                string strAccessModes = string.Empty;

                foreach (AccessMode accessMode in Enum.GetValues(typeof(AccessMode)))
                {
                    if (HasMode(accessMode))
                    {
                        // accessModes.Add((AccessMode)accessMode);
                        if (accessMode != AccessMode.None)
                            strAccessModes += accessMode.ToString() + ",";
                    }
                }


                if (!string.IsNullOrEmpty(strAccessModes))
                    strAccessModes = strAccessModes.Substring(0, strAccessModes.Length - 1);

                return string.Format("{0} - {1}", Name, strAccessModes);
            }
        }
    }
}
