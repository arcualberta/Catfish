using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface ISheetService
    {
        public bool DuplicateSheet(object spreadsheetRef, string srcSheetName, string dstSheetName);

        public object LoadSpreadsheet(string spreadsheetRef);
        public bool SaveSpreadsheet(object spreadsheet);
    }
}
