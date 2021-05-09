using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Catfish.GoogleApi.Services
{
    public class GoogleApiServiceBuilder : IGoogleApiServiceBuilder
    {
        public UserCredential UserCredential { get; private set; }
        private string _CredentialTokenFile;
        private string _AppName;
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

            _AppName = appName;
        }

        public IGoogleDriveService CreateDriveService()
        {
            IGoogleDriveService service = new GoogleDriveService();
            service.Init(UserCredential, _AppName);
            return service;
        }

        public IGoogleSheetsService CreateSheetService()
        {
            IGoogleSheetsService service = new GoogleSheetsService();
            service.Init(UserCredential, _AppName);
            return service;
        }

        public IGoogleDocsService CreateDocsService()
        {
            IGoogleDocsService service = new GoogleDocsService();
            service.Init(UserCredential, _AppName);
            return service;
        }
    }
}
