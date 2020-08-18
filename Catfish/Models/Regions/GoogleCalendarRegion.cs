using Piranha.Extend;
using Piranha.Extend.Fields;
using Catfish.Helper;
using System;
using Google.Apis.Calendar.v3.Data;
using System.Collections.Generic;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using System.IO;
using AutoMapper.Configuration;

namespace Catfish.Models.Regions
{
    public class GoogleCalendarRegion
    {
       // private readonly ICatfishAppConfiguration _catfishConfig;
        [Field(Placeholder = "Event Title")]
        public StringField Title { get; set; }

        [Field(Title ="Api Key", Placeholder ="Google API key")]
        public StringField ApiKey { get; set; }
        [Field(Title = "Calendar Id", Placeholder ="Your public Calendar ID")]
        public StringField CalendarId { get; set; }
        [Field(Title = "Number of Past Days", Placeholder ="Number of days in past you wish to include, please put 0 if you don't want to include past events")]
        public NumberField DaysRangePast { get; set; }
        [Field(Title = "Number of Future Days", Placeholder ="Number of days in future, default is 30")]
        public NumberField DaysRangeFuture { get; set; }
        [Field(Title = "Max Number of Events", Placeholder ="Max number of events you want to retrieve")]
        public NumberField MaxEvents { get; set; }

        //public string JsonFileName { get; set; }
        public GoogleCalendarRegion() {
           
        }
       
        public string GetCalendarId()
        {
            if (CalendarId != null)
            {

                return CalendarId.Value;
            }
            return "";
        }
        public string GetApiKey()
        {
            if (ApiKey != null)
            {

                return ApiKey.Value;
            }
            return "";
        }
        public int? GetDayRangePast()
        {
            if (DaysRangePast != null)
            {

                return DaysRangePast.Value;
            }
            return 0; //default value
        }
        public int? GetDayRangeFuture()
        {
            if (DaysRangeFuture != null)
            {

                return DaysRangeFuture.Value;
            }
            return 30; //default value
        }
        public int? GetMaxEvents()
        {
            if (MaxEvents != null)
            {

                return MaxEvents.Value;
            }
            return 100; //default value
        }

       
        public List<CalendarEvent> GetCalendarEvents()
        {
            List<CalendarEvent> CalendarEvents = new List<CalendarEvent>();
            string[] Scopes = { CalendarService.Scope.CalendarReadonly};
            string ApplicationName = "Google Calendar Region";
            string jsonFile = Startup.Configuration["GoogleCalendar:ServiceAccountFileName"];//"catfish2-0-GoogleCalendarServiceAccount.json";
            string calendarId = GetCalendarId();

            try
            {

                ServiceAccountCredential credential;

                using (var stream =
                    new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
                {
                    var confg = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
                    credential = new ServiceAccountCredential(
                       new ServiceAccountCredential.Initializer(confg.ClientEmail)
                       {
                           Scopes = Scopes
                       }.FromPrivateKey(confg.PrivateKey));
                }

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });



             EventsResource.ListRequest request = service.Events.List(GetCalendarId());
             int pastDays = Convert.ToInt32(GetDayRangePast()) * -1;
             int futureDays = Convert.ToInt32(GetDayRangeFuture());
             request.TimeMin = DateTime.Now.AddDays(pastDays);
             request.TimeMax = DateTime.Now.AddDays(futureDays);
             request.ShowDeleted = false;
             request.SingleEvents = true;
             request.MaxResults = GetMaxEvents();
             request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

             Events events = request.Execute();
             CalendarEvents = events.Items.Select(m => new CalendarEvent(m)).ToList();
                   
            }catch(Exception ex)
            {
                throw ex;
            }

            return CalendarEvents;
        }
    }


    public class CalendarEvent
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string StartYear { get; set; }
        public string StartMonth { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; } = null;
        public string EndYear { get; set; }
        public string EndMonth { get; set; }
        public string EndDay { get; set; }
        public string EndTime { get; set; } = null;
        public DateTime StartDateTime;
        public DateTime? EndDateTime;
        public bool IsWholeDay = false;

        private static string YearFormat = "yyyy";
        private static string MonthFormat = "MMM";
        private static string DayFormat = "dd";
        private static string HourFormat = "h:mm tt";

        public CalendarEvent(Event currentEvent)
        {
            Summary = currentEvent.Summary;
            Description = currentEvent.Description;
            Location = currentEvent.Location;
            ParseStart(currentEvent);
            ParseEnd(currentEvent);
        }

        private void ParseStart(Event currentEvent)
        {
            bool hasDate = false;

            if (currentEvent.Start.DateTime != null)
            {
                StartDateTime = (DateTime)currentEvent.Start.DateTime;
                StartTime = StartDateTime.ToString(HourFormat);
                hasDate = true;
            }
            else if (currentEvent.Start.Date != null)
            {
                StartDateTime = DateTime.Parse(currentEvent.Start.Date);
                hasDate = true;
            }

            if (hasDate)
            {
                StartYear = StartDateTime.ToString(YearFormat);
                StartMonth = StartDateTime.ToString(MonthFormat);
                StartDay = StartDateTime.ToString(DayFormat);
            }
        }

        private void ParseEnd(Event currentEvent)
        {

            bool hasDate = false;
            DateTime endDateTime = DateTime.MinValue;

            if (currentEvent.End.DateTime != null)
            {
                endDateTime = (DateTime)currentEvent.End.DateTime;
                EndTime = endDateTime.ToString(HourFormat);
                hasDate = true;
            }
            else if (currentEvent.End.Date != null)
            {
                // If Date is not null for End it means we are in an event that 
                // lasts for full days. The end Day needs to be reduced by one 
                // to show only the days when the event take place

                endDateTime = DateTime.Parse(currentEvent.End.Date).AddDays(-1);

                if (endDateTime != StartDateTime)
                {
                    hasDate = true;
                }

            }

            if (hasDate)
            {
                EndDateTime = endDateTime;
                EndYear = endDateTime.ToString(YearFormat);
                EndMonth = endDateTime.ToString(MonthFormat);
                EndDay = endDateTime.ToString(DayFormat);
            }
        }
    }

}
