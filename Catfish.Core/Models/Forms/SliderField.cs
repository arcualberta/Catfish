using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Forms
{
    [TypeLabel("Slider field")]
    public class SliderField : NumberField
    {
        [NotMapped]
        public decimal Min
        {
            get
            {
                return GetAttribute("min", 0m);
            }
            set
            {
                SetAttribute("min", value);
            }
        }

        [NotMapped]
        public decimal Max
        {
            get
            {
                return GetAttribute("max", 0m);
            }
            set
            {
                SetAttribute("max", value);
            }
        }

        [NotMapped]
        public decimal Step
        {
            get
            {
                return GetAttribute("step", 1m);
            }
            set
            {
                SetAttribute("step", value);
            }
        }
        
        [NotMapped]
        public string MinLabel //TODO: Make TextValues
        {
            get
            {
                return GetAttribute("minLabel", Data);
            }

            set
            {
                SetAttribute("minLabel", value);
            }
        }

        [NotMapped]
        public string MaxLabel //TODO: Make TextValues
        {
            get
            {
                return GetAttribute("maxLabel", Data);
            }

            set
            {
                SetAttribute("maxLabel", value);
            }
        }
    }
}
