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
    [FieldType(Name = "Controlled Keywords", Component = "controlled-keywords")]
    public class ControlledVocabularySelectorField : IField
    {
      
        public List<Keyword> AllowedKeywords { get; set; }
        public StringField Vocabulary { get; set; } = new StringField();
        public StringField SelectedKeywords { get; set; } = new StringField();

        public ControlledVocabularySelectorField()
        {
            AllowedKeywords = new List<Keyword>();
            Vocabulary = new StringField();
            SelectedKeywords = new StringField();
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
            var siteKeyword = csSrv.getDefaultSiteKeywordAsync();

           // if (string.IsNullOrWhiteSpace(Vocabulary.Value))
           // {
                if (!string.IsNullOrWhiteSpace(siteKeyword.Result))
                    Vocabulary.Value = siteKeyword.Result;
           // }
            AllowedKeywords = string.IsNullOrWhiteSpace(Vocabulary.Value)
                ? new List<Keyword>()
                : Vocabulary.Value.Split(",").Select(kw => new Keyword() { Label = kw }).ToList();

            if (SelectedKeywords != null && !string.IsNullOrWhiteSpace(SelectedKeywords.Value))
            {
                var selected = SelectedKeywords.Value
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                foreach (var keyword in AllowedKeywords)
                    keyword.Selected = selected.Contains(keyword.Label);
            }

        }
    }

    //public class Keyword
    //{
    //    public bool Selected { get; set; }
    //    public string Label { get; set; }
    //}
}
