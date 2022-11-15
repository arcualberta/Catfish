namespace Catfish.API.Repository.Models.Forms
{
    public class FormData
    {
        /// <summary>
        /// Unique FormData object ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Form definition ID
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Created timestamp.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Last updated timestamp.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// List of fields associated with the form in JSON serialized form-data object.
        /// </summary>
        [JsonIgnore]
        public string? SerializedFieldData { get; set; }

        /// <summary>
        /// List of field data objects associated with the form-data object.
        /// </summary>
        [NotMapped]
        public IList<object>? FieldData
        {
            get => SerializedFieldData == null ? null : JsonConvert.DeserializeObject<List<object>>(SerializedFieldData);
            set => SerializedFieldData = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public eState State { get; set; }
    }
}
