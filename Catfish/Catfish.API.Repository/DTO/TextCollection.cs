namespace Catfish.API.Repository.DTO
{
    public class TextCollection
    {
        public Guid Id { get; set; }

    /**
     * List of Text values encapsulated in this Text Collecton object.
     * */
        public Text[] Values { get;set; }
    }
}
