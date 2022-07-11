


namespace Catfish.API.Repository.Models.Form
{
    /// <summary>
    /// Defines a form that consists of a list of fields.
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Unique form ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Status of the form.
        /// </summary>
        public eStatus Status { get; set; }

        /// <summary>
        /// Form name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Form description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Created timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Last updated timestamp.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Stores the serialized list of field as a JSON string.
        /// </summary>
        public string? SerializedFields { get; set; }

        /// <summary>
        /// List of fields associated with the form.
        /// </summary>
        [NotMapped]
        public IList<Field> Fields { get; set; } = new List<Field>();
    }
}
