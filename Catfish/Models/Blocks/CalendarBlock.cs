using System;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.AttributeBuilder;
using Piranha.Models;
using static Google.Apis.Calendar.v3.CalendarService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Catfish.Helper;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Piranha;


/*
    Please Note:
    This block will not show up/run properly without the file
    catfish2-0-GoogleCalendarServiceAccount.json
    present in the project. This file is in the gitignore and is not tracked.
    It helps to configure the Google Calendar access, so make sure you have it!
*/

namespace Catfish.Models.Blocks
{
    public enum CalendarStyles
    {
        [Display(Description = "Simple")]
        Simple,
        [Display(Description = "Rounded")]
        Rounded
    }

    public enum CalendarTypes: int
    {
        [Display(Description = "Do Not Display Calendar")]
        DoNotDisplayCalendar = 0,
        [Display(Description = "Regular Calendar")]
        RegularCalendar = 1,
        [Display(Description = "Weekly Strip")]
        WeeklyStrip = 2
    }

    [BlockType(Name = "Calendar Block", Category = "Content", Component = "calendar-block-vue", Icon = "fas fa-calendar-alt")] //calendar-block
    public class CalendarBlock : VueComponent, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<CalendarBlock>();

        public TextField CalendarId { get; set; }
        public NumberField DaysRangePast { get; set; }
        public NumberField DaysRangeFuture { get; set; }
        public NumberField MaxEvents { get; set; }
        public SelectField<CalendarTypes> DisplayCalendarUI { get; set; }

        public SelectField<CalendarStyles> CalendarStyle { get; set; }

        
        public CalendarBlock()
        {
        }

        // Initializes the field
        public async Task Init(ICatfishAppConfiguration config)
        {
           // string apiKey = config.GetGoogleCalendarAPIKey();

        }
        public string GetCalendarId()
        {
            if (CalendarId != null)
            {

                return CalendarId.Value;
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

        //public bool? GetDisplayCalendarUI()
        //{
        //    if (DisplayCalendarUI != null)
        //    {

        //        return DisplayCalendarUI;
        //    }
        //    return false; //default value
        //}

        public List<CalendarEvent> GetCalendarEvents()
        {
            List<CalendarEvent> CalendarEvents = new List<CalendarEvent>();
            string[] Scopes = { CalendarService.Scope.CalendarReadonly };
            string ApplicationName = "Google Calendar Block";
            
            string jsonFile = Startup.Configuration["GoogleCalendar:ServiceAccountFileName"]; //"catfish2-0-GoogleCalendarServiceAccount.json";
            string calendarId = GetCalendarId();

           

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

            Events events = request.Execute(); //this line is suuuuper slow, task gives up so thats why
            CalendarEvents = events.Items.Select(m => new CalendarEvent(m)).ToList();
             
            return CalendarEvents;
        }

    }//calendar block
    public class CatfishAppConfig
    {
        private readonly ICatfishAppConfiguration _catfishConfig;
        private readonly string ApiKey;

        public CatfishAppConfig(ICatfishAppConfiguration config)
        {
            _catfishConfig = config;
            //ApiKey = _catfishConfig.GetGoogleCalendarAPIKey();
        }

    }

    public class CalendarEvent
    {
        //private readonly ICatfishAppConfiguration _catfishConfig;
        //private readonly string ApiKey;

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
        public DateTime EndDateTime;
        public bool IsWholeDay = false;
        public List<EventAttachment> Attachments { get; }

        private static string YearFormat = "yyyy";
        private static string MonthFormat = "MMM";
        private static string DayFormat = "dd";
        private static string HourFormat = "h:mm tt";

        public CalendarEvent(Event currentEvent)
        {
            Summary = currentEvent.Summary;
            Description = currentEvent.Description;
            Location = currentEvent.Location;
            Attachments = new List<EventAttachment>();
            if (currentEvent.Attachments != null)
            {
                foreach (var attachment in currentEvent.Attachments)
                {
                    Attachments.Add(attachment);
                }
            }
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

    public enum GoogleCalendarPanelRenderMode
    {
        Default,
        Flat,
        Json
    }


}
