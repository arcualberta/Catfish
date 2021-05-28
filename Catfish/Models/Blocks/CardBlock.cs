using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Models.Blocks
{
    public enum ModalSize
    {
        [Display(Description = "Small")]
        Small,
        [Display(Description = "Large")]
        Large
    }

    public enum Positioning
    {
        [Display(Description = "Left")]
        Left,
        [Display(Description = "Right")]
        Right,
        [Display(Description = "Top")]
        Top,
        [Display(Description = "Bottom")]
        Bottom
    }

    public enum Styling
    {
        [Display(Description = "Simple")]
        Simple,
        [Display(Description = "Something")]
        Something,
    }

    [BlockType(Name = "Card Block", Category = "Content", Component = "card-block", Icon = "far fa-square")]
    public class CardBlock:Block
    {
        public ImageField Body { get; set; }
        public TextField CardTitle { get; set; }
        public TextField CardSubTitle { get; set; }
        public CheckBoxField HasAModal { get; set; }
        //public SelectField<Styling> CardStyling { get; set; }

        public SelectField<ModalSize> ModalSize { get; set; }
        public CheckBoxField IsModalCenteredOnTheScreen { get; set; }
        public ImageField ModalImage { get; set; }
        public SelectField<Positioning> ImagePosition { get; set; }
        public TextField ModalTitle { get; set; }
        public TextField ModalSubTitle { get; set; }
        public TextField EmailAddress { get; set; }
        public TextField ButtonLink { get; set; }
        public TextField ButtonText { get; set; }
        public CheckBoxField PreventUserFromExitingOnOutsideClick { get; set; }
        //public SelectField<Styling> ModalStyling { get; set; }

    }
}
