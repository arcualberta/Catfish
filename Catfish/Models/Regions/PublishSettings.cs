using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;



namespace Catfish.Models.Regions
{
    public class PublishSettings
    {
        [Field(Title = "Publish Date Start")]
        public DateField PublishedStart { get; set; }

        [Field(Title = "Publish Date End")]
        public DateField PublishedEnd { get; set; }

        [Field(Title = "Custom Message", Placeholder ="Custom message to be displayed if the page is still not availabe.")]
        public StringField CustomMessage { get; set; }
    }
}
