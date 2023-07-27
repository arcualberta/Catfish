// See https://aka.ms/new-console-template for more information
using Catfish.DataImport.ImportHandlers;
using Catfish.DataImport.Interfaces;

Console.WriteLine("Start data importing!");

IImportHnadler skipHandler = new SkipDataImportHandler();
skipHandler.Execute();
