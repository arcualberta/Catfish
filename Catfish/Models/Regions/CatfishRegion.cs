using Piranha.Areas.Manager.Views.Shared.EditorTemplates;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Piranha;

namespace Catfish.Models.Regions
{
    public class CatfishRegion : Piranha.Extend.IExtension
    {
        [Display(Name = "CSS Id")]
        public string CssId { get; set; }

        [Display(Name = "CSS Classes")]
        public string CssClasses { get; set; }

        [Display(Name = "Styles")]
        [DataType(DataType.MultilineText)]
        public string CssStyles { get; set; }

        public virtual void Ensure(DataContext db)
        {
        }

        public virtual object GetContent(object model)
        {
            return this;
        }

        public virtual void Init(object model)
        {
        }

        public virtual void InitManager(object model)
        {
        }

        public virtual void OnManagerDelete(object model)
        {
        }

        public virtual void OnManagerSave(object model)
        {
        }
    }
}