using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Forms
{
    public class FieldData
    {
        /** 
     *  Unique ID.
     * */
        public Guid Id { get; set; }

        /**
         * ID of the Field to which this FieldData object belongs.
         * */
        public Guid FieldId { get; set; }

        /**
         * Stores multilingual text values when this field-data object belongs to a 
         * multilingual text field (e.g. Short Answer, Paragraph, or Rich Text).
         * */
        public TextCollection[]? MultilingualTextValues { get; set; } = null;

        /**
         * Stores monolingual text values when this field-data object belongs to a 
         * monolingual text field such as Email, Integer, Decimal, etc.
         * */
        public Text[]? MonolingualTextValues { get; set; } = null;

        /** 
         * Stores the IDs of selected options when this field-data object belongs to an
         * option field such as Checkboxes, Radio Buttons, etc.
         * */
        public Guid[]? SelectedOptionIds { get; set; } = null;

        /**
         * Stores custom option values when this field-data object belongs to an option
         * field that allows custom option values values.
         * */
        public string[]? CustomOptionValues { get; set; } = null;

        /**
         * Stores extended option values when this field-data object belongs to an option
         * field that has some options which takes extended values.
         * */
        public ExtendedOptionValue[]? ExtendedOptionValues { get; set; } = null;

        /* storing attachment files if any */
        public FileReference[]? FileReferences { get; set; } = Array.Empty<FileReference>();

        public FieldData[]? CompositeFieldData { get; set; } = null;
    }
}
