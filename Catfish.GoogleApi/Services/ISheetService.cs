using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface ISheetService
    {
        public object DuplicateSheet(object spreadsheetRef, string srcSheetName, string dstSheetName);

        public object LoadSpreadsheet(string spreadsheetRef);
        public bool SaveSpreadsheet(object spreadsheet);
        public object GetSheet(object spreadsheetRef, string workSheetName);
        public bool SetCellVal(object sheet, int row, int col, object val);
    }
}
