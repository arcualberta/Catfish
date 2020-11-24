using Catfish.Core.Models;
using Catfish.Services;
using ElmahCore;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Catfish.Models.Fields
{
    [FieldType(Name = "Controlled Categories", Component = "controlled-categories")]
    public class ControlledCategoriesField : IField
    {
      
        public List<Keyword> AllowedCategories { get; set; }
        public StringField Vocabulary { get; set; } = new StringField();
        public StringField SelectedCategories { get; set; } = new StringField();
        
        public ControlledCategoriesField()
        {
            AllowedCategories = new List<Keyword>();
            Vocabulary = new StringField();
            SelectedCategories = new StringField();
        }
        public string GetTitle()
        {
            return "";
        }

        public void Init(IApi api, ICatfishSiteService csSrv, AppDbContext db, ErrorLog errorLog)
        {
            if (csSrv == null)
            {

                CatfishSiteService catSrv = new CatfishSiteService(api, db, errorLog);
                csSrv = catSrv;
            }
            var siteCategory = csSrv.getDefaultSiteCategoryAsync();

           // if (string.IsNullOrWhiteSpace(Vocabulary.Value))
           // {
                if (!string.IsNullOrWhiteSpace(siteCategory.Result))
                    Vocabulary.Value = siteCategory.Result;
           // }
            AllowedCategories = string.IsNullOrWhiteSpace(Vocabulary.Value)
                ? new List<Keyword>()
                : Vocabulary.Value.Split(",").Select(kw => new Keyword() { Label = kw }).ToList();

            if (SelectedCategories != null && !string.IsNullOrWhiteSpace(SelectedCategories.Value))
            {
                var selected = SelectedCategories.Value
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                foreach (var keyword in AllowedCategories)
                    keyword.Selected = selected.Contains(keyword.Label);
            }

        }
    }

   
}
