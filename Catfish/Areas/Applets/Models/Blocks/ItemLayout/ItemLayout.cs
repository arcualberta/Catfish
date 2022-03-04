using Catfish.Models.Blocks;
using Piranha.Extend;
using Piranha;
using Piranha.Extend.Fields;
using Catfish.Models.Fields;
using Catfish.Core.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catfish.Areas.Applets.Models.Blocks.ItemLayout
{
    [BlockType(Name = "Item Layout", Category = "Content", Component = "item-layout", Icon = "fas fa-server")]
    public class ItemLayout : Block , ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ItemLayout>();

        public TextField QueryParameter { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
        public TextField SelectedItemTemplateId { get; set; }
        
        [NotMapped]
        public List<ComponentLayout> ComponentTemplates { get; set; }
        public TextField SelectedComponents { get; set; }

        public ItemLayout()
		{
            ComponentTemplates = new List<ComponentLayout>()
            {
                new FieldLayout(),
                new StaticText()
            };
        }


    }
    //public enum DisplayType : int
    //{
    //    [Display(Description = "H1")]
    //    H1 = 0,
    //    [Display(Description = "H2")]
    //    H2 = 1,
    //    [Display(Description = "H3")]
    //    H3 = 2,
    //    [Display(Description = "H4")]
    //    H4= 3,
    //    [Display(Description = "H5")]
    //    H5 = 4,
    //    [Display(Description = "Div")]
    //    Div = 5,
    //    [Display(Description = "P")]
    //    P = 6,
    //    [Display(Description = "Img")]
    //    Img = 7,
    //    [Display(Description = "File")]
    //    File = 8
    //}

}
