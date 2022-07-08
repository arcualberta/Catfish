using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public class GoogleDocsService : IGoogleDocsService
    {
        private DocsService _docsService;
        private DriveService _driveService;
        public void Init(UserCredential credential, string appName)
        {
            _docsService = new DocsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });
        }

        public string CreateDoc(string parentFolderId, string docName, List<string> content)
        {
            File file = new File()
            {
                Name = docName,
                Parents = new List<string> { parentFolderId },
                MimeType = "application/vnd.google-apps.document"
            };

            file = _driveService.Files.Create(file).Execute();

            Document doc = _docsService.Documents.Get(file.Id).Execute();

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

            return doc.DocumentId;
        }
    }
}
