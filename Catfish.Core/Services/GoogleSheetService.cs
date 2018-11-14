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

                if (string.IsNullOrEmpty(model.ListIdColumn))
                    throw new Exception("Please identify the List ID column");
                int listIdCol = model.ColumnHeadings.IndexOf(model.ListIdColumn);

                if (string.IsNullOrEmpty(model.BlockIdColumn))
                    throw new Exception("Please identify the Block ID column");
                int blockIdCol = model.ColumnHeadings.IndexOf(model.BlockIdColumn);

                List<int> preContextColIndicies = model.PreContextColumns.Select(s => model.ColumnHeadings.IndexOf(s)).ToList();

                if (string.IsNullOrEmpty(model.QuestionColumn))
                    throw new Exception("Please idenfify the Question column");
                int questionCol = model.ColumnHeadings.IndexOf(model.QuestionColumn);

                if (string.IsNullOrEmpty(model.AnswerTypeColumn))
                    throw new Exception("Please idenfify the Answer Type column");
                int answerTypeCol = model.ColumnHeadings.IndexOf(model.AnswerTypeColumn);

                if (string.IsNullOrEmpty(model.AnswerOptionsColumn))
                    throw new Exception("Please idenfify the Answer Options column");
                int answerOptionsCol = model.ColumnHeadings.IndexOf(model.AnswerOptionsColumn);

                //Iterating through all the data and creating numbered lists and list of blocks, list of headers and list of footers
                //The key of each dictionary element represents the list where each block, header or foter belongs to.
                List<CompositeFormField> listFields = new List<CompositeFormField>();
                Dictionary<int, List<CompositeFormField>> blockFieldSets = new Dictionary<int, List<CompositeFormField>>();
                Dictionary<int, CompositeFormField> headers = new Dictionary<int, CompositeFormField>();
                Dictionary<int, CompositeFormField> footers = new Dictionary<int, CompositeFormField>();

                var uniqueListIds = result.Values.Select(x => x[listIdCol] as string).Distinct().ToList();
                foreach (var listId in uniqueListIds.Where(x => x != "*").Select(x =>int.Parse(x)))
                {
                    listFields.Add(new CompositeFormField() { Page = listId });
                    blockFieldSets.Add(listId, new List<CompositeFormField>());
                    headers.Add(listId, new CompositeFormField());
                    footers.Add(listId, new CompositeFormField());
                }

                //Iterating through all the data and creating blocks, headers, footers and questions
                foreach (var row in result.Values)
                {
                    List<string> values = row.Select(s => s.ToString().Trim()).ToList();

                    //Creating the question
                    List<string> preContexts = preContextColIndicies.Select(i => values[i]).ToList();
                    string questionText = values[questionCol];
                    string answerType = values[answerTypeCol];
                    string answerOptions = answerOptionsCol < values.Count ? values[answerOptionsCol] : "";

                    if (string.IsNullOrEmpty(answerType))
                    {
                        if (string.IsNullOrEmpty(answerOptions))
                            answerType = "ShortText";
                        else
                            answerType = "RadioButtonSet";
                    }

                    ///
                    /// Each precontext and question are represented by a composite field inside the selected block.
                    /// 
                    CompositeFormField surveyItem = new CompositeFormField();
                    foreach (string pc in preContexts)
                    {
                        if (!string.IsNullOrEmpty(pc.Trim()))
                        {
                            HtmlField html = new HtmlField();
                            html.SetDescription(pc);
                            surveyItem.InsertChildElement("./fields", html.Data);
                        }
                    }

                    FormField question = null;
                    if (answerType == "ShortText")
                        question = new TextField();
                    else if (answerType == "Number")
                    {
                        question = new NumberField();
                    }
                    else if (answerType == "Scale")
                    {
                        List<string> options = answerOptions.Split(new char[] { '|' }).Select(s => s.Trim()).ToList();
                        string[] begin = options[0].Split(new char[] { ':' });
                        string[] end = options[1].Split(new char[] { ':' });

                        question = new TextField();
                        //question = new Scale
                    }
                    else if (answerType == "RadioButtonSet")
                    {
                        List<string> optionStrings = answerOptions.Split(new char[] { '|' }).Select(s => s.Trim()).ToList();
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
                        throw new Exception(string.Format("Answer type \"{0}\" is not implemented in survey form ingestion.", answerType));

                    question.SetName(questionText);
                    surveyItem.InsertChildElement("./fields", question.Data);

                    //Adding the question to the header, footer or the appropriate block of the 
                    //selected list (or all the lists, if list number is *)
                    var listIdStr = values[listIdCol];
                    string blockIdStr = values[blockIdCol].ToUpper();
                    var applicableLists = listIdStr == "*" ? listFields : listFields.Where(x => x.Page == int.Parse(listIdStr));
                    foreach (var list in applicableLists)
                    {
                        if (blockIdStr == "H")
                        {
                            headers[list.Page].InsertChildElement("./fields", surveyItem.Data);
                        }
                        else if (blockIdStr == "F")
                        {
                            footers[list.Page].InsertChildElement("./fields", surveyItem.Data);
                        }
                        else
                        {
                            var blockSet = blockFieldSets[list.Page];
                            var block = blockSet.Where(x => x.Page == int.Parse(blockIdStr)).FirstOrDefault();
                            if (block == null)
                            {
                                block = new CompositeFormField() { Page = int.Parse(blockIdStr) };
                                blockSet.Add(block);
                            }
                            block.InsertChildElement("./fields", surveyItem.Data);
                        }
                    }
                }

                ///
                /// By this point, we have all "lists" in the listFields array and all "blocks" corresponding to 
                /// each of those lists in the blockFieldSets dictionary.
                /// 

                //Inserting all blocks into each list entry
                foreach (var list in listFields)
                {
                    List<CompositeFormField> blocks = blockFieldSets[list.Page];
                    foreach (var block in blocks)
                        list.InsertChildElement("./fields", block.Data);

                    if (headers[list.Page].Fields.Any())
                        list.InsertChildElement("./header", headers[list.Page].Data);

                    if (footers[list.Page].Fields.Any())
                        list.InsertChildElement("./footer", footers[list.Page].Data);
                }

                Form form = new Form();
                form.SetName(model.FormName);
                form.SetDescription(model.FormDescription);

                foreach (var field in listFields)
                {
                    form.InsertChildElement("./fields", field.Data);

                }

                form.Serialize();
                return form;







                //////    //If the block number is H or F, the question goes to the header or the footer section of a block.
                //////    if (blockIdCol.HasValue && values[blockIdCol.Value].ToUpper() == "H")
                //////    {
                //////        string lsitNum = listIdCol.HasValue ? values[listIdCol.Value] : "0";
                //////        if (!headers.ContainsKey(lsitNum))
                //////            headers.Add(lsitNum, new List<CompositeFormField>());

                //////    }
                //////    //Otherwise, this is a regular question.


                //////    string listNum = listIdCol.HasValue ? values[listIdCol.Value] : "0";
                //////    string blockNum = blockIdCol.HasValue ? values[blockIdCol.Value] : "0";
                //////    List<string> preContexts = preContextColIndicies.Select(i => values[i]).ToList();
                //////    string questionText = values[questionCol];

                //////    string answerType = answerOptionsCol.HasValue ? values[answerOptionsCol.Value] : "";
                //////    string answerOptions = answerOptionsCol.HasValue ? values[answerOptionsCol.Value] : "";

                //////    if (string.IsNullOrEmpty(answerType))
                //////    {
                //////        if (string.IsNullOrEmpty(answerOptions))
                //////            answerType = "TextField";
                //////        else
                //////            answerType = "RadioButtonSet";
                //////    }

                //////    ///
                //////    /// Each list is represented by a composite field at the top level of the form. 
                //////    ///The list number is stored in the "page" property of the field.
                //////    ///Get the composite field representing the given list number, or create a new one if it doesn't exist
                //////    ///
                //////    CompositeFormField list = listFields.Where(field => field.Page == int.Parse(listNum)).FirstOrDefault();
                //////    if(list == null)
                //////    {
                //////        list = new CompositeFormField() { Page = int.Parse(listNum) };
                //////        listFields.Add(list);
                //////        blockFieldSets.Add(listNum, new List<CompositeFormField>()); //Placehoder for blocks of this list.
                //////    }

                //////    ///
                //////    /// Each block is represented by a composite field in the "list". 
                //////    ///The block number is stored in the "page" propoerty of this composite field.
                //////    ///Get the composite field representinhg the give block number from the selected list. pr create a new one if it doesn't exist
                //////    ///
                //////    List<CompositeFormField> blocks = blockFieldSets[listNum];
                //////    CompositeFormField block = blocks.Where(field => field.Page == int.Parse(blockNum)).FirstOrDefault();
                //////    if(block == null)
                //////    {
                //////        block = new CompositeFormField() { Page = int.Parse(blockNum) };
                //////        blocks.Add(block);
                //////    }

                //////    ///
                //////    /// Each precontext and question are represented by a composite field inside the selected block.
                //////    /// 
                //////    CompositeFormField surveyItem = new CompositeFormField();
                //////    foreach(string pc in preContexts)
                //////    {
                //////        if(!string.IsNullOrEmpty(pc.Trim()))
                //////        {
                //////            HtmlField html = new HtmlField();
                //////            html.SetDescription(pc);
                //////            surveyItem.InsertChildElement("./fields", html.Data);
                //////        }
                //////    }

                //////    FormField question = null;
                //////    if (answerType == "TextField")
                //////        question = new TextField();
                //////    else if (answerOptions == "RadioButtonSet")
                //////    {
                //////        List<string> optionStrings = answerOptions.Split(new char[] { '\n' }).Select(s => s.Trim()).ToList();
                //////        List<Option> options = new List<Option>();
                //////        foreach (var optVal in optionStrings)
                //////        {
                //////            Option opt = new Option();
                //////            opt.Value = new List<TextValue>() { new TextValue() { Value = optVal } };
                //////            options.Add(opt);
                //////        }

                //////        question = new RadioButtonSet()
                //////        {
                //////            Options = options
                //////        };
                //////    }
                //////    else
                //////        throw new Exception(string.Format("Answer type \"{0}\" is not implemented in survey form ingestion."));

                //////    question.SetName(questionText);
                //////    surveyItem.InsertChildElement("./fields", question.Data);

                //////    block.InsertChildElement("./fields", surveyItem.Data);
                //////}

                ////////    ///
                ////////    /// By this point, we have all "lists" in the listFields array and all "blocks" corresponding to 
                ////////    /// each of those lists in the blockFieldSets dictionary.
                ////////    /// 

                ////////    //Inserting all blocks into each list entry
                ////////    foreach(var list in listFields)
                ////////    {
                ////////        List<CompositeFormField> blocks = blockFieldSets[list.Page.ToString()];
                ////////        foreach (var block in blocks)
                ////////            list.InsertChildElement("./fields", block.Data);
                ////////    }

                ////////    Form form = new Form();
                ////////    form.SetName(model.FormName);
                ////////    form.SetDescription(model.FormDescription);

                ////////    foreach (var field in listFields)
                ////////        form.InsertChildElement("./fields", field.Data);

                ////////    form.Serialize();
                ////////    return form;
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

        [Display(Name = "Name")]
        public string FormName { get; set; }

        [Display(Name = "Description")]
        public string FormDescription { get; set; }

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
