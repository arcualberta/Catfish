using Catfish.API.Repository.Services;
using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
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
            string outputFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:OutputFolder").Value;
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

        [Fact]
        public async Task UploadSplitFilesToMySqlDatabase()
        {
            string folderRoot = Path.Combine(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:SrcFolder").Value, "output");
            string srcFolder = Path.Combine(folderRoot, "insert-files");
            Assert.True(Directory.Exists(srcFolder));

            string[] sqlFiles = Directory.GetFiles(srcFolder).OrderBy(x => x).ToArray();

            string trackerFile = Path.Combine(folderRoot, "mysql-ingestion-tracker.txt");
            List<string> ingestedFiles = File.Exists(trackerFile) ? new List<string>(await File.ReadAllLinesAsync(trackerFile)) : new List<string>();

            int i = 12;
            string x = string.Format("{0:d3}", i);

            foreach (string sqlFile in sqlFiles)
            {
                if (ingestedFiles.Contains(sqlFile))
                    continue;

                IngestFile(sqlFile);
                ingestedFiles.Add(sqlFile);
                await File.AppendAllTextAsync(trackerFile, $"{sqlFile}\n");
            }
        }

        protected void IngestFile(string sqlFile)
        {
            Thread.Sleep(1000);
        }
    }
}
