namespace Catfish.API.Repository.Models.Form.Fields
{
    public class TextField: IValueField
    {
        /// <summary>
        /// Should this text field support multiple langauges?
        /// </summary>
        bool? IsMultilingual { get; set; }

        /// <summary>
        /// Should this text field support multiple lines of input (paragraphs)?
        /// </summary>
        bool? IsMultiline { get; set; }


        /// <summary>
        /// The content format.
        /// </summary>
        public eContentFormat Format { get; set; }

    }
}
