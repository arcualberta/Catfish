using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Models.Access;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class AccessDefinitionsViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
       
        public List<AccessCheckBox> AccessModes { get; set; }
        public string[] SelectedAccessModes { get; set; }

        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        public AccessDefinitionsViewModel()
        {
            AccessModes = new List<AccessCheckBox>();
        }

    }

    public class AccessCheckBox
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }

        public AccessCheckBox()
        {
            Checked = false;
        }
    }

}