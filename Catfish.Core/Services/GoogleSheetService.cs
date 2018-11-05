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
using Catfish.Core.Models.Forms;

namespace Catfish.Core.Services
{
    public class GoogleSheetService: ServiceBase
    {
        protected static MemoryDataStore DataStore = new MemoryDataStore();

        protected SheetsService SheetsService { get; set; }
        protected Spreadsheet Spreadsheet { get; set; }

        private Dictionary<string, List<string>> mColumnHeadings;

        public GoogleSheetService(string spreadsheetId, CatfishDbContext db)
            :base(db)
        {
            mColumnHeadings = new Dictionary<string, List<string>>();
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
            mColumnHeadings.Clear();
        }

        public IEnumerable<string> GetSheetNames()
        {
            return Spreadsheet.Sheets.Select(s => s.Properties.Title);
        }

        public List<string> GetColumnHeadings(string sheetName)
        {
            if (!mColumnHeadings.ContainsKey(sheetName))
            {
                Sheet sheet = Spreadsheet.Sheets.Where(s => s.Properties.Title == sheetName).FirstOrDefault();
                var request = SheetsService.Spreadsheets.Values.Get(Spreadsheet.SpreadsheetId, "A1:1");
                var result = request.Execute();
                List<string> columnHeadings = result.Values.First().Select(h => h.ToString().Trim()).ToList();
                mColumnHeadings.Add(sheetName, columnHeadings);
            }

            return mColumnHeadings[sheetName];
        }

        public Form CreateForm(FormIngestionViewModel model)
        {
            try
            {

                model.ColumnHeadings = GetColumnHeadings(model.DataSheet);
                string lastColName = GetColumnName(model.ColumnHeadings.Count - 1);
                string dataRange = string.Format("A2:{0}", lastColName);

                Sheet sheet = Spreadsheet.Sheets.Where(s => s.Properties.Title == model.DataSheet).FirstOrDefault();
                var request = SheetsService.Spreadsheets.Values.Get(Spreadsheet.SpreadsheetId, dataRange);
                var result = request.Execute();

                int? listIdCol = string.IsNullOrEmpty(model.ListIdColumn) ? null : model.ColumnHeadings.IndexOf(model.ListIdColumn) as int?;
                int? blockIdCol = string.IsNullOrEmpty(model.BlockIdColumn) ? null : model.ColumnHeadings.IndexOf(model.BlockIdColumn) as int?;

                List<int> preReqColIndicies = model.PreContextColumns.Select(s => model.ColumnHeadings.IndexOf(s)).ToList();

                if (string.IsNullOrEmpty(model.QuestionColumn))
                    throw new Exception("Question column is not defined");
                int questionCol = model.ColumnHeadings.IndexOf(model.QuestionColumn);

                int? answerTypeCol = string.IsNullOrEmpty(model.AnswerTypeColumn) ? null : model.ColumnHeadings.IndexOf(model.AnswerTypeColumn) as int?;
                int? answerOptionsCol = string.IsNullOrEmpty(model.AnswerOptionsColumn) ? null : model.ColumnHeadings.IndexOf(model.AnswerOptionsColumn) as int?;


                Form form = new Form();

                foreach (var row in result.Values)
                {
                    var values = row.Select(s => s.ToString().Trim()).ToList();

                    int listNum = listIdCol.HasValue ? int.Parse(values[listIdCol.Value]) : 0;
                    int blockNum = blockIdCol.HasValue ? int.Parse(values[blockIdCol.Value]) : 0;
                    List<string> preContexts = preReqColIndicies.Select(i => values[i]).ToList();
                    string questionText = values[questionCol];

                    string answerType = answerOptionsCol.HasValue ? values[answerOptionsCol.Value] : "";
                    string answerOptions = answerOptionsCol.HasValue ? values[answerOptionsCol.Value] : "";

                    if (string.IsNullOrEmpty(answerType))
                    {
                        if (string.IsNullOrEmpty(answerOptions))
                            answerType = "TextField";
                        else
                            answerType = "RadioButtonSet";
                    }

                    CompositeFormField cf = new CompositeFormField();
                    foreach(string pc in preContexts)
                    {
                        HtmlField html = new HtmlField();
                        html.SetDescription(pc);
                        cf.InsertChildElement("./fields", html.Data);
                    }

                    FormField question = null;
                    if (answerType == "TextField")
                        question = new TextField();
                    else if (answerOptions == "RadioButtonSet")
                    {
                        List<string> optionStrings = answerOptions.Split(new char[] { '\n' }).Select(s => s.Trim()).ToList();
                        List<Option> options = new List<Option>();
                        foreach (var optVal in optionStrings)
                        {
                            Option opt = new Option();
                            opt.Value = new List<TextValue>() { new TextValue() { Value = optVal } };
                            options.Add(opt);
                        }

                        question = new RadioButtonSet()
                        {
                            Options = options
                        };
                    }
                    else
                        throw new Exception(string.Format("Answer type \"{0}\" is not implemented in survey form ingestion."));

                    question.SetName(questionText);
                    cf.InsertChildElement("./fields", question.Data);

                    form.InsertChildElement("./fields", cf.Data);
                }

                Db.FormTemplates.Add(form);
                //Db.SaveChanges();
                //return form;
            }
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }

            return null;
        }

        public static string GetColumnName(int zeroBasedColumnIndex)
        {
            const byte BASE = 'Z' - 'A' + 1;
            string name = String.Empty;
            do
            {
                name = Convert.ToChar('A' + zeroBasedColumnIndex % BASE) + name;
                zeroBasedColumnIndex = zeroBasedColumnIndex / BASE - 1;
            }
            while (zeroBasedColumnIndex >= 0);
            return name;
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

        public List<string> ColumnHeadings { get; set; } = new List<string>();

        [Display(Name = "Num Pre-contexts")]
        public int PreContextColumnCount { get; set; }

        public List<string> PreContextColumns { get; set; } = new List<string>();

        [Display(Name = "List Id")]
        public string ListIdColumn { get; set; }

        [Display(Name = "Block Id")]
        public string BlockIdColumn { get; set; }

        [Display(Name = "Question")]
        public string QuestionColumn { get; set; }

        [Display(Name = "Answer Type")]
        public string AnswerTypeColumn { get; set; }

        [Display(Name = "Answer Options")]
        public string AnswerOptionsColumn { get; set; }

        public string Button { get; set; }

        public string Error { get; set; } = null;
    }
}
