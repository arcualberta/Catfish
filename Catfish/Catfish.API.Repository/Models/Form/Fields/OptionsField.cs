
namespace Catfish.API.Repository.Models.Form.Fields
{
    public class OptionsField: Field
    {
        /// <summary>
        /// List of options available with this field.
        /// </summary>
        public IList<Option> Options { get; set; } = new List<Option>();

        /// <summary>
        /// If set to true, this Option Field will allow the user to enter new options that
        /// are not already in the list of provided options.
        /// </summary>
        public bool? IsExtensible { get; set; }
    }
}
