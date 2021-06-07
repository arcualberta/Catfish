using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockGroupType(Name = "ExtendedColumn", Category = "Media", Icon = "fas fa-images")]
    [BlockItemType(Type = typeof(HtmlBlock))]
    public class ExtendedColumnBlock : BlockGroup
    {


        //This could be better, Id like to have it so it only shows the possible row options (limit to # items)
        public enum NumRows
        {
            [Display(Description = "1")]
            One,
            [Display(Description = "2")]
            Two,
            [Display(Description = "3")]
            Three,
            [Display(Description = "4")]
            Four,
            [Display(Description = "5")]
            Five,
            [Display(Description = "6")]
            Six,
            [Display(Description = "7")]
            Seven,
            [Display(Description = "8")]
            Eight,
            [Display(Description = "9")]
            Nine,
            [Display(Description = "10")]
            Ten
        }

        //This could be better, Id like to have it so options are limited to the number of items, 
        //but still 12 as max (ie don't show 8-12 as options if they only have 7 items)
        public enum NumItemsPerRow
        {
            [Display(Description = "1")]
            One,
            [Display(Description = "2")]
            Two,
            [Display(Description = "3")]
            Three,
            [Display(Description = "4")]
            Four,
            [Display(Description = "5")]
            Five,
            [Display(Description = "6")]
            Six,
            [Display(Description = "7")]
            Seven,
            [Display(Description = "8")]
            Eight,
            [Display(Description = "9")]
            Nine,
            [Display(Description = "10")]
            Ten,
            [Display(Description = "11")]
            Eleven,
            [Display(Description = "12")]
            Twelve
        }

        [Region]
        public SelectField<NumRows> NumberOfRows { get; set; }
        [Region]
        public SelectField<NumItemsPerRow> ItemsPerRow { get; set; }
        //[Region]
        //public SelectField<PageStyle> Style { get; set; }

        //public List<Block>Items = new List<Block>();
    }
}
