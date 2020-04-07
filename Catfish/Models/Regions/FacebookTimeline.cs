using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Models.Regions
{
    public class FacebookTimeline
    {
        [Field(Title = "Title", Placeholder = "Facebook Timeline")]
        public StringField Title { get; set; }
        [Field(Title="Facebook Url",Placeholder ="Your Facebook public url")]
        public StringField FacebookUrl { get; set; }
        [Field(Title = "Width", Placeholder ="The widget's width")]
        public NumberField Width { get; set; }
        [Field(Title = "Height", Placeholder = "The widget's height")]
        public NumberField Height { get; set; }

        public FacebookTimeline() { }
    }
}
