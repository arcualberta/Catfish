namespace Catfish.API.Repository.Models.Form.Fields
{
    /// <summary>
    /// Base class for form fields.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Unique field ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Field name. Displayed in the rendered form.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Field description. Displayed in the rendered form.
        /// </summary>
        public string Description { get; set; } = "";

        public string? ModelType { get; set; }

        public Field()
        {
            ModelType = GetType().FullName;
        }
    }
}
