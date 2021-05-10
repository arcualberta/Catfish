using Catfish.GoogleApi.Helpers;
using Catfish.GoogleApi.Services;
using Catfish.Test.Helpers;
using Google.Apis.Drive.v3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Catfish.UnitTests
{
    public class CongressBillinTest
    {
        private protected TestHelper _testHelper;

        private readonly Dictionary<string, List<string>> _errorLog = new Dictionary<string, List<string>>();

        string _apiKey = "";

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _apiKey = _testHelper.Configuration.GetSection("GoogleApiKey").Value;
        }

        public void LogError(string sectionName, int rowNumber, string message)
        {
            LogError(sectionName, string.Format("Row {0}: {1}", rowNumber, message));
        }
        public void LogError(string sectionName, string message)
        {
            if (!_errorLog.ContainsKey(sectionName))
                _errorLog.Add(sectionName, new List<string>());

            _errorLog[sectionName].Add(message);
        }

        ////[Test]
        ////public void CreateEventOrderForms()
        ////{
        ////    string credentialFile = "_CatfishGoogleApiDesktopCredentials.json";
        ////    string[] scopes = new string[] { DriveService.Scope.Drive };

        ////    var serviceBuilder = _testHelper.GoogleApiServiceBuilder;
        ////    serviceBuilder.Init(credentialFile, scopes, "Congress 2021 Billing");

        ////    IGoogleDriveService driveService = serviceBuilder.CreateDriveService();
        ////    IGoogleSheetsService sheetsService = serviceBuilder.CreateSheetService();
        ////    IGoogleDocsService docsService = serviceBuilder.CreateDocsService();

        ////    string sheetNamePrefix = "Assoc";

        ////    string srcId = "1Rt6C2Unna5CLAhwLD6RROO_Vb5m2uCiQGRbhd2Rjez8";
        ////    string templateId = "1KcKj8b4NGJDVMTCoxztgGzmv9yc0kU5vi26lnEBHLFs";
        ////    string outputRootId = "16lxbYpBFufGKMbz6mgyfXs47C2WCh6z4";

        ////    var src = sheetsService.LoadSpreadSheet(srcId);

        ////    string outputFolderName = string.Format("Output_{0}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        ////    var outputFolder = driveService.CreateFolder(outputRootId, outputFolderName);

        ////    var assocSheetNames = src.Sheets
        ////        .Where(sh => sh.Properties.Title.StartsWith(sheetNamePrefix))
        ////        .Select(sh => sh.Properties.Title)
        ////        .ToList();

        ////    var srcDataSets = sheetsService.LoadData(srcId, assocSheetNames, "A:AZ");

        ////    for(int assocIndex = 0; assocIndex<assocSheetNames.Count; ++assocIndex)
        ////    {
        ////        var sheetName = assocSheetNames[assocIndex];
        ////        var range = srcDataSets.ValueRanges[assocIndex];

        ////        try
        ////        {
        ////            var assocNameNum = range.Values[0][1] as string; //B1
        ////            Assert.IsNotNull(assocNameNum);

        ////            var bookingCode = range.Values[2][1] as string; //B3
        ////            Assert.IsNotNull(bookingCode);

        ////            if (!bookingCode.StartsWith("21-"))
        ////                throw new Exception("Booking code must start with \"21-\"");

        ////            //Creating a clone of the template
        ////            var orderFormId = driveService.Clone(templateId, outputFolder.Id, bookingCode);
        ////            Assert.IsNotNull(orderFormId);

        ////            int fullDayPackageCount = 0;
        ////            int halfDayPackageCount = 0;
        ////            int additionalPackageCount = 0;

        ////            //Repeat from row #7 onwards (note the 0-basd index)
        ////            for (int r = 7; r < range.Values.Count; ++r)
        ////            {
        ////                string packageType = sheetsService.GetCellValue(range, "A", r, "");

        ////                if (packageType.ToLower().StartsWith("full-day av"))
        ////                    ++fullDayPackageCount;
        ////                else if (packageType.ToLower().StartsWith("half-day av"))
        ////                    ++halfDayPackageCount;
        ////                else if (packageType.ToLower().StartsWith("additional av"))
        ////                    ++additionalPackageCount;

        ////                bool status = false;
        ////                do
        ////                {
        ////                    try
        ////                    {
        ////                        status = sheetsService.DuplicateSheet(orderFormId, "DetailsTemplate", string.Format("R-{0}", r));
        ////                    }
        ////                    catch (Exception ex)
        ////                    {
        ////                        status = false;
        ////                        Thread.Sleep(60000);
        ////                    }
        ////                }
        ////                while (!status);
                        

        ////                if (string.IsNullOrEmpty(packageType))
        ////                    LogError(sheetName, r, "No package type");

        ////                string sessionId = sheetsService.GetCellValue(range, "C", r, ""); 
        ////                if (string.IsNullOrEmpty(sessionId))
        ////                    LogError(sheetName, r, "No session id");

        ////                string sesstionTitle = sheetsService.GetCellValue(range, "D", r, "");
        ////                if (string.IsNullOrEmpty(sesstionTitle))
        ////                    LogError(sheetName, r, "No session title");

        ////                string date = sheetsService.GetCellValue(range, "E", r, "");
        ////                if (string.IsNullOrEmpty(date))
        ////                    LogError(sheetName, r, "No session date");

        ////                string startTime = sheetsService.GetCellValue(range, "F", r, "");
        ////                if (string.IsNullOrEmpty(startTime))
        ////                    LogError(sheetName, r, "No start time");

        ////                string endTime = sheetsService.GetCellValue(range, "F", r, "");
        ////                if (string.IsNullOrEmpty(endTime))
        ////                    LogError(sheetName, r, "No end time");

        ////                string virtualFormat = sheetsService.GetCellValue(range, "F", r, "");
        ////                if (string.IsNullOrEmpty(virtualFormat))
        ////                    LogError(sheetName, r, "No virtual format");

        ////                string zoomOption = sheetsService.GetCellValue(range, "F", r, "");
        ////                if (string.IsNullOrEmpty(zoomOption))
        ////                    LogError(sheetName, r, "No zoom option");
        ////            }



        ////        }
        ////        catch(Exception ex)
        ////        {
        ////            LogError(sheetName, ex.Message);
        ////        }

        ////        //break;
        ////    }

        ////    if(_errorLog.Count > 0)
        ////    {
        ////        List<string> errors = new List<string>();
        ////        foreach(var entry in _errorLog)
        ////        {
        ////            string details = string.Join("\n", entry.Value);
        ////            errors.Add(string.Format("{0}:\n{1}\n\n", entry.Key, details));
        ////        }
        ////        docsService.CreateDoc(outputFolder.Id, "_error-log", errors);
        ////    }
        ////}

        [Test]
        public void CreateEventOrderFormsLocal()
        {
            string credentialFile = "_CatfishGoogleApiDesktopCredentials.json";
            string[] scopes = new string[] { DriveService.Scope.Drive };

            var serviceBuilder = _testHelper.GoogleApiServiceBuilder;
            serviceBuilder.Init(credentialFile, scopes, "Congress 2021 Billing");

            IGoogleSheetsService googleSheetsService = serviceBuilder.CreateSheetService();
            IDriveService driveService = new WindowsDriveService(); //serviceBuilder.CreateDriveService();
            IDocsService docsService = new WindowsTextDocService(); //serviceBuilder.CreateDocsService();
            ISheetService excelSheetService = new ExcelSheetService();

            string sheetNamePrefix = "Assoc";

            string srcId = "1Rt6C2Unna5CLAhwLD6RROO_Vb5m2uCiQGRbhd2Rjez8";
            string outputRootId = @"C:\Projects\Catfish-2.0-Congress21_Data";
            string templateId = Path.Combine(outputRootId, "Template.xlsx");

            var src = googleSheetsService.LoadSpreadSheet(srcId);

            string outputFolderName = string.Format("Output_{0}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            var outputFolderId = driveService.CreateFolder(outputRootId, outputFolderName);

            var assocSheetNames = src.Sheets
                .Where(sh => sh.Properties.Title.StartsWith(sheetNamePrefix))
                .Select(sh => sh.Properties.Title)
                .ToList();

            var srcDataSets = googleSheetsService.LoadData(srcId, assocSheetNames, "A:AZ");

            for (int assocIndex = 0; assocIndex < assocSheetNames.Count; ++assocIndex)
            {
                var sheetName = assocSheetNames[assocIndex];
                var range = srcDataSets.ValueRanges[assocIndex];

                try
                {
                    var assocNameNum = googleSheetsService.GetCellValue(range, "B", 1, "");// range.Values[0][1] as string; //B1
                    if (string.IsNullOrEmpty(assocNameNum))
                        LogError(sheetName, "Association name/number not specified");

                    var bookingCode = googleSheetsService.GetCellValue(range, "B", 3, ""); //range.Values[2][1] as string; //B3
                    if (string.IsNullOrEmpty(bookingCode))
                        LogError(sheetName, "Booking code not specified");

                    if (!bookingCode.StartsWith("21-"))
                        LogError(sheetName, "Booking code must start with \"21-\"");

                    //Creating a clone of the template
                    var orderFormId = driveService.Clone(templateId, outputFolderId, bookingCode + ".xlsx");
                    Assert.IsNotNull(orderFormId);
                    var workbook = excelSheetService.LoadSpreadsheet(orderFormId);

                    int fullDayPackageCount = 0;
                    int halfDayPackageCount = 0;
                    int additionalPackageCount = 0;

                    //Repeat from row #7 onwards (note the 0-basd index)
                    for (int r = 7; r < range.Values.Count; ++r)
                    {
                        var sheet = excelSheetService.DuplicateSheet(workbook, "Details", string.Format("R-{0}", r));
                        if (sheet == null)
                        {
                            LogError(sheetName, r, "Couldn't create the worksheet");
                            continue;
                        }

                        excelSheetService.SetCellVal(sheet, 11, SheetHelper.Letter2Column("C"), assocNameNum);
                        excelSheetService.SetCellVal(sheet, 13, SheetHelper.Letter2Column("C"), bookingCode);

                        string packageType = googleSheetsService.GetCellValue(range, "A", r, "");
                        if (string.IsNullOrEmpty(packageType))
                            LogError(sheetName, r, "No package type");
                        else
                        {
                            if (packageType.ToLower().StartsWith("full-day av"))
                                ++fullDayPackageCount;
                            else if (packageType.ToLower().StartsWith("half-day av"))
                                ++halfDayPackageCount;
                            else if (packageType.ToLower().StartsWith("additional av"))
                                ++additionalPackageCount;

                        }


                        string sessionId = googleSheetsService.GetCellValue(range, "C", r, "");
                        if (string.IsNullOrEmpty(sessionId))
                            LogError(sheetName, r, "No session id");
                        else
                            excelSheetService.SetCellVal(sheet, 13, SheetHelper.Letter2Column("F"), sessionId);

                        string sesstionTitle = googleSheetsService.GetCellValue(range, "D", r, "");
                        if (string.IsNullOrEmpty(sesstionTitle))
                            LogError(sheetName, r, "No session title");
                        else
                            excelSheetService.SetCellVal(sheet, 15, SheetHelper.Letter2Column("F"), sesstionTitle);

                        string dateStr = googleSheetsService.GetCellValue(range, "E", r, "");
                        if (string.IsNullOrEmpty(dateStr))
                            LogError(sheetName, r, "No session date");
                        else
                        {
                            int month = int.Parse(dateStr.Substring(0, 2));
                            int day = int.Parse(dateStr.Substring(3, 2));
                            int year = 2000 + int.Parse(dateStr.Substring(6, 2));
                            var date = new DateTime(year, month, day);
                           excelSheetService.SetCellVal(sheet, 15, SheetHelper.Letter2Column("B"), date);
                        }

                        string startTime = googleSheetsService.GetCellValue(range, "F", r, "");
                        if (string.IsNullOrEmpty(startTime))
                            LogError(sheetName, r, "No start time");
                        else
                            excelSheetService.SetCellVal(sheet, 17, SheetHelper.Letter2Column("B"), startTime);

                        string endTime = googleSheetsService.GetCellValue(range, "G", r, "");
                        if (string.IsNullOrEmpty(endTime))
                            LogError(sheetName, r, "No end time");
                        else
                            excelSheetService.SetCellVal(sheet, 17, SheetHelper.Letter2Column("F"), endTime);

                        ///Begin: Accessibility Section
                        int outRow = 24;
                        int outrowMax = 30;
                        string[] itemCols = { "N", "P", "T", "U", "W", "Y", "AA" };
                        string[] costCols = { "O", "Q", "", "V", "X", "Z", "AB" };

                        for(int i=0; i<itemCols.Length; ++i)
                        {
                            string item = googleSheetsService.GetCellValue(range, itemCols[i], r, "");
                            if (!string.IsNullOrEmpty(item))
                            {
                                excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("A"), item);

                                if (!string.IsNullOrEmpty(costCols[i]))
                                {
                                    string cost = googleSheetsService.GetCellValue(range, costCols[i], r, "");
                                    excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("G"), cost);
                                }
                                ++outRow;
                            }
                        }
                        //Clearing up any remainig rows
                        while(outRow <= outrowMax)
                        {
                            excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("A"), "");
                            excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("G"), "");
                            ++outRow;
                        }
                        ///END: Accessibility Section
                        ///

                        ///Begin: Editing Section
                        outRow = 38;
                        outrowMax = 44;
                        itemCols = new string[] { "AM", "AO", "AQ", "AS", "AU", "AW", "AY" };
                        costCols = new string[] { "AN", "AP", "AR", "AT", "AV", "AX", "AZ" };

                        for (int i = 0; i < itemCols.Length; ++i)
                        {
                            string item = googleSheetsService.GetCellValue(range, itemCols[i], r, "");
                            if (!string.IsNullOrEmpty(item))
                            {
                                excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("A"), item);

                                if (!string.IsNullOrEmpty(costCols[i]))
                                {
                                    string cost = googleSheetsService.GetCellValue(range, costCols[i], r, "");
                                    excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("G"), cost);
                                }
                                ++outRow;
                            }
                        }
                        //Clearing up any remainig rows
                        while (outRow <= outrowMax)
                        {
                            excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("A"), "");
                            excelSheetService.SetCellVal(sheet, outRow, SheetHelper.Letter2Column("G"), "");
                            ++outRow;
                        }
                        ///END: Editing Section
                    }

                    var summarySheet = excelSheetService.GetSheet(workbook, "Summary");
                    excelSheetService.SetCellVal(summarySheet, 11, SheetHelper.Letter2Column("C"), assocNameNum);
                    excelSheetService.SetCellVal(summarySheet, 13, SheetHelper.Letter2Column("C"), bookingCode);
                    if (halfDayPackageCount > 0)
                        excelSheetService.SetCellVal(summarySheet, 20, SheetHelper.Letter2Column("D"), halfDayPackageCount);
                    if (fullDayPackageCount > 0)
                        excelSheetService.SetCellVal(summarySheet, 23, SheetHelper.Letter2Column("D"), fullDayPackageCount);
                    if (additionalPackageCount > 0)
                        excelSheetService.SetCellVal(summarySheet, 26, SheetHelper.Letter2Column("D"), additionalPackageCount);


                    excelSheetService.SaveSpreadsheet(workbook);
                }
                catch (Exception ex)
                {
                    LogError(sheetName, ex.Message);
                }

            }

            if (_errorLog.Count > 0)
            {
                List<string> errors = new List<string>();
                foreach (var entry in _errorLog)
                {
                    string details = string.Join("\n", entry.Value);
                    errors.Add(string.Format("{0}:\n{1}\n\n", entry.Key, details));
                }
                docsService.CreateDoc(outputFolderId, "_error-log.txt", errors);
            }
        }
    }
}
