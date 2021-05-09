using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IServiceBuilder
    {
        public void Init(string credentialsFile, string[] scopes, string appName);
        public IGoogleDriveService CreateDriveService();
        public IGoogleSpreadsheetService CreateSheetService();
    }
}
