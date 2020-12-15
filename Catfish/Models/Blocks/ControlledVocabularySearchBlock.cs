using Catfish.Core.Models;
using Catfish.Services;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catfish.Core.Services.Solr;
using Catfish.Core.Models.Solr;
using SolrNet;
using Catfish.Models.Fields;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Controlled Vocabulary Search", Category = "Control", Component = "controlled-vocabulary-search", Icon = "fas fa-search")]
    public class ControlledVocabularySearchBlock : VueComponent
    {

        [Field(Title = "Search Block Title", Placeholder = "The name of the type of Search Block to be created")]
        public StringField SearchPageName { get; set; }


        [Field(Title = "Block Css", Placeholder = "Css for the root element of the block")]
        public StringField BlockCss { get; set; }

        [Field(Title = "Option CSS", Placeholder = "Css for each entry")]
        public StringField OptionCss { get; set; }


        [Field(Title = "Selected Keywords")]
        public StringField SelectedKeywords { get; set; }

        [Field(Title = "Selected Categories")]
        public StringField SelectedCategories { get; set; }

        [Field(Title = "Vocabulary Settings")]
        public ControlledKeywordsField VocabularySettings { get; set; }

        [Field(Title = "Category Settings")]
        public ControlledCategoriesField CategorySettings { get; set; }

        public TextField CssVal { get; set; }

        public ControlledVocabularySearchBlock()
        {
            VocabularySettings = new ControlledKeywordsField();
            SelectedKeywords = new StringField();
            SelectedCategories = new StringField();
            BlockCss = new StringField();
            SearchPageName = new StringField();
            CategorySettings = new ControlledCategoriesField();
        }
        public string GetCss()
        {
            if (CssVal != null)
            {
                return CssVal.Value;
            }

            return "";
        }
    }
}
