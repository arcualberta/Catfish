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
        private DriveService _driveService;

        public void Init(UserCredential credential, string appName)
        {
            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });
        }

        ////public void Init(string credentialsFile, string[] scopes, string appName)
        ////{

        ////    using (var stream = new System.IO.FileStream(credentialsFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        ////    {
        ////        // The file token.json stores the user's access and refresh tokens, and is created
        ////        // automatically when the authorization flow completes for the first time.
        ////        _CredentialTokenFile = string.Format("token_{0}.json", 1);
        ////        ////_CredentialTokenFile = string.Format("token_{0}.json", Guid.NewGuid());
        ////        UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        ////            GoogleClientSecrets.Load(stream).Secrets,
        ////            scopes,
        ////            "user",
        ////            CancellationToken.None,
        ////            new FileDataStore(_CredentialTokenFile, true)).Result;
        ////    }


        ////    // Create Drive API service.
        ////    _driveService = new DriveService(new BaseClientService.Initializer()
        ////    {
        ////        HttpClientInitializer = UserCredential,
        ////        ApplicationName = appName,
        ////    });

        ////    _sheetsService = new SheetsService(new BaseClientService.Initializer
        ////    {
        ////        HttpClientInitializer = UserCredential,
        ////        ApplicationName = appName
        ////    });

        ////    _docsService = new DocsService(new BaseClientService.Initializer
        ////    {
        ////        HttpClientInitializer = UserCredential,
        ////        ApplicationName = appName
        ////    });
        ////}

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


    }
}
