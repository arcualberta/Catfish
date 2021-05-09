using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IGoogleDriveService
    {
        public void Init(UserCredential credential, string appName);
        public void Init(string credentialsFile, string[] scopes, string appName);
        public File CreateFolder(string parentFolderId, string childFolderName);
        public File Clone(string srcId, string outputFolderId, string cloneName);
        public Google.Apis.Docs.v1.Data.Document CreateDoc(string parentFolderId, string docName, List<string> content);
        public Spreadsheet LoadSpreadSheet(string spreadSheetId);
        public SheetProperties DuplicateSheet(string spreadsheetId, string srcSheetName, string dstSheetName);
    }
}
