
namespace Catfish.API.Repository.Models.Forms
{
    /// <summary>
    /// Defines a form that consists of a list of fields.
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Unique form ID.
        /// </summary>
        [Key]
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
        /// List of fields associated with the form in JSON serialized form.
        /// </summary>
        [JsonIgnore]
        public string? SerializedFields { get; set; }

        /// <summary>
        /// List of fields associated with the form.
        /// </summary>
        [NotMapped]
        public IList<object>? Fields 
        {
            get => SerializedFields == null ? null : JsonConvert.DeserializeObject<List<object>>(SerializedFields);
            set => SerializedFields = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public ICollection<EntityTemplate> EntityTemplates { get; set; } = new List<EntityTemplate>();
    }
}
