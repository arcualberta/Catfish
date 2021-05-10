using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface ISheetService
    {
        public bool DuplicateSheet(string spreadsheetId, string srcSheetName, string dstSheetName);
    }
}
