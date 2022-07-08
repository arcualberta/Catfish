using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Regions
{
    public class Keywords
    {
        public List<string> keywords = new List<string>();

        [Field(Placeholder = "Event Title")]
        public List<CheckBoxField> Title { get; set; } = new List<CheckBoxField>();

        [Field(Title = "Api Key", Placeholder = "Google API key")]
        public StringField ApiKey { get; set; }

        [Field(Title = "Api Keyx", Placeholder = "Google API keyx")]
        public StringField ApiKeyx { get; set; }

        public Keywords()
        {
            Title.Add(new CheckBoxField());
            Title.Add(new CheckBoxField());
            Title.Add(new CheckBoxField());
        }

    }
}
