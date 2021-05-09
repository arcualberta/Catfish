﻿using Catfish.GoogleApi.Services;
using Catfish.Test.Helpers;
using Google.Apis.Drive.v3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        [Test]
        public void CreateEventOrderForms()
        {
            string credentialFile = "_CatfishGoogleApiDesktopCredentials.json";
            string[] scopes = new string[] { DriveService.Scope.Drive };

            var serviceBuilder = _testHelper.GoogleApiServiceBuilder;
            serviceBuilder.Init(credentialFile, scopes, "Congress 2021 Billing");

            IGoogleDriveService driveService = serviceBuilder.CreateDriveService();
            IGoogleSheetsService sheetsService = serviceBuilder.CreateSheetService();
            IGoogleDocsService docsService = serviceBuilder.CreateDocsService();

            string sheetNamePrefix = "Assoc";

            string srcId = "1Rt6C2Unna5CLAhwLD6RROO_Vb5m2uCiQGRbhd2Rjez8";
            string templateId = "1KcKj8b4NGJDVMTCoxztgGzmv9yc0kU5vi26lnEBHLFs";
            string outputRootId = "16lxbYpBFufGKMbz6mgyfXs47C2WCh6z4";

            var src = sheetsService.LoadSpreadSheet(srcId);

            string outputFolderName = string.Format("Output_{0}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            var outputFolder = driveService.CreateFolder(outputRootId, outputFolderName);

            var assocSheetNames = src.Sheets
                .Where(sh => sh.Properties.Title.StartsWith(sheetNamePrefix))
                .Select(sh => sh.Properties.Title)
                .ToList();

            foreach (var sheet in src.Sheets)
            {
                string sheetName = sheet.Properties.Title;
                if (!sheetName.StartsWith(sheetNamePrefix))
                    continue;

                try
                {
                    var range = sheetsService.LoadData(srcId, sheetName, "A:AZ");

                    var assocNameNum = range.Values[0][1] as string; //B1
                    Assert.IsNotNull(assocNameNum);

                    var bookingCode = range.Values[2][1] as string; //B3
                    Assert.IsNotNull(bookingCode);

                    if (!bookingCode.StartsWith("21-"))
                        throw new Exception("Booking code must start with \"21-\"");

                    //Creating a clone of the template
                    var orderForm = driveService.Clone(templateId, outputFolder.Id, bookingCode);
                    Assert.IsNotNull(orderForm);

                    int fullDayPackageCount = 0;
                    int halfDayPackageCount = 0;
                    int additionalPackageCount = 0;

                    //Repeat from row #7 onwards (note the 0-basd index)
                    for (int r = 7; r < range.Values.Count; ++r)
                    {
                        string packageType = sheetsService.GetCellValue(range, "A", r, "");

                        if (packageType.ToLower().StartsWith("full-day av"))
                            ++fullDayPackageCount;
                        else if (packageType.ToLower().StartsWith("half-day av"))
                            ++halfDayPackageCount;
                        else if (packageType.ToLower().StartsWith("additional av"))
                            ++additionalPackageCount;

                        var detailsSheet = sheetsService.DuplicateSheet(orderForm.Id, "DetailsTemplate", string.Format("R-{0}", r + 1));

                        if (string.IsNullOrEmpty(packageType))
                            LogError(sheetName, r, "No package type");

                        string sessionId = sheetsService.GetCellValue(range, "C", r, ""); 
                        if (string.IsNullOrEmpty(sessionId))
                            LogError(sheetName, r, "No session id");

                        string sesstionTitle = sheetsService.GetCellValue(range, "D", r, "");
                        if (string.IsNullOrEmpty(sesstionTitle))
                            LogError(sheetName, r, "No session title");

                        string date = sheetsService.GetCellValue(range, "E", r, "");
                        if (string.IsNullOrEmpty(date))
                            LogError(sheetName, r, "No session date");

                        string startTime = sheetsService.GetCellValue(range, "F", r, "");
                        if (string.IsNullOrEmpty(startTime))
                            LogError(sheetName, r, "No start time");

                        string endTime = sheetsService.GetCellValue(range, "F", r, "");
                        if (string.IsNullOrEmpty(endTime))
                            LogError(sheetName, r, "No end time");

                        string virtualFormat = sheetsService.GetCellValue(range, "F", r, "");
                        if (string.IsNullOrEmpty(virtualFormat))
                            LogError(sheetName, r, "No virtual format");

                        string zoomOption = sheetsService.GetCellValue(range, "F", r, "");
                        if (string.IsNullOrEmpty(zoomOption))
                            LogError(sheetName, r, "No zoom option");
                    }



                }
                catch(Exception ex)
                {
                    LogError(sheetName, ex.Message);
                }

                break;
            }

            if(_errorLog.Count > 0)
            {
                List<string> errors = new List<string>();
                foreach(var entry in _errorLog)
                {
                    string details = string.Join("\n", entry.Value);
                    errors.Add(string.Format("{0}:\n{1}\n\n", entry.Key, details));
                }
                docsService.CreateDoc(outputFolder.Id, "_error-log", errors);
            }
        }
    }
}
