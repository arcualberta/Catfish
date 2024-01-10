using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class SkippDataProcessing
    {
        public readonly TestHelper _testHelper;
        string _apiKey = "AIzaSyBb0mHe17jNMZbI4Ydei57t00ZpUvbvUoY";
        private readonly Guid FORM_ID = Guid.Parse("8d9a6bc9-863d-2ee8-ea93-d5544778f090");
        private readonly Guid NATION_OR_COMMUNITY = Guid.Parse("af27cd1f-4df1-fc15-4ef4-286eb2002816");
        private readonly Guid NAME = Guid.Parse("77ef4194-c2bf-686c-a05d-e3a63ab53750");
        private readonly Guid EMAIL = Guid.Parse("6c130003-7792-10c5-cdf5-372f8fe237bd");
        private readonly Guid POSITION = Guid.Parse("b0620da7-6bf1-5c9c-b70d-b96aadeb0413");
        private readonly Guid FACULTY = Guid.Parse("93f55bd0-8620-515e-411e-3abb2abf66e4");
        private readonly Guid ADDITIONAL_FACULTY = Guid.Parse("8f097f5a-d234-53e0-1399-968cf6ea2243");
        private readonly Guid PRONOUNS = Guid.Parse("bf33b1ab-ebd5-2452-e313-46a3f0f8d5eb");
        private readonly Guid DISABILITY = Guid.Parse("c27ebf23-8765-3e9f-a0eb-35553d71e9f0");
        private readonly Guid INDIGENOUS = Guid.Parse("3138944e-82e7-3ff0-831a-b17cc2c950fe");
        private readonly Guid COMMUNITIES_NATIONS_ORGANIZATIONS = Guid.Parse("be9ebb54-2971-0267-80b2-3d963b56b8a4");
        private readonly Guid LOCATION = Guid.Parse("a5582c7f-5c0b-b373-32ee-ad8584abe106");
        private readonly Guid ETHNICITY = Guid.Parse("71ae6003-556c-2ed0-d197-ff382ce75c45");
        private readonly Guid THEME = Guid.Parse("dca2335d-3c60-8785-6c07-c9e9079aa5c8");
        private readonly Guid GENDER_IDENTITY = Guid.Parse("f503da45-f9ec-6942-269d-45cdea413f84");
        private readonly Guid CURRENT_PROJECTS = Guid.Parse("020e9f66-911d-55dd-3eff-3bb3aec182ee");
        private readonly Guid INITIATION = Guid.Parse("03d1ffdd-0736-d8c8-418f-58691f22c19b");
        private readonly Guid ROLES = Guid.Parse("0b89ad9f-7a76-7594-b932-9a20bfa44b4b");
        private readonly Guid WEBSITE_LINKS = Guid.Parse("0b559321-498d-95b3-0734-2ae9ae876b93");
        private readonly Guid ADDITIONAL_KEYWORDS = Guid.Parse("aeaf264c-06e0-7bec-1073-832cbdb88bca");
        //private readonly Guid SOCIAL_MEDIA = Guid.Parse("");
        private readonly Guid ADITIONAL_CONTACT = Guid.Parse("260bedf2-ef68-c0be-be4b-e0e98ddcaf65");
        private readonly Guid UNDERSTAND = Guid.Parse("4e030a3d-818c-9c3a-00cf-6be47a8dfc6e");
        private readonly Guid CONSENT = Guid.Parse("a9ca2955-352d-e5f5-2901-3bcf0a2cb721");
        private readonly Guid AGREE = Guid.Parse("232a1241-949d-e430-7deb-cd3fa1011b72");
        private readonly string TIMESTAMP = "TimeStamp_dt";
        public SkippDataProcessing()
        {
            _testHelper = new TestHelper();
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.SkippDataProcessing.ImportData
        [Fact]
        public async Task ImportData()
        {
            int rowCount = 1;
            bool headerRow = true;
            string[] colHeadings = null;

            var solrService = _testHelper.Solr;

            List<SolrDoc> solrDocs = new List<SolrDoc>();

            foreach (RowData row in ReadGoogleSheet())
            {
                if (row.Values.Count == 0 || string.IsNullOrEmpty(row.Values[0].FormattedValue))
                    continue;

                if (headerRow)
                {
                    colHeadings = row.Values.Select(x => x.FormattedValue).ToArray();
                    headerRow = false;
                    continue;
                }

                SolrDoc solrDoc = new SolrDoc();
                solrDocs.Add(solrDoc);

                solrDoc.AddId(Guid.NewGuid().ToString());
                solrDoc.AddField("tenant_id_s", "a4a50d9f-fd20-4d74-8274-2acad28a6553");
                for (int i = 0; i < row.Values.Count; ++i)
                {
                    string colHeading = colHeadings[i];
                    string colValue = row.Values.ElementAt(i).FormattedValue;

                    if (string.IsNullOrEmpty(colValue))
                        continue;

                    if (colHeading == "Timestamp")
                    {
                        if(DateTime.TryParse(colValue, out DateTime timestamp))
                            solrDoc.AddField(TIMESTAMP, timestamp.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));

                    }
                    else if (colHeading == "Email Address")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{EMAIL}_t", colValue);
                    }
                    else if (colHeading == "Name")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{NAME}_ts", colValue);
                    }
                    else if (colHeading == "Additional Contact Information")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{ADITIONAL_CONTACT}_ts", colValue);
                    }
                    else if (colHeading == "Position")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{POSITION}_s", colValue);
                    }
                    else if (colHeading == "Faculty or Organizational Affiliation")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{FACULTY}_t", colValue);
                    }
                    else if (colHeading == "Additional Faculties or Organizations")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{ADDITIONAL_FACULTY}_ts", colValue);
                    }
                    else if (colHeading == "Pronouns")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{PRONOUNS}_t", colValue);
                    }
                    else if (colHeading == "Gender identity")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{GENDER_IDENTITY}_t", colValue);
                    }
                    else if (colHeading == "Indigenous Self-Declaration")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{INDIGENOUS}_t", colValue);
                    }
                    else if (colHeading == "Indigenous Nation and/or Community")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{NATION_OR_COMMUNITY}_t", colValue);
                    }
                    else if (colHeading == "Ethnicity")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{ETHNICITY}_t", colValue);
                    }
                    else if (colHeading == "Do you identify as living with disability")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{DISABILITY}_t", colValue);
                    }
                    else if (colHeading == "Where are they located")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{LOCATION}_ts", SplitWords(colValue, ";"));
                    }
                    else if (colHeading == "What theme(s) does your research fall under")
                    {
                        
                        solrDoc.AddField($"data_{FORM_ID}_{THEME}_ts", SplitWords(colValue, ","));
                    }
                    else if (colHeading == "Please briefly describe your current or recent research project(s)")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{CURRENT_PROJECTS}_ts", colValue);
                    }
                    else if (colHeading == "Research keywords")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{ADDITIONAL_KEYWORDS}_ts", SplitWords(colValue, ";"));
                    }
                    else if (colHeading == "Who initiated your research project")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{INITIATION}_ts", colValue);
                    }
                    else if (colHeading == "Do you have links to a professional website you would like to share")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{WEBSITE_LINKS}_ts", colValue);
                    }
                    else if (colHeading == "What role do community members play in your project")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{ROLES}_ts", colValue);
                    }
                    else if (colHeading == "understand")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{UNDERSTAND}_ts", colValue);
                    }
                    else if (colHeading == "consent")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{CONSENT}_ts", colValue);
                    }
                    else if (colHeading == "agree")
                    {
                        solrDoc.AddField($"data_{FORM_ID}_{AGREE}_ts", colValue);
                    }



                }
            }
            await solrService.Index(solrDocs);
            await solrService.CommitAsync();
        }
        private string[] SplitWords(string val, string seperator)
        {
          return val.Split(seperator, StringSplitOptions.RemoveEmptyEntries).Select(k => k.Trim()).ToArray();
        }
        public List<RowData> ReadGoogleSheet()
        {
            //https://docs.google.com/spreadsheets/d/e/2PACX-1vSPTFgPPcCiCngUPXFE8PdsOgxg7Xybq91voXFxHMFd4JpjUIZGLj7U_piRJZV4WZx3YEW31Pln7XV4/pubhtml => this is my own copy
            String spreadsheetId = "1A4BegCI9mqSkBCedJKVUBueW6u7hG6OLS0KszK5gLLE"; // "1m -oYJH-15DbqhE31dznAldB-gz75BJu1XAV5p5WJwxo";//==>google sheet Id
            String ranges = "A1:AC"; // "A2:AC";// read from col A to AI, starting 2rd row

            SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = "Google-Sheets",
                ApiKey = _apiKey
            });


            bool includeGridData = true;

            SpreadsheetsResource.GetRequest request = sheetsService.Spreadsheets.Get(spreadsheetId);
            request.Ranges = ranges;
            request.IncludeGridData = includeGridData;


            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.Spreadsheet response = request.Execute(); //await request.ExecuteAsync();


            // Read all the rows
            var values = response.Sheets[0].Data.Select(d => d).ToList();

            List<RowData> rows = values[0].RowData.ToList();

            return rows;
        }
        public static UserCredential GetCredential()
        {
            // TODO: Change placeholder below to generate authentication credentials. See:
            // https://developers.google.com/sheets/quickstart/dotnet#step_3_set_up_the_sample
            //
            // Authorize using one of the following scopes:
            //     "https://www.googleapis.com/auth/drive"
            //     "https://www.googleapis.com/auth/drive.file"
            //     "https://www.googleapis.com/auth/drive.readonly"
            //     "https://www.googleapis.com/auth/spreadsheets"
            //     "https://www.googleapis.com/auth/spreadsheets.readonly"
            return null;
        }
    }
}
