namespace Catfish.API.Repository.DTO
{
    public class Option
    {
        public Guid Id { get; set; }

        public bool Selected { get; set; }

        public TextCollection OptionText { get; set; }

        public bool IsExtendedInput { get; set; }

        public bool IsExtendedInputRequired { get; set; }
    }
}
