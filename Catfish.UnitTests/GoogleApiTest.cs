using Catfish.Core.Models;
using Catfish.Test.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using Catfish.Core.Services;
using System.Threading;
using Google.Apis.Util.Store;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Catfish.UnitTests
{
    public class GoogleApiTest
    {
        private protected AppDbContext _db;
        private protected TestHelper _testHelper;
       

        string _apiKey = "";

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
          
             _apiKey = _testHelper.Configuration.GetSection("GoogleApiKey").Value;
        }
        
        [Test]

        public void MoveFileOnGoogleDrive()
        {
            string folderId = "1-4jX1pu9vBF5u9msMaj8F5lQEu6SQvMA";
            Google.Apis.Drive.v3.Data.File file = CopyFileFromGoogleDrive();
            DriveService driveService = GetService("../../Catfish.UnitTests", "../../client_secret_googledriveapi_desktop.json");

            //move to this folder

            //retrieve existing parents to remove
            FilesResource.GetRequest req = driveService.Files.Get(file.Id);
            req.Fields = "parents";
            Google.Apis.Drive.v3.Data.File fileToModified = req.Execute();
            string prevParents = string.Join(",", fileToModified.Parents);


           //modified content col1, row 1
           FilesResource.UpdateRequest updateRequest = driveService.Files.Update(new Google.Apis.Drive.v3.Data.File(), file.Id);


            updateRequest.Fields = "id, parents";
            updateRequest.RemoveParents = prevParents;
            updateRequest.AddParents=folderId;

            fileToModified = updateRequest.Execute();

            Assert.IsNotNull(fileToModified);
        }

        [Test]
        public void InsertRowsToGoogleSheet()
        {
            Google.Apis.Sheets.v4.SheetsService service = GetAuthorizeGoogleAppForSheetsService("../../Catfish.UnitTests", "../../client_secret_googledriveapi_desktop.json");
            string sheetId = "1H6bRriStvrViCoc2grVR8j041H5yngqz-yWaHk2lKxo";
            ValueRange getResponse = GetRange(service, sheetId);
            IList<IList<Object>> getValues = getResponse.Values;
            int currRowCount = getValues.Count() + 1;
            string newRange = "A" + currRowCount + ":A";

              IList<IList<Object>> objNeRecords = GenerateData();//new rows
            InsertRowsGoogleSheet(service, sheetId, objNeRecords,  newRange); //add new rows to the existing sheet
        }

        [Test]
        public void UpdateGoogleSheetCell()
        {
            Google.Apis.Sheets.v4.SheetsService service = GetAuthorizeGoogleAppForSheetsService("../../Catfish.UnitTests", "../../client_secret_googledriveapi_desktop.json");
            string sheetId = "*********REPLACEGOOGLESHEETID*************";//"1H6bRriStvrViCoc2grVR8j041H5yngqz-yWaHk2lKxo";
           
            String range = "Sheet1!B2";  // update cell B2 
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "COLUMNS";//"ROWS";//COLUMNS

            var oblist = new List<object>() { "Update content of B2" };

            UpdateGoogleSheetCell(service, sheetId, range, oblist);

            //valueRange.Values = new List<IList<object>> { oblist };
        }
        public Google.Apis.Drive.v3.Data.File CopyFileFromGoogleDrive()
        {
            string folderId = "1-4jX1pu9vBF5u9msMaj8F5lQEu6SQvMA"; // FolderId in google drive
            string originFileId = "1AYFEza6sOAcjhi4wvhbGoAMU_rR__ZNgA59AR6MfwNI"; //originalfileId in the google drive

            DriveService driveService = GetService("../../Catfish.UnitTests", "../../client_secret_googledriveapi_desktop.json");

            Google.Apis.Drive.v3.Data.File copiedFile = new Google.Apis.Drive.v3.Data.File();
          
            copiedFile.Name = "CopiedFileTest2";
            //set the parent folder Id -- which folder this file will be created/copy to
            copiedFile.DriveId = folderId;
            //copiedFile.Parents.Add(folderId);
           
            copiedFile = CopyGoogleFile(driveService, originFileId, "CopiedFileTest2"); //this will copy to the same drive/folder

            return copiedFile;
           
        }

        /// <summary>
        /// Get Google Drive Service
        /// </summary>
        /// <param name="client_secret_path"></param>
        /// <param name="client_secret_json_file"></param>
        /// <returns></returns>
        public DriveService GetService(string client_secret_path, string client_secret_json_file)
        {
            string[] Scopes = { DriveService.Scope.Drive};
           
            UserCredential credential;
          
            using (var stream = new FileStream(Path.Combine(client_secret_path, client_secret_json_file), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = @"c://temp"; 
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
        /// <summary>
        /// Copy an existing file.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="originFileId">ID of the origin file to copy.</param>
        /// <param name="copyTitle">Title of the copy.</param>
        /// <returns>The copied file, null is returned if an API error occurred</returns>
        public Google.Apis.Drive.v3.Data.File CopyGoogleFile(DriveService service, String originFileId, String copyTitle)
        {
            Google.Apis.Drive.v3.Data.File copiedFile = new Google.Apis.Drive.v3.Data.File();
            copiedFile.Name= copyTitle;
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


        public List<RowData> ReadGoogleSheet()
        {
            String spreadsheetId =  "*********REPLACEGOOGLESHEETID*************"; //"1YFS3QXGpNUtakBRXxsFmqqTYMYNv8bL-XbzZ3n6LRsI";//==>google sheet Id
            String ranges = "A2:Y";// read from col A to Y, starting 2nd row

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

        /// <summary>
        /// To upload a file to google drive -- TODO: TEST
        /// </summary>
        /// <param name="_service"></param>
        /// <param name="_uploadFile"></param>
        /// <param name="_parent"></param>
        /// <param name="_descrp"></param>
        /// <returns></returns>
        public Google.Apis.Drive.v3.Data.File uploadFile(DriveService _service, string _uploadFile, string _parent, string _descrp = "Uploaded with .NET!")
        {
            if (System.IO.File.Exists(_uploadFile))
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = _descrp;
                // body.MimeType = GetMimeType(_uploadFile);
                // body.Parents = new List<string> { _parent };// UN comment if you want to upload to a folder(ID of parent folder need to be send as paramter in above method)
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, "app/txt" /*GetMimeType(_uploadFile)*/);
                    request.SupportsTeamDrives = true;
                    // You can bind event handler with progress changed event and response recieved(completed event)
                    //   request.ProgressChanged += Request_ProgressChanged;
                    //  request.ResponseReceived += Request_ResponseReceived;
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message, "Error Occured");
                    return null;
                }
            }
            else
            {
                // MessageBox.Show("The file does not exist.", "404");
                return null;
            }
        }
        public static UserCredential GetCredential()
        {
            // TODO: Change placeholder below to generate authentication credentials. See:
            // https://developers.google.com/sheets/quickstart/dotnet#step_3_set_up_the_sample
            //
            // Authorize using one of the following scopes:
            string scope= "https://www.googleapis.com/auth/drive";
            //     "https://www.googleapis.com/auth/drive.file"
            //     "https://www.googleapis.com/auth/drive.readonly"
            // return "https://www.googleapis.com/auth/spreadsheets";
            //     "https://www.googleapis.com/auth/spreadsheets.readonly"

              return null;
        }

        private static SheetsService GetAuthorizeGoogleAppForSheetsService( string credential_path, string credentialFileName)
        {
            // If modifying these scopes, delete your previously saved credentials  
            // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json  
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "Google Sheets API";
            UserCredential credential;
            using (var stream =
               new FileStream(Path.Combine(credential_path, credentialFileName), FileMode.Open, FileAccess.Read))
            {

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("MyAppsToken")).Result;

            }

            // Create Google Sheets API service.  
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }
        //private void UpdateSpreadSheet(SheetsService service, string sheetId)
        //{
        //    // SpreadhSheetID of above created document.  
          
           
        //    string newRange = GetRange(service, sheetId);
        //    IList<IList<Object>> objNeRecords = GenerateData();
        //    UpdatGoogleSheetinBatch(objNeRecords, sheetId, newRange, service);
        //   // MessageBox.Show("done!");
        //}
        protected ValueRange GetRange(SheetsService service, string SheetId)
        {
            // Define request parameters.  
            String spreadsheetId = SheetId;
            String range = "Sheet1"; //Read the entire sheet "A:A";

            SpreadsheetsResource.ValuesResource.GetRequest getRequest =
                       service.Spreadsheets.Values.Get(spreadsheetId, range);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            ValueRange getResponse = getRequest.Execute();
            IList<IList<Object>> getValues = getResponse.Values;
            return getResponse;
        }

        private  IList<IList<Object>> GenerateData()
        {
            List<IList<Object>> objNewRecords = new List<IList<Object>>();
            int maxrows = 2;
            for (var i = 1; i <= maxrows; i++)
            {
                IList<Object> obj = new List<Object>();
                obj.Add("Data row value - " + i + "A");
                obj.Add("Data row value - " + i + "B");
                obj.Add("Data row value - " + i + "C");
                objNewRecords.Add(obj);
            }
            return objNewRecords;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="spreadsheetId"></param>
        /// <param name="values">rows to be inserted</param>
        /// <param name="newRange"></param>
        private  void InsertRowsGoogleSheet(SheetsService service, string spreadsheetId, IList<IList<Object>> values, string newRange)
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
        private void UpdateGoogleSheetCell(SheetsService service, string sheetId, string rangeCell, List<Object> values)
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
