using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IGoogleApiServiceBuilder
    {
        public void Init(string credentialsFile, string[] scopes, string appName);
        public IGoogleDriveService CreateDriveService();
        public IGoogleSheetsService CreateSheetService();
        public IGoogleDocsService CreateDocsService();
    }
}
