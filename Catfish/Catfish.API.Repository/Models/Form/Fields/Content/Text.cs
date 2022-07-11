namespace Catfish.API.Repository.Models.Form.Fields.Content
{
    public class Text
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Language code. e.g. "en", "fr", etc.
        /// </summary>
        public string? Language { get; set; }

        /// <summary>
        /// The text content (data) encapsulated in this Text object.
        /// </summary>
        public string? Value { get; set; }

    }
}
