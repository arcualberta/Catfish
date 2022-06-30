﻿using Catfish.GoogleApi.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public class GoogleSheetsService : IGoogleSheetsService
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
            if (range.Values.Count < oneBasedRowIndex || range.Values[oneBasedRowIndex-1].Count < oneBasedColIndex)
                return defaultVal;

            return range.Values[oneBasedRowIndex-1][oneBasedColIndex-1] as string;
        }

        public string GetCellValue(ValueRange range, string col, int oneBasedRowIndex, string defaultVal)
        {
            return GetCellValue(range, oneBasedRowIndex, SheetHelper.Letter2Column(col.ToUpper()), defaultVal);
        }

        public Spreadsheet LoadSpreadSheet(string spreadSheetId)
        {
            return _sheetsService.Spreadsheets.Get(spreadSheetId).Execute();
        }

        public BatchGetValuesResponse LoadData(string spreadSheetId, IList<string> sheetNames, string range)
        {
            List<string> dataRanges = sheetNames.Select(sn => string.Format("{0}!{1}", sn, range)).ToList();
            var req = _sheetsService.Spreadsheets.Values.BatchGet(spreadSheetId);
            req.Ranges = new Google.Apis.Util.Repeatable<string>(dataRanges);
            return req.Execute();
        }

        public ValueRange LoadData(string spreadSheetId, string sheetName, string range)
        {
            string dataRange = string.Format("{0}!{1}", sheetName, range);
            SpreadsheetsResource.ValuesResource.GetRequest request = _sheetsService.Spreadsheets.Values.Get(spreadSheetId, dataRange);
            return request.Execute();
        }

        public object LoadSpreadsheet(string spreadsheetRef)
        {
            return spreadsheetRef;
        }

        public bool SaveSpreadsheet(object spreadsheet)
        {
            return true;
        }

        public object DuplicateSheet(object spreadsheetRef, string srcSheetName, string dstSheetName)
        {
            string spreadsheetId = spreadsheetRef as string;

            var spreadsheet = _sheetsService.Spreadsheets.Get(spreadsheetId).Execute(); ;
            var srcSheet = spreadsheet.Sheets.FirstOrDefault(sh => sh.Properties.Title == srcSheetName);
            if (srcSheet == null)
                throw new Exception(string.Format("Source sheet \"{0}\" not found in spreadsheet with ID \"{1}\"", srcSheetName, spreadsheetId));


            CopySheetToAnotherSpreadsheetRequest requestBody = new CopySheetToAnotherSpreadsheetRequest();
            requestBody.DestinationSpreadsheetId = spreadsheetId;

            SpreadsheetsResource.SheetsResource.CopyToRequest sheetDuplicateRequest =
                _sheetsService.Spreadsheets.Sheets.CopyTo(requestBody, spreadsheetId, srcSheet.Properties.SheetId.Value);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            var newSheetProps = sheetDuplicateRequest.Execute();

            var sheetRenameRequest = new Google.Apis.Sheets.v4.Data.Request()
            {
                UpdateSheetProperties = new UpdateSheetPropertiesRequest
                {
                    Properties = new SheetProperties()
                    {
                        Title = dstSheetName,
                        SheetId = newSheetProps.SheetId

                    },
                    Fields = "Title"
                }
            };

            BatchUpdateSpreadsheetRequest updateRequest = new BatchUpdateSpreadsheetRequest();
            updateRequest.Requests = new List<Google.Apis.Sheets.v4.Data.Request>() { sheetRenameRequest };
            updateRequest.Requests.Add(sheetRenameRequest);
            var bur = _sheetsService.Spreadsheets.BatchUpdate(updateRequest, spreadsheetId);
            bur.Execute();

            return newSheetProps;
        }

        public bool SetCellVal(object sheet, int row, int col, object val)
        {
            throw new NotImplementedException();
        }

        public object GetSheet(object spreadsheetRef, string workSheetName)
        {
            throw new NotImplementedException();
        }

        public object DeleteSheet(object spreadsheet, string sheetName)
        {
            throw new NotImplementedException();
        }
    }
}
