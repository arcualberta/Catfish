using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Catfish.GoogleApi.Services
{
    public class GoogleDriveService : IGoogleDriveService
    {
        public UserCredential UserCredential { get; private set; }

        private string _CredentialTokenFile;
        private DriveService _driveService;
        private SheetsService _sheetsService;
        private DocsService _docsService;

        public void Init(UserCredential credential, string appName)
        {
            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });
        }

        public void Init(string credentialsFile, string[] scopes, string appName)
        {

            using (var stream = new System.IO.FileStream(credentialsFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                _CredentialTokenFile = string.Format("token_{0}.json", 1);
                ////_CredentialTokenFile = string.Format("token_{0}.json", Guid.NewGuid());
                UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(_CredentialTokenFile, true)).Result;
            }


            // Create Drive API service.
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = appName,
            });

            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = appName
            });

            _docsService = new DocsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = UserCredential,
                ApplicationName = appName
            });
        }

        public File CreateFolder(string parentFolderId, string childFolderName)
        {
            File child = new File();
            child.Name = childFolderName;
            child.MimeType = "application/vnd.google-apps.folder";

            //set the parent folder Id -- which folder this file will be created/copy to
            child.Parents = new List<string> { parentFolderId };

            var resource = _driveService.Files.Create(child);

            var task = resource.ExecuteAsync();
            task.Wait(60000);

            var result = task.Result;

            return result;
        }

        public File Clone(string srcId, string outputFolderId, string cloneName)
        {
            File clone = new File()
            {
                Name = cloneName,
                Parents = new List<string> { outputFolderId },
                MimeType = "application/vnd.google-apps.spreadsheet"
            };

            return _driveService.Files.Copy(clone, srcId).Execute();
        }

        public Spreadsheet LoadSpreadSheet(string spreadSheetId)
        {
            return _sheetsService.Spreadsheets.Get(spreadSheetId).Execute();
        }

        public Document CreateDoc(string parentFolderId, string docName, List<string> content)
        {
            File file = new File()
            {
                Name = docName,
                Parents = new List<string> { parentFolderId },
                MimeType = "application/vnd.google-apps.document"
            };

            file = _driveService.Files.Create(file).Execute();

            Document doc = _docsService.Documents.Get(file.Id).Execute();


            List<Google.Apis.Docs.v1.Data.Request> requests = new List<Google.Apis.Docs.v1.Data.Request>();

            string text = string.Join("\n", content);
            var request = new Google.Apis.Docs.v1.Data.Request();
            request.InsertText = new InsertTextRequest()
            {
                Text = text,
                Location = new Location() { Index = 1 }
            };

            BatchUpdateDocumentRequest body = new BatchUpdateDocumentRequest();
            body.Requests = new List<Google.Apis.Docs.v1.Data.Request>() { request };
            _docsService.Documents.BatchUpdate(body, doc.DocumentId).Execute();

            return doc;
        }


        

        public SheetProperties DuplicateSheet(string spreadsheetId, string srcSheetName, string dstSheetName)
        {
            var spreadsheet = _sheetsService.Spreadsheets.Get(spreadsheetId).Execute(); ;
            var srcSheet = spreadsheet.Sheets.FirstOrDefault(sh => sh.Properties.Title == srcSheetName);
            if (srcSheet == null)
                throw new Exception(string.Format("Source sheet \"{0}\" not found in spreadsheet with ID \"{1}\"", srcSheetName, spreadsheetId));


            CopySheetToAnotherSpreadsheetRequest requestBody = new CopySheetToAnotherSpreadsheetRequest();
            requestBody.DestinationSpreadsheetId = spreadsheetId;

            SpreadsheetsResource.SheetsResource.CopyToRequest sheetDuplicateRequest = 
                _sheetsService.Spreadsheets.Sheets.CopyTo(requestBody, spreadsheetId, srcSheet.Properties.SheetId.Value);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            var newSheetProps = sheetDuplicateRequest.Execute();

            var sheetRenameRequest = new Google.Apis.Sheets.v4.Data.Request()
            {
                UpdateSheetProperties = new UpdateSheetPropertiesRequest
                {
                    Properties = new SheetProperties()
                    {
                        Title = dstSheetName,
                        SheetId = newSheetProps.SheetId

                    },
                    Fields = "Title"
                }
            };

            BatchUpdateSpreadsheetRequest updateRequest = new BatchUpdateSpreadsheetRequest();
            updateRequest.Requests = new List<Google.Apis.Sheets.v4.Data.Request>() { sheetRenameRequest };
            updateRequest.Requests.Add(sheetRenameRequest);
            var bur = _sheetsService.Spreadsheets.BatchUpdate(updateRequest, spreadsheetId);
            bur.Execute();

            return newSheetProps;
        }

    }
}
