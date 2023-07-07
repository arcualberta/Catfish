﻿using Google.Apis.Auth.OAuth2;
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
        string _apiKey = "";
        public readonly Guid FORM_ID = Guid.Parse("8d9a6bc9-863d-2ee8-ea93-d5544778f090");
        public readonly Guid NATIONORCOMMUNITY = Guid.Parse("af27cd1f -4df1-fc15-4ef4-286eb2002816");
        public readonly Guid NAME = Guid.Parse("77ef4194-c2bf-686c-a05d-e3a63ab53750");
        public readonly Guid EMAIL = Guid.Parse("6c130003-7792-10c5-cdf5-372f8fe237bd");
        public readonly Guid POSITION = Guid.Parse("b0620da7-6bf1-5c9c-b70d-b96aadeb0413");
        public readonly Guid FACULTY = Guid.Parse("93f55bd0-8620-515e-411e-3abb2abf66e4");
        public readonly Guid ADDITIONAL_FACULTY = Guid.Parse("8f097f5a-d234-53e0-1399-968cf6ea2243");
        public readonly Guid PRONOUNS = Guid.Parse("bf33b1ab-ebd5-2452-e313-46a3f0f8d5eb");
        public readonly Guid DISABILITY = Guid.Parse("c27ebf23-8765-3e9f-a0eb-35553d71e9f0");
        public readonly Guid INDIGENOUS = Guid.Parse("3138944e-82e7-3ff0-831a-b17cc2c950fe_ts");
        public readonly Guid COMMUNITIESNATIONSORGANIZATIONS = Guid.Parse("be9ebb54-2971-0267-80b2-3d963b56b8a4_ss");
        public readonly Guid LOCATION = Guid.Parse("a5582c7f-5c0b-b373-32ee-ad8584abe106_ts");
        public readonly Guid ETHNICITY = Guid.Parse("71ae6003-556c-2ed0-d197-ff382ce75c45_ts");
        public readonly Guid THEME = Guid.Parse("dca2335d-3c60-8785-6c07-c9e9079aa5c8_ts");
        public readonly Guid GENDER_IDENTITY = Guid.Parse("f503da45-f9ec-6942-269d-45cdea413f84_ss");
        public readonly Guid CURRENTPROJECTS = Guid.Parse("020e9f66-911d-55dd-3eff-3bb3aec182ee_ts");
        public readonly Guid INITIATION = Guid.Parse("03d1ffdd-0736-d8c8-418f-58691f22c19b_ts");
        public readonly Guid ROLES = Guid.Parse("0b89ad9f-7a76-7594-b932-9a20bfa44b4b_ts");
        public readonly Guid WEBSITELINKS = Guid.Parse("0b559321-498d-95b3-0734-2ae9ae876b93_ts");
        public readonly Guid ADDITIONAL_KEYWORDS = Guid.Parse("aeaf264c-06e0-7bec-1073-832cbdb88bca_ts");
        public readonly Guid SOCIALMEDIA = Guid.Parse("");
        public readonly Guid ADITIONAL_CONTACT = Guid.Parse("260bedf2-ef68-c0be-be4b-e0e98ddcaf65");

        [Fact]
        public void ImportData()
        {
            int rowCount = 1;
            bool headerRow = true;
            string[] colHeadings = null;

        }
        public List<RowData> ReadGoogleSheet()
        {
            //https://docs.google.com/spreadsheets/d/e/2PACX-1vSPTFgPPcCiCngUPXFE8PdsOgxg7Xybq91voXFxHMFd4JpjUIZGLj7U_piRJZV4WZx3YEW31Pln7XV4/pubhtml => this is my own copy
            String spreadsheetId = "1x6CeEfZiZcGxtnmkoluaZ6GjUhSmutf5GjwjI6dMOyw"; // "1m -oYJH-15DbqhE31dznAldB-gz75BJu1XAV5p5WJwxo";//==>google sheet Id
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
