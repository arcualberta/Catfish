using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;
using System;

namespace Catfish.Models.Regions
{
    public class PublishSettings
    {

        [Field(Title = "Start date", Options = FieldOption.HalfWidth)]
        public DateField StartDate { get; set; }

        [Field(Title = "Start time (HH:mm) 24-hour format", Options = FieldOption.HalfWidth)]
        public StringField StarTime { get; set; }

        [Field(Title = "End date", Options = FieldOption.HalfWidth)]
        public DateField EndDate { get; set; }

        [Field(Title = "End time (HH:mm) 24-hour format", Options = FieldOption.HalfWidth)]
        public StringField EndTime { get; set; }

        [Field(Title = "Message", Placeholder ="Custom message to be displayed if the page is still not availabe.")]
        public StringField Message { get; set; }

        [Field(Title = "Page Title Settings", Placeholder = "Show the title of the page as a heading")]
        public CheckBoxField ShowTitleOfPage { get; set; }
        public DateTime? Start => ToDateTime(StartDate, StarTime);
        public DateTime? End => ToDateTime(EndDate, EndTime);

        private DateTime? ToDateTime(DateField date, StringField time)
        {
            if (date.Value.HasValue)
            {
                DateTime dateTime = date.Value.Value;
                if (!string.IsNullOrWhiteSpace(time.Value))
                {
                    try
                    {
                        string[] hhmm = time.Value.Trim().Split(":");
                        dateTime = dateTime
                            .AddHours(int.Parse(hhmm[0]))
                            .AddMinutes(int.Parse(hhmm[1]));
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return dateTime;
            }
            else
                return null;
        }
    }
}
