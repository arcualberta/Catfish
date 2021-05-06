using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Catfish.Core.Services
{
    public interface IGoogleApiService
    {
        DriveService GetService(string client_secret_path, string client_secret_json);
       
        Google.Apis.Drive.v3.Data.File CopyGoogleFile(DriveService service, String originFileId, String copyTitle);

        Google.Apis.Drive.v3.Data.File MoveGoogleFile(DriveService service, String originFileId, String parentFolderId);
        List<RowData> ReadGoogleSheet(string googleApiKey, string sheetId, string range /* google sheet range to read*/);
        void InsertRowsGoogleSheet(SheetsService service, string spreadsheetId, IList<IList<Object>> values, string newRange);
        void UpdateGoogleSheetCell(SheetsService service, string sheetId, string rangeCell, List<Object> values);
    }

    public class GoogleApiService : IGoogleApiService
    {
        private readonly IHostEnvironment _hostEnvironment;
        static string[] Scopes = { DriveService.Scope.Drive };


        public GoogleApiService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public DriveService GetService(string client_secret_path, string client_secret_json_file)
        {
            //get Credentials from client_secret.json file     
            UserCredential credential;
           // string webRoot = _hostEnvironment.ContentRootPath;
        
            using (var stream = new FileStream(Path.Combine(client_secret_path, client_secret_json_file), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = @"c://temp"; //saved the credention on the local drive so don't need to authenticate to google every time
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }

            //create Drive API service.    
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveRestAPI-v3",
            });
            return service;
        }

       
        public Google.Apis.Drive.v3.Data.File CopyGoogleFile(DriveService service, String originFileId, String copyTitle)
        {
            Google.Apis.Drive.v3.Data.File copiedFile = new Google.Apis.Drive.v3.Data.File();
            copiedFile.Name = copyTitle;
            try
            {
                return service.Files.Copy(copiedFile, originFileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }

        public Google.Apis.Drive.v3.Data.File MoveGoogleFile(DriveService service, string originFileId, string parentFolderId)
        {
            //string folderId = "1-4jX1pu9vBF5u9msMaj8F5lQEu6SQvMA";
            //Google.Apis.Drive.v3.Data.File file = CopyFileFromGoogleDrive();
            //DriveService driveService = GetService("../../Catfish.UnitTests", "../../client_secret_googledriveapi_desktop.json");

            //move to this folder

            //retrieve existing parents to remove
            FilesResource.GetRequest req = service.Files.Get(originFileId);
            req.Fields = "parents";
            Google.Apis.Drive.v3.Data.File fileMoved = req.Execute();
            string prevParents = string.Join(",", fileMoved.Parents);


            //modified content col1, row 1
            FilesResource.UpdateRequest updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), originFileId);


            updateRequest.Fields = "id, parents";
            updateRequest.RemoveParents = prevParents;
            updateRequest.AddParents = parentFolderId;

            fileMoved = updateRequest.Execute();

            return fileMoved;
        }

        public List<RowData> ReadGoogleSheet(string googleApiKey, string sheetId, string range /* google sheet range to read*/)
        {
            String spreadsheetId = sheetId;//==>google sheet Id
            String ranges = range; // example: "A2:Y";// read from col A to Y, starting 2nd row

            SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = null,
                ApplicationName = "Google-Sheets",
                ApiKey = googleApiKey
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="spreadsheetId"></param>
        /// <param name="values">rows to be inserted</param>
        /// <param name="newRange">starting row number to insert: i.e: "A5:A" -- insert stating from row 5</param>
        public void InsertRowsGoogleSheet(SheetsService service, string spreadsheetId, IList<IList<object>> values, string newRange)
        {
            SpreadsheetsResource.ValuesResource.AppendRequest request =
              service.Spreadsheets.Values.Append(new ValueRange() { Values = values }, spreadsheetId, newRange);

            request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            var response = request.Execute();
        }

        /// <summary>
        /// Update google sheet cell
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sheetId"></param>
        /// <param name="rangeCell">range of the cell -- i.e: "Sheet1!B2"  -- Update col B row 2 of Sheet1</param>
        /// <param name="values">Value of the cell(s) to be updated</param>
        public void UpdateGoogleSheetCell(SheetsService service, string sheetId, string rangeCell, List<Object> values)
        {

            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "COLUMNS";//"ROWS";//COLUMNS

            valueRange.Values = new List<IList<object>> { values };
            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, sheetId, rangeCell);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse result2 = update.Execute();
        }

       
    }
}
