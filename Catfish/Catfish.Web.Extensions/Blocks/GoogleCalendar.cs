
namespace CatfishWebExtensions.Blocks
{
    [BlockType(Name = "Google Calendar", Category = "Widgets", Icon = "fas fa-calendar-alt")]
    public class GoogleCalendar : Block
    {
        [Field(Title = "Css Class")]
        public StringField CssClass { get; set; }

        [Field(Title = "Calendar IDs", Description ="You could provide more than one id if you wish, please separate them by a comma")]
        public StringField CalendarIds { get; set; }

        public enum DisplayOptions
        {
            Calendar, List
        }

        [Field(Title = "Display Style")]
        public SelectField<DisplayOptions> DisplayStyle { get; set; } = new SelectField<DisplayOptions>();

        public string[] getCalendarIds()
        {
            if(CalendarIds.Value != null)
            {
                return CalendarIds.Value.Split(',');
            }
            return null;
        }

        public string getDisplayStyle()
        {
            if (DisplayStyle.Value == DisplayOptions.Calendar)
                return "listMonth";
            else
                return "dayGridMonth";
        }

    }
}
