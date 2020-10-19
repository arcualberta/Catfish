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

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Controlled Vocabulary Search", Category = "Control", Component = "controlled-vocabulary-search", Icon = "fas fa-search")]
    public class ControlledVocabularySearchBlock : Block
    {

        [Field(Title = "Search Page Name", Placeholder = "The name of the type of Search Block to be created")]
        public StringField SearchPageName { get; set; }


        [Field(Title = "Vocab Css", Placeholder = "Css for each entry")]
        public StringField VocabCss { get; set; }


        [Field(Title = "Vocabulary List", Placeholder = "List of vocabulary terms to be used to create a the list of checkboxes")]
        public StringField VocabList { get; set; }


        [Field(Title = "Designated SolrField", Placeholder = "Solr Field")]
        public StringField DesignatedSolrField { get; set; }


        public TextField CssVal { get; set; }
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
