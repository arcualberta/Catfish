using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Newtonsoft.Json;
using Catfish.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Core.Services
{
    public class GoogleSheetService: ServiceBase
    {
        protected static MemoryDataStore DataStore = new MemoryDataStore();

        protected SheetsService SheetsService { get; set; }
        protected Spreadsheet Spreadsheet { get; set; }

        public GoogleSheetService(string spreadsheetId, CatfishDbContext db)
            :base(db)
        {
            InitRead(spreadsheetId);
        }

        public void InitRead(string spreadsheetId)
        {
            //**** Reference 1: https://developers.google.com/sheets/api/quickstart/dotnet ****
            //**** Reference 1: https://www.twilio.com/blog/2017/03/google-spreadsheets-and-net-core.html ****

            string credentialFilePath = System.Configuration.ConfigurationManager.AppSettings["GoogleCredentialFilePath"];
            string credentialsFile = Path.Combine(credentialFilePath, System.Configuration.ConfigurationManager.AppSettings["GoogleCredentialFile"]);

            if (string.IsNullOrEmpty(credentialsFile))
                throw new Exception("Google API Credentials JSON file not defined in Web.config");

            else if(!File.Exists(credentialsFile))
                throw new Exception(string.Format("Google API Credentials JSON file \"{0}\"not found", credentialsFile));

            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
            string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string appName = "Catfish";

            var json = File.ReadAllText(credentialsFile);
            var cr = JsonConvert.DeserializeObject<Dictionary<string, string>>(json); // "personal" service account credential

            // Create an explicit ServiceAccountCredential credential
            var credentials = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(cr["client_email"])
            {
                Scopes = scopes
            }.FromPrivateKey(cr["private_key"]));


            // Create Google Sheets API service.
            SheetsService  = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = appName
            });

            // Get sheet names
            var request = SheetsService.Spreadsheets.Get(spreadsheetId);

            Spreadsheet = request.Execute();
        }

        public IEnumerable<string> GetSheetNames()
        {
            return Spreadsheet.Sheets.Select(s => s.Properties.Title);
        }

        public IEnumerable<string> GetColumnHeadings(string sheetName)
        {
            Sheet sheet = Spreadsheet.Sheets.Where(s => s.Properties.Title == sheetName).FirstOrDefault();
            var request = SheetsService.Spreadsheets.Values.Get(Spreadsheet.SpreadsheetId, "A1:1");
            var result = request.Execute();

            var headings = result.Values.First().Select(h => h.ToString());
            return headings;
        }

        public int CreateForm(FormIngestionViewModel model)
        {
            return 0;
        }
    }

    public class MemoryDataStore : IDataStore
    {
        [ThreadStatic]
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public Task ClearAsync()
        {
            data.Clear();

            return Task.Delay(0);
        }

        public Task DeleteAsync<T>(string key)
        {
            data.Remove(key);

            return Task.Delay(0);
        }

        public Task<T> GetAsync<T>(string key)
        {
            TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
            if (data.ContainsKey(key))
            {
                completionSource.SetResult((T)data[key]);
            }
            else
            {
                completionSource.TrySetResult(default(T));
            }

            return completionSource.Task;
        }

        public Task StoreAsync<T>(string key, T value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }

            return Task.Delay(0);
        }
    }

    public class FormIngestionViewModel
    {
        [Display(Name = "Sheet ID")]
        public string SpreadSheetId { get; set; }

        [Display(Name = "Data Sheet")]
        public string DataSheet { get; set; }

        [Display(Name = "Num Pre-contexts")]
        public int PreContextColumnCount { get; set; }

        [Display(Name = "List Id")]
        public int ListIdColumn { get; set; }

        [Display(Name = "Block Id")]
        public string BlockIdColumn { get; set; }

        public List<string> PreContextColumns { get; set; } = new List<string>();

        [Display(Name = "Question")]
        public string QuestionColumn { get; set; }

        [Display(Name = "Answer Type")]
        public string AnswerTypeColumn { get; set; }

        [Display(Name = "Answer Options")]
        public string AnswerOptionsColumn { get; set; }

        public string Button { get; set; }
    }
}
