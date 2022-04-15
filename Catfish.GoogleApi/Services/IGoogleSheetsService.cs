﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public interface IGoogleSheetsService : ISheetService
    {
        public void Init(UserCredential credential, string appName);
        public string GetCellValue(ValueRange range, int oneBasedRowIndex, int oneBasedColIndex, string defaultVal);
        public string GetCellValue(ValueRange range, string col, int oneBasedRowIndex, string defaultVal);
        public ValueRange LoadData(string spreadSheetId, string sheetName, string range);
        public BatchGetValuesResponse LoadData(string spreadSheetId, IList<string> sheetNames, string range);
        public Spreadsheet LoadSpreadSheet(string spreadSheetId);
        //public void WriteBatch(string spreadsheetId)
    }
}