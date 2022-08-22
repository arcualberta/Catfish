
namespace CatfishWebExtensions.Blocks
{
    [BlockType(Name = "Google Calendar", Category = "Widgets", Icon = "fas fas-image")]
    public class GoogleCalendar : Block
    {
        [Display(Name = "API Key")]
        public StringField ApiKey { get; set; }

        [Display(Name = "Calendar IDs")]
        public StringField CalendarIds { get; set; }

    }
}
