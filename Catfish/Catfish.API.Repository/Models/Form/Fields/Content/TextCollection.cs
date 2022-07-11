namespace Catfish.API.Repository.Models.Form.Fields.Content
{
    public class TextCollection
    {
        /// <summary>
        /// Unique ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// List of Text values encapsulated in this Text Collecton object.
        /// </summary>
        public IList<Text> Value { get; set; } = new List<Text>();
    }
}
