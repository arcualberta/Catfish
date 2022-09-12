﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.GoogleApi.Services
{
    public class ExcelSheetService : ISheetService
    {
        public object LoadSpreadsheet(string spreadsheetRef)
        {
            return new XLWorkbook(spreadsheetRef);
        }

        public bool SaveSpreadsheet(object spreadsheet)
        {
            (spreadsheet as XLWorkbook).Save();
            return true;
        }

        public object GetSheet(object spreadsheetRef, string workSheetName)
        {
            var workbook = spreadsheetRef is string ? new XLWorkbook(spreadsheetRef as string) : spreadsheetRef as XLWorkbook;
            var sheet = workbook.Worksheets.FirstOrDefault(sh => sh.Name == workSheetName);
            return sheet;
        }

        public object DuplicateSheet(object spreadsheetRef, string srcSheetName, string dstSheetName)
        {
            var workbook = spreadsheetRef is string ? new XLWorkbook(spreadsheetRef as string) : spreadsheetRef as XLWorkbook;

            var srcSheet = workbook.Worksheets.FirstOrDefault(sh => sh.Name == srcSheetName);
            var newSheet = srcSheet.CopyTo(dstSheetName);

            if (spreadsheetRef is string)
                workbook.Save();

            return newSheet;

            ////using (var package = new ExcelPackage(new FileInfo("Book.xlsx")))
            ////{
            ////    var firstSheet = package.Workbook.Worksheets["First Sheet"];
            ////    Console.WriteLine("Sheet 1 Data");
            ////    Console.WriteLine($"Cell A2 Value   : {firstSheet.Cells["A2"].Text}");
            ////    Console.WriteLine($"Cell A2 Color   : {firstSheet.Cells["A2"].Style.Font.Color.LookupColor()}");
            ////    Console.WriteLine($"Cell B2 Formula : {firstSheet.Cells["B2"].Formula}");
            ////    Console.WriteLine($"Cell B2 Value   : {firstSheet.Cells["B2"].Text}");
            ////    Console.WriteLine($"Cell B2 Border  : {firstSheet.Cells["B2"].Style.Border.Top.Style}");
            ////    Console.WriteLine("");

            ////    var secondSheet = package.Workbook.Worksheets["Second Sheet"];
            ////    Console.WriteLine($"Sheet 2 Data");
            ////    Console.WriteLine($"Cell A2 Formula : {secondSheet.Cells["A2"].Formula}");
            ////    Console.WriteLine($"Cell A2 Value   : {secondSheet.Cells["A2"].Text}");
            ////}
            ////throw new NotImplementedException();
        }

        public bool SetCellVal(object sheet, int row, int col, object val)
        {
            IXLWorksheet worksheet = sheet as IXLWorksheet;
            worksheet.Row(row).Cell(col).Value = val;
            return true;
        }

        public object DeleteSheet(object spreadsheet, string sheetName)
        {
            IXLWorkbook workbook = spreadsheet as IXLWorkbook;
            workbook.Worksheets.Delete(sheetName);
            return workbook;
        }
    }
}