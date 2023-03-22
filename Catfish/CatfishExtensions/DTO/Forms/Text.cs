using CatfishExtensions.Constants;

namespace CatfishExtensions.DTO.Forms
{
    public class Text
    {
        public Guid Id { get; set; }
        public string? Value { get; set; }
        public eTextType TextType { get; set; }
        public string Lang { get; set; }
    }
}
