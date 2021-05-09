using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public class GoogleSpreadsheetService : IGoogleSpreadsheetService
    {
        private SheetsService _sheetsService;

        public void Init(UserCredential credential, string appName)
        {
            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });
        }

        public string GetCellValue(ValueRange range, int oneBasedRowIndex, int oneBasedColIndex, string defaultVal)
        {
            if (range.Values.Count < oneBasedRowIndex || range.Values[oneBasedRowIndex].Count < oneBasedColIndex)
                return defaultVal;

            return range.Values[oneBasedRowIndex-1][oneBasedColIndex-1] as string;
        }

        public string GetCellValue(ValueRange range, string col, int oneBasedRowIndex, string defaultVal)
        {
            return GetCellValue(range, oneBasedRowIndex, Letter2Column(col.ToUpper()), defaultVal);
        }

        public int Letter2Column(string col)
        {
            col = col.ToUpper();
            int column = 0, length = col.Length;
            for (var i = 0; i < length; i++)
            {
                column += (col[i] - 64) * (int) Math.Pow(26, length - i - 1);
            }
            return column;
        }

        public string Column2Letter(int col)
        {
            int temp;
            string letter = "";
            while (col > 0)
            {
                temp = (col - 1) % 26;
                letter = ((char)(temp + 65)) + letter;
                col = (col - temp - 1) / 26;
            }
            return letter;
        }

        public ValueRange LoadData(string spreadSheetId, string sheetName, string range)
        {
            string dataRange = string.Format("{0}!{1}", sheetName, range);
            SpreadsheetsResource.ValuesResource.GetRequest request = _sheetsService.Spreadsheets.Values.Get(spreadSheetId, dataRange);
            return request.Execute();
        }
    }
}
