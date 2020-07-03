using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;
using static Google.Apis.Calendar.v3.CalendarService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Piranha.Entities;
  
namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "GoogleCalendarPanel")]
    [ExportMetadata("Name", "GoogleCalendarPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class GoogleCalendarPanel : CatfishRegion
    {

        [Display(Name = "Api key")]
        public string ApiKey { get; set; }

        [Display(Name = "Calendar id")]
        public string CalendarId { get; set; }
        [Display(Name = "Day range past")]
        public int DayRangePast { get; set; } = 0;

        [Display(Name = "Day range future")]
        public int DayRange { get; set; } = 10;

        [Display(Name = "Max events")]
        public int MaxEvents { get; set; } = 100;

        [Display(Name = "Show event name")]
        public bool ShowEventSummary { get; set; } = true;

        [Display(Name = "Show event description")]
        public bool ShowEventDescription { get; set; } = true;

        [Display(Name = "Show event location")]
        public bool ShowEventLocation { get; set; } = true;

        [Display(Name = "Show event start")]
        public bool ShowEventStart { get; set; } = true;

        [Display(Name = "Show event end")]
        public bool ShowEventEnd { get; set; } = true;

        [Display(Name = "Render mode")]
        public GoogleCalendarPanelRenderMode RenderMode { get; set; } 
            = GoogleCalendarPanelRenderMode.Default;

        public List<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

        public string CalendarEventsJson { get; set; } = "";

        private static string[] Scopes = { Scope.CalendarReadonly };

        //public bool CanShowEventSummary(Event calendarEvent) {
        //    return ShowEventSummary && calendarEvent.Summary != null;
        //}

        //public bool CanShowEventDescription(Event calendarEvent)
        //{
        //    return ShowEventDescription && calendarEvent.Description != null;
        //}

        //public bool CanShowEventLocation(Event calendarEvent)
        //{
        //    return ShowEventLocation && calendarEvent.Location != null;
        //}

        //public bool CanShowEventStart(Event calendarEvent)
        //{          
        //    return ShowEventStart && calendarEvent.Start != null;
        //}

        //public bool CanShowEventEnd(Event calendarEvent)
        //{
        //    return ShowEventEnd && calendarEvent.End != null;
        //}

        public override object GetContent(object model)
        {

            if (ApiKey != null)
            {
                try
                {
                
                string[] Scopes = { CalendarService.Scope.CalendarReadonly };
                string ApplicationName = "Google Calendar Panel";
                string credentialFilePath = System.Configuration.ConfigurationManager.AppSettings["GoogleCredentialFilePath"];
                string jsonFile = Path.Combine(credentialFilePath, System.Configuration.ConfigurationManager.AppSettings["GoogleCredentialFile"]);
               
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
                 
                EventsResource.ListRequest request = service.Events.List(CalendarId);

                request.TimeMin = DateTime.Now.AddDays(DayRangePast);
                request.TimeMax = DateTime.Now.AddDays(DayRange);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = MaxEvents;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                Events events = request.Execute();
                CalendarEvents = events.Items.Select(m => new CalendarEvent(m)).ToList();
                CalendarEventsJson = JsonConvert.SerializeObject(CalendarEvents);
                //EventsJson = JsonConvert.SerializeObject(Events);
               }
                catch (Exception ex)
                {
                }
            }

            return base.GetContent(model);
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

        public CalendarEvent(Event currentEvent) {
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
            } else if (currentEvent.End.Date != null)
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