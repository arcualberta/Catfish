namespace Catfish.API.Repository.Models.Form.Fields.Content
{
    public class Option: MultilingualText
    {
        /// <summary>
        /// Is this an option that would allow the user to type-in an addional values?
        /// </summary>
        public bool? IsExtendedOption { get; set; }
    }
}
