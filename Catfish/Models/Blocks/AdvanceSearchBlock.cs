using Catfish.Core.Models;
using Catfish.Models.Fields;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System.Linq;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Advance Search", Category = "Control", Component = "advance-search", Icon = "fas fa-search")]

    public class AdvanceSearchBlock : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<AdvanceSearchBlock>();
        public CatfishSelectList<Collection> Metadatasets { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }

        public TextField SelectedMetadataset { get; set; }

        public TextField SelectedItemTemplate { get; set; }

        public TextField SelectedRenderingType { get; set; }

        public TextAreaField SelectedTitleFieldId { get; set; }
        public TextAreaField SelectedFirstSubTitleFieldId { get; set; }
        public TextAreaField SelectedSecondSubTitleFieldId { get; set; }
        public TextAreaField SelectedBodyFieldId { get; set; }
        public TextAreaField SelectedFooterFieldId { get; set; }
        //Link to "More" in Description/body area
        public TextAreaField SelectedLinkFieldId { get; set; }

        //link can be link to Title
        public TextAreaField SelectedUrlFieldId { get; set; }
        public TextAreaField SelectedThumbnailFieldId { get; set; }

        public TextAreaField SelectedFieldList { get; set; }
        public NumberField MaxWords { get; set; } = 100;

        public string[] GetSelectedFieldList()
        {
            string[] fieldNames = new string[] { };
            if (SelectedFieldList.Value != null)
            {
                fieldNames = SelectedFieldList.Value.Split(new char[] { ',', '\n' }, System.StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();
            }

            return fieldNames;
        }
    }
}
