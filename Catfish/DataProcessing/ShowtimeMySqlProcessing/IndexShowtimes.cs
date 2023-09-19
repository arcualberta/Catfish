﻿using Catfish.API.Repository.Services;
using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
using CliWrap;
using DataProcessing.ShowtimeMySqlProcessing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataProcessing.ShowtimeMySqlProcessing.IndexMovies;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class IndexShowtimes
    {
        private readonly TestHelper _testHelper = new TestHelper();

        public delegate void CreateMySqlModel(string concatenatedFieldValues);
        public delegate Task FileProcessingDelegate(string fileName, string? param1 = null, int? param2 = null);

        protected void ProcessSqlInserts(string sourceSqlDumpFileName, string modelName, CreateMySqlModel createInstance)
        {
            string dataFolder = sourceSqlDumpFileName.Substring(0, sourceSqlDumpFileName.LastIndexOf("\\"));
            string tmp = sourceSqlDumpFileName.Substring(sourceSqlDumpFileName.LastIndexOf("\\") + 1);
            tmp = tmp.Substring(0, tmp.Length - 4);
            string errorLogFile = $"{dataFolder}\\zz_{tmp}-errors.txt";
            string progressLogFile = $"{dataFolder}\\zz_{tmp}-progress.txt";

            DateTime startTime = DateTime.Now;
            File.AppendAllText(progressLogFile, $"Started at: {startTime}\n");

            int lineNumber = 0;
            int count = 0;
            using (StreamReader sr = File.OpenText(sourceSqlDumpFileName))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    ++lineNumber;

                    if (!line.StartsWith($"INSERT INTO `{modelName}` VALUES "))
                        continue;

                    int offset = 0, end = 0;
                    while (offset >= 0)
                    {
                        offset = line.IndexOf('(', offset);
                        if (offset > 0)
                        {
                            end = line.IndexOf("),(", offset + 1);
                            string valueSet = end > 0 ? line.Substring(offset + 1, end - offset - 1) : line.Substring(offset + 1).TrimEnd(new char[] {')', ';'});
                            ++count;
                            try
                            {
                                createInstance(valueSet);
                            }
                            catch (Exception ex)
                            {
                                string message = $"Line: {lineNumber} Start: {offset} End: {end}\n{ex.Message}\n{ex.StackTrace}\n{line}\n\n";
                                File.AppendAllText(errorLogFile, message);
                            }

                            offset = end > 0 ? end + 1 : -1;
                        }
                    }

                    File.AppendAllText(progressLogFile, $"Line: {lineNumber}, Total Objects Processed: {count}\n");
                }

                sr.Close();
            }

            DateTime endTime = DateTime.Now;
            File.AppendAllText(progressLogFile, $"Completed at: {endTime}\n");
            File.AppendAllText(progressLogFile, $"Computation time: {endTime - startTime}\n");

        }

        private void ProcessShwotime(string concatenatedFieldValues)
        {
            var showtime = MySqlShowtime.CreateInstance2(concatenatedFieldValues);
        }

        protected List<MySqlCountryOrigin> _countryOrigins = new List<MySqlCountryOrigin>();
        protected List<MySqlDistribution> _distribuitons= new List<MySqlDistribution>();
        protected List<MySqlMovie> _movies = new List<MySqlMovie>();
        protected List<MySqlMovieCast> _movieCasts = new List<MySqlMovieCast>();
        protected List<MySqlMovieGenre> _movieGenres = new List<MySqlMovieGenre>();
        protected List<MySqlTheater> _theaters = new List<MySqlTheater>();

        protected List<SolrDoc> _solrDocs = new List<SolrDoc>();

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.Execute
        [Fact]
        public async Task Execute()
        {
            ////_countryOrigins = _testHelper.countryDbContext.Data.ToList();
            ////_distribuitons = _testHelper.distributionDbContext.Data.ToList();
            ////_movies = _testHelper.movieDbContext.Data.ToList();
            ////_movieCasts = _testHelper.movieCastDbContext.Data.ToList();
            ////_movieGenres = _testHelper.movieGenreDbContext.Data.ToList();
            ////_theaters = _testHelper.theaterDbContext.Data.ToList();


            string sourceSqlDumpFileName = "C:\\Projects\\Showtime Database\\kinomatics\\07-showtime.sql";
            ProcessSqlInserts(sourceSqlDumpFileName, "Showtime", ProcessShwotime);

            /*
            int count = 0;
            int lineNumber = 0;

            using (StreamReader sr = File.OpenText(sourceSqlDumpFileName))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    ++lineNumber;

                    if (!line.StartsWith("INSERT INTO"))
                        continue;

                    int offset = 0, end = 0;
                    while (offset >= 0)
                    {
                        offset = line.IndexOf('(', offset);
                        if (offset > 0)
                        {
                            end = line.IndexOf(')', offset + 1);

                            if (end > 0)
                            {
                                string valueSet = line.Substring(offset + 1, end - offset - 1);

                                try
                                {
                                    ++count;
                                    MySqlShowtime showtime = MySqlShowtime.CreateInstance(valueSet);
                                }
                                catch (Exception ex)
                                {
                                    string message = $"Line: {lineNumber} Start: {offset} End: {end}\n{ex.Message}\n{ex.StackTrace}\n\n ";
                                    File.AppendAllText(errorLogFile, message);
                                }
                            }

                            offset = end + 1;
                        }
                    }

                    File.AppendAllText(progressLogFile, $"Line: {lineNumber}, Total Objects Processed: {count}");
                }

                sr.Close();
            }
            */
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.SplitDataFileIntoMultipleFiles
        [Fact]
        public async Task SplitDataFileIntoMultipleFiles()
        {
            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:SrcFolder").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;
            string outputFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:InsertFileFolder").Value;

            Directory.CreateDirectory(logFolder);
            Directory.CreateDirectory(outputFolder);

            string srcSqlDumpFileName = Path.Combine(srcFolder, "07-showtime.sql");
            string outLogFile = Path.Combine(logFolder, "out.txt");
            string errorLogFile = Path.Combine(logFolder, "error.txt");


            //Loading statements that needs to be added before and after each insert set
            //======================================================================================
            List<string> preStatements = new List<string>();
            List<string> postStatements = new List<string>();

            bool addToPreStatements = true;
            bool addToPostStatements = false;
            bool donePreStatements = false;

            DateTime start = DateTime.Now;

            using (StreamReader sr = File.OpenText(srcSqlDumpFileName))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("--"))
                        continue; //Skippping comments.

                    if (line.StartsWith("DROP TABLE IF EXISTS"))
                        addToPreStatements = false;

                    if(line.StartsWith("LOCK TABLES"))
                        addToPreStatements = true;

                    if (addToPreStatements && line.StartsWith("INSERT INTO"))
                    {
                        //DONE all the prestatements and starts the inserts block
                        addToPreStatements = false;
                        donePreStatements = true;
                    }

                    if (addToPreStatements)
                        preStatements.Add(line);

                    if (donePreStatements)
                    {
                        if(!addToPostStatements && !line.StartsWith("INSERT INTO"))
                            addToPostStatements = true;

                        if(addToPostStatements)
                            postStatements.Add(line);
                    }
                }
                sr.Close();
            }

            DateTime stage1 = DateTime.Now;
            await File.AppendAllTextAsync(outLogFile, $"Finished collecting pre statamenets and post statements in {stage1 - start}.\n");

            int numInsertsPerFile = 50;
//            int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:NumInsertsPerFile").Value;
            string outputFile = null;
            int lineCount = 0;
            int insertCount = 0;
            int insertFileIndex = 0;
            bool appendPostStatements = false;
            try
            {
                using (StreamReader sr = File.OpenText(srcSqlDumpFileName))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++lineCount;

                        if (!line.StartsWith($"INSERT INTO `Showtime` VALUES "))
                            continue;

                        if (insertCount % numInsertsPerFile == 0)
                        {
                            if(appendPostStatements)
                            {
                                //We already have copied insert statements to the outfile.
                                //We should now copy the post statements to it
                                await File.AppendAllLinesAsync(outputFile!, postStatements);
                                appendPostStatements = false;
                            }

                            //Creating a new output file
                            outputFile = Path.Combine(outputFolder, $"insert-{string.Format("{0:d3}", ++insertFileIndex)}.sql");

                            //Inserting pre-statements
                            await File.AppendAllLinesAsync(outputFile, preStatements);

                            //Flag to insert post statements when the inserts are complete.
                            appendPostStatements = true;
                        }

                        await File.AppendAllTextAsync(outputFile!, line + "\n");
                        ++insertCount;
                    }

                    //Done processing all lines. We need to append the post statements to the end of the last file
                    if(appendPostStatements)
                        await File.AppendAllLinesAsync(outputFile!, postStatements);

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(errorLogFile, $"{ex.Message}\n");
                File.AppendAllText(errorLogFile, $"Inner Exception:\n{ex.InnerException}\n");
                File.AppendAllText(errorLogFile, $"Stack Trace:\n {ex.StackTrace}\n");
            }

            DateTime stage2 = DateTime.Now;

            await File.AppendAllTextAsync(outLogFile, $"Total line count: {lineCount}\nTotal insert count: {insertCount}\nTotal time: {stage2 - start}\n");
        }

        #region Main Batch Processing Delegator Method

        protected async Task ProcessFileBatch (
            string srcFolder,
            string stopFlagFile,
            string logFolder,
            string? outputFolder,
            string trackerFile,
            string errorLogFile,
            string progressLogFile,
            FileProcessingDelegate fileProcessingDelegate)
        {
            Assert.True(Directory.Exists(srcFolder));

            Directory.CreateDirectory(logFolder);

            if (!string.IsNullOrEmpty(outputFolder))
                Directory.CreateDirectory(outputFolder);

            trackerFile = Path.Combine(logFolder, trackerFile);
            errorLogFile = Path.Combine(logFolder, errorLogFile);
            progressLogFile = Path.Combine(logFolder, progressLogFile);

            string[] srcFfiles = Directory.GetFiles(srcFolder).OrderBy(x => x).ToArray();
            List<string> ingestedFiles = File.Exists(trackerFile) ? new List<string>(await File.ReadAllLinesAsync(trackerFile)) : new List<string>();

            var started = DateTime.Now;
            await File.AppendAllTextAsync(progressLogFile, $"Started at: {started}\n");
            int fileIndex = 0;

            foreach (string srcFile in srcFfiles)
            {
                if (File.Exists(stopFlagFile) && File.ReadAllText(stopFlagFile).StartsWith("1"))
                    break;

                if (ingestedFiles.Contains(srcFile))
                    continue;

                try
                {
                    var t1 = DateTime.Now;
                    await fileProcessingDelegate(srcFile, outputFolder, ++fileIndex);
                    var t2 = DateTime.Now;
                    ingestedFiles.Add(srcFile);
                    await File.AppendAllTextAsync(trackerFile, $"{srcFile}\n");
                    await File.AppendAllTextAsync(progressLogFile, $"{srcFile.Substring(srcFile.LastIndexOf("\\") + 1)}: {t2 - t1}\n");
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync(errorLogFile, $"{ex.Message}\n\n{ex.InnerException}\n\n{ex.StackTrace}\n\n\n");
                }
            }

            var completed = DateTime.Now;
            await File.AppendAllTextAsync(progressLogFile, $"Completed at: {completed}\n");
            await File.AppendAllTextAsync(progressLogFile, $"Total time: {completed - started}\n");

        }

        #endregion

        #region Uploading Batch of SQL Insert Files to MySql Database

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.UploadSplitFilesToMySqlDatabase
        [Fact]
        public async Task UploadSplitFilesToMySqlDatabase()
        {
            //Source folder for this method is the output folder of insert split files
            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:InsertFileFolder").Value;
            string stopFlagFile = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:StopFlagFile").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;

            string trackerFile = "mysql-ingestion-tracker.txt";
            string errorLogFile = "mysql-ingestion-errors.txt";
            string progressLogFile = "mysql-ingestion-progress.txt";

            await ProcessFileBatch(
                srcFolder,
                stopFlagFile,
                logFolder,
                null, //Nothing to output into an output folder since the output goes to the MySql database.
                trackerFile,
                errorLogFile,
                progressLogFile,
                IngestFile);
        }

        protected async Task IngestFile(string sqlFile, string? _, int? __)
        {
            string host = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Host").Value;
            string user = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:User").Value;
            string password = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Password").Value;
            string port = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Port").Value;
            string database = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Database").Value;

            await using var input = File.OpenRead(sqlFile);

            try
            {
                await Cli.Wrap("mysql.exe")
                    .WithArguments(new[] {
                    "--protocol=tcp",
                    $"--host={host}",
                    $"--user={user}",
                    $"--password={password}",
                    $"--port={port}",
                    "--default-character-set=utf8",
                    "--comments",
                    $"--database={database}"
                    })
                    .WithStandardInputPipe(PipeSource.FromStream(input))
                    .ExecuteAsync();

                input.Close();

            }
            catch (Exception ex)
            {
                input.Close();
                throw ex;
            }
        }

        #endregion

        #region Extracting Insert Data from a Batch of Files to Text Data Files

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.ExtractInsertDataFromSplitMySqlFilesToTextFiles
        [Fact]
        public async Task ExtractInsertDataFromSplitMySqlFilesToTextFiles()
        {
            //Source folder for this method is the output folder of insert split files
            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:InsertFileFolder").Value;
            string stopFlagFile = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:StopFlagFile").Value; 
            string outptFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:TextDataFolder").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;

            string trackerFile = "text-export-tracker.txt";
            string errorLogFile = "text-export-errors.txt";
            string progressLogFile = "text-export-progress.txt";

            await ProcessFileBatch(
                srcFolder,
                stopFlagFile,
                logFolder,
                outptFolder,
                trackerFile,
                errorLogFile,
                progressLogFile,
                ExportTextFromInserts);
        }

        protected async Task ExportTextFromInserts(string sqlFile, string? outputFolder, int? fileIndex)
        {
            int lineCount = 0;

            try
            {
                string outFile = $"showtime.txt_{string.Format("{0:d3}", fileIndex)}";
                outFile = Path.Combine(outputFolder!, outFile);

                using (StreamReader sr = File.OpenText(sqlFile))
                {
                    using (StreamWriter writer = File.CreateText(outFile))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            ++lineCount;

                            if (!line.StartsWith($"INSERT INTO `Showtime` VALUES "))
                                continue;

                            var valueStr = line.Substring(line.IndexOf("VALUES (") + 8);
                            valueStr = valueStr.TrimEnd(new char[] { ')', ';' });
                            string[] values = valueStr.Split("),(");

                            foreach(var val in values)
                                await writer.WriteLineAsync(val);
                        }

                        writer.Close();
                    }

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Export Text Error on line {lineCount} in {sqlFile}\n", ex);
            }
        }

        #endregion


        #region Uploading Batch of Text Data to MySql Database

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.UploadTextDataFilesToMySqlDatabase
        [Fact]
        public async Task UploadTextDataFilesToMySqlDatabase()
        {
            //Source folder for this method is the output folder of insert split files
            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:TextDataFolder").Value;
            string stopFlagFile = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:StopFlagFile").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;

            string trackerFile = "mysql-text-data-ingestion-tracker.txt";
            string errorLogFile = "mysql-text-data-ingestion-errors.txt";
            string progressLogFile = "mysql-text-data-ingestion-progress.txt";

            await ProcessFileBatch(
                srcFolder,
                stopFlagFile,
                logFolder,
                null, //Nothing to output into an output folder since the output goes to the MySql database.
                trackerFile,
                errorLogFile,
                progressLogFile,
                IngestTextDataFile);
        }

        protected async Task IngestTextDataFile(string txtDataFile, string? _, int? __)
        {
            string host = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Host").Value;
            string user = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:User").Value;
            string password = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Password").Value;
            string port = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Port").Value;
            string database = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Database").Value;

            try
            {
                await Cli.Wrap("mysqlimport.exe")
                    .WithArguments(new[] {
                    "--protocol=tcp",
                    $"--host={host}",
                    $"--user={user}",
                    $"--password={password}",
                    $"--port={port}",
                    "--default-character-set=utf8",
                    "--fields-optionally-enclosed-by='",
                    "--fields-terminated-by=,",
                    "--lines-terminated-by=\\r\\n",
                    database,
                    txtDataFile
                    })
                    .ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
