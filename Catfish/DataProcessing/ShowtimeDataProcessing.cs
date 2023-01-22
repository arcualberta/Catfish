using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.API.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.Xml;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR.Protocol;
using NuGet.Packaging.Signing;
using Catfish.API.Repository.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataProcessing
{
    [Collection("Database collection")]
    public class ShowtimeDataProcessing : IClassFixture<TransactionalTestDatabaseFixture>
    {
        public TransactionalTestDatabaseFixture _fixture { get; }

        public readonly TestHelper _testHelper;
        public int MAX_RECORDS = 1; //DEBUG ONLY -- set it to 0 or -1 to ignore it

        public ShowtimeDataProcessing(TransactionalTestDatabaseFixture fixture)
        {
            _fixture = fixture;
            _testHelper = new TestHelper();
        }


        [Fact]
        public void MergeTest()
        {
            XmlDoc doc = new XmlDoc();

            string sa = "String A";
            string sb = "String B";
            string sc = "String C";
            string s1 = doc.MergeStrings(sa, sb, 2);
            string s2 = doc.MergeStrings(s1, sc, 3);
            string s3 = doc.MergeStrings(null, sb, 2);

            List<string> aa = new List<string>() { "aa a" };
            List<string> ab = new List<string>() { "ab a", "ab b" };
            List<string> ac = new List<string>() { "ac a" };

            var a1 = doc.MergeArrays(aa, ab, 2);
            var a2 = doc.MergeArrays(a1, ac, 3);
            var a3 = doc.MergeArrays(new List<string>(), ab, 2);

        }


        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.CreateDbRecords
        [Fact]
        public void CreateDbRecords()
        {
            DateTime start = DateTime.Now;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:IsProductionRun")?.Value, out bool productionRun))
                productionRun = true;
            if (!bool.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SkipProcessedFoldersAndZipFiles")?.Value, out bool skippProcessedFoldersAndZipFiles))
                skippProcessedFoldersAndZipFiles = true;
            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:ContextTimeoutInMinutes")?.Value, out int contextTimeoutInMinutes))
                contextTimeoutInMinutes = 3;


            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:MaxBatchesToProcess")?.Value, out int maxBatchesToProcess))
                maxBatchesToProcess = int.MaxValue;

            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:MaxShowtimeBatchesToProcess")?.Value, out int maxShowtimeBatchesToProcess))
                maxShowtimeBatchesToProcess = int.MaxValue;

            //var context = _testHelper.ShowtimeDb;

            string srcFolderRoot = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));
            string outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string logFilePrefix = productionRun ? "" : "dry-run-";
            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}processing - log-{fileSuffix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{logFilePrefix}error-log-{fileSuffix}.txt");

            var srcBatcheFolders = Directory.GetDirectories(srcFolderRoot);

            int batch = 0;

            //var tracking_keys = context.TrackingKeys.Select(record => record.entry_key).ToList();
            foreach (var batchFolder in srcBatcheFolders)
            {
                using (var batchContext = _testHelper.CreateNewShowtimeDbContext())
                {
                    try
                    {
                        int? timeout1 = batchContext.Database.GetCommandTimeout();
                        batchContext.Database.SetCommandTimeout(contextTimeoutInMinutes * 60);
                        File.AppendAllText(processingLogFile, $"    Changed DB Timeout from: {(timeout1.HasValue ? timeout1.Value : "NULL")} to {(contextTimeoutInMinutes * 60)} seconds{Environment.NewLine}{Environment.NewLine}");

                        ++batch;

                        if (maxBatchesToProcess < batch)
                            break;

                        string folder_key = batchFolder.Substring(srcFolderRoot.Length + 1);
                        if (skippProcessedFoldersAndZipFiles && batchContext.TrackingKeys.Where(record => record.entry_key == folder_key).Any())
                            continue;

                        var zipFiles = Directory.GetFiles(batchFolder);
                        foreach (var zipFile in zipFiles)
                        {
                            try
                            {
                                string zipfile_key = zipFile.Substring(srcFolderRoot.Length + 1);
                                if (skippProcessedFoldersAndZipFiles && batchContext.TrackingKeys.Where(record => record.entry_key == zipfile_key).Any())
                                    continue;

                                File.AppendAllText(processingLogFile, $"Archive {zipFile}{Environment.NewLine}");
                                int showtimeCount = 0, newTheaterCount = 0, updatedTheaterCount = 0, newMovieCount = 0, updatedMovieCount = 0;

                                using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        using (var entryContext = _testHelper.CreateNewShowtimeDbContext())
                                        {
                                            try
                                            {
                                                entryContext.Database.SetCommandTimeout(contextTimeoutInMinutes * 60);

                                                if ((maxShowtimeBatchesToProcess < batch) && entry.Name.EndsWith("S.XML"))
                                                    continue;

                                                var entry_key = $"{zipfile_key}\\{entry.Name}";
                                                if (entryContext.TrackingKeys.Where(record => record.entry_key == entry_key).Any())
                                                    continue;

                                                var extractFile = Path.Combine(outputFolder, entry.Name);
                                                if (File.Exists(extractFile))
                                                    File.Delete(extractFile);

                                                entry.ExtractToFile(extractFile);

                                                XElement xml = XElement.Load(extractFile);
                                                if (xml.Name == "times")
                                                {
                                                    //This is a showtime data set
                                                    foreach (var child in xml.Elements("showtime"))
                                                    {
                                                        Showtime showtime = new Showtime(child);
                                                        if (productionRun)
                                                            entryContext.ShowtimeRecords.Add(new ShowtimeRecord() { batch = batch, movie_id = showtime.movie_id, theater_id = showtime.theater_id, show_date = showtime.show_date, movie_error = false, theater_error = false, is_validated = false, content = JsonSerializer.Serialize(showtime) });
                                                        ++showtimeCount;
                                                        //context.SaveChanges();
                                                    }
                                                }
                                                else if (xml.Name == "movies")
                                                {
                                                    //This is a movies data set. 
                                                    foreach (var child in xml.Elements("movie"))
                                                    {
                                                        Movie movie = new Movie(child);

                                                        //Checking if such a move record already exist in the database
                                                        MovieRecord dbMovie = entryContext.MovieRecords.FirstOrDefault(m => m.movie_id == movie.movie_id);
                                                        if (dbMovie != null)
                                                        {
                                                            dbMovie.Merge(movie);
                                                            ++updatedMovieCount;
                                                        }
                                                        else
                                                        {
                                                            if (productionRun)
                                                                entryContext.MovieRecords.Add(new MovieRecord() { batch = batch, instances = 1, movie_id = movie.movie_id, content = JsonSerializer.Serialize(movie) });
                                                            ++newMovieCount;
                                                        }
                                                        //context.SaveChanges();
                                                    }
                                                }
                                                else if (xml.Name == "houses")
                                                {
                                                    //This is a theatres data set
                                                    foreach (var child in xml.Elements("theater"))
                                                    {
                                                        Theater theater = new Theater(child);

                                                        //Checking if such a theater record already exist in the database
                                                        TheaterRecord dbTheater = entryContext.TheaterRecords.FirstOrDefault(t => t.theater_id == theater.theater_id);
                                                        if (dbTheater != null)
                                                        {
                                                            dbTheater.Merge(theater);
                                                            ++updatedTheaterCount;
                                                        }
                                                        else
                                                        {
                                                            if (productionRun)
                                                                entryContext.TheaterRecords.Add(new TheaterRecord() { batch = batch, instances = 1, theater_id = theater.theater_id, content = JsonSerializer.Serialize(theater) });
                                                            ++newTheaterCount;
                                                        }
                                                        //context.SaveChanges();
                                                    }
                                                }

                                                //Mark that current entry is done processing
                                                entryContext.TrackingKeys.Add(new TrackingKey() { entry_key = entry_key });

                                                if (productionRun)
                                                    entryContext.SaveChanges();
                                                File.Delete(extractFile);
                                            }
                                            catch (Exception ex)
                                            {
                                                File.AppendAllText(errorLogFile, $"EXCEPTION in {entry.Name}: {ex.Message}{Environment.NewLine}");
                                            }
                                        } //End: using (var entryContext = new ShowtimeDbContext(dbOptions))
                                        GC.Collect();

                                    } //End: foreach (ZipArchiveEntry entry in archive.Entries)

                                    File.AppendAllText(processingLogFile, $"    New Movies: {newMovieCount}, Updated Movies: {updatedMovieCount}, New Theaters: {newTheaterCount}, Updated Theaters: {updatedTheaterCount}, Showtimes: {showtimeCount}{Environment.NewLine}{Environment.NewLine}");

                                } //End:  using (ZipArchive archive = ZipFile.OpenRead(zipFile))

                                //Mark that the current zip file is done processing
                                batchContext.TrackingKeys.Add(new TrackingKey() { entry_key = zipfile_key });
                                if (productionRun)
                                    batchContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText(errorLogFile, $"EXCEPTION in {zipFile}: {ex.Message}{Environment.NewLine}");
                            }
                        } //End: foreach (var zipFile in zipFiles)

                        //Mark that the current batch is done processing
                        batchContext.TrackingKeys.Add(new TrackingKey() { entry_key = folder_key });
                        if (productionRun)
                            batchContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(errorLogFile, $"EXCEPTION in {batchFolder}: {ex.Message}{Environment.NewLine}");
                    }
                } //End: using (var batchContext = new ShowtimeDbContext(dbOptions))
                GC.Collect();

            }//End: foreach (var batchFolder in srcBatcheFolders)
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexRawEntries
        [Fact]
        public void IndexRawEntries()
        {
            DateTime start = DateTime.Now;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SkipMovies")?.Value, out bool skipMovies))
                skipMovies = false;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SkipTheaters")?.Value, out bool skipTheaters))
                skipTheaters = false;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SkipShowtimes")?.Value, out bool skipShowtimes))
                skipShowtimes = false;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SkipProcessedFoldersAndZipFiles")?.Value, out bool skippProcessedFoldersAndZipFiles))
                skippProcessedFoldersAndZipFiles = true;

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:MaxBatchesToProcess")?.Value, out int maxBatchesToProcess))
                maxBatchesToProcess = int.MaxValue;

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:MaxShowtimeBatchesToProcess")?.Value, out int maxShowtimeBatchesToProcess))
                maxShowtimeBatchesToProcess = int.MaxValue;

            string outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string srcFolderRoot = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SourceFolderRoot")?.Value;
            if (string.IsNullOrEmpty(srcFolderRoot))
                outputFolder = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));

            string logFilePrefix = "raw-";
            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}processing-{fileSuffix}.txt");
            string errorLogFile =      Path.Combine(outputFolder, $"{logFilePrefix}processing-{fileSuffix}-errors.txt");

            string trackingFile = Path.Combine(outputFolder, "tracking-keys.txt");
            if(!File.Exists(trackingFile))
                File.Create(trackingFile).Close();
            string[] trackingKeys = File.ReadAllLines(trackingFile);

            var srcBatcheFolders = Directory.GetDirectories(srcFolderRoot);

            var solrService = _testHelper.Solr;
            int taskWaitTimeoutMilliseconds = 60000;//10 minutes
            int batch = 0;
            foreach (var batchFolder in srcBatcheFolders)
            {
                
                try
                {
                    if (maxBatchesToProcess < batch)
                        break;
                    ++batch;

                    string folder_key = batchFolder.Substring(srcFolderRoot.Length + 1);
                    if (skippProcessedFoldersAndZipFiles && trackingKeys.Contains(folder_key))
                        continue;

                    var zipFiles = Directory.GetFiles(batchFolder);
                    List<SolrDoc> solrDocs = new List<SolrDoc>();
                    foreach (var zipFile in zipFiles)
                    {
                        try
                        {
                            string zipfile_key = zipFile.Substring(srcFolderRoot.Length + 1);
                            if (skippProcessedFoldersAndZipFiles && trackingKeys.Contains(zipfile_key))
                                continue;

                            File.AppendAllText(processingLogFile, $"Archive {zipFile}.{Environment.NewLine}");
                            int movieCount = 0, theaterCount = 0, showtimeCount = 0;

                            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                            {
                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
                                    if (folder_key == "0_backfill")
                                    {
                                        if (skipMovies && entry.Name.ToUpper().EndsWith("IMOVIES.XML"))
                                            continue;

                                        if (skipTheaters && entry.Name.ToUpper().EndsWith("THEATER.XML"))
                                            continue;

                                        if (skipShowtimes && entry.Name.ToUpper().EndsWith("SCREENS.XML"))
                                            continue;
                                    }
                                    else
                                    {
                                        if (skipMovies && entry.Name.ToUpper().EndsWith("I.XML"))
                                            continue;

                                        if (skipTheaters && entry.Name.ToUpper().EndsWith("T.XML"))
                                            continue;

                                        if (skipShowtimes && entry.Name.ToUpper().EndsWith("S.XML"))
                                            continue;
                                    }

                                    if ((maxShowtimeBatchesToProcess < batch) && entry.Name.EndsWith("S.XML"))
                                        continue;

                                    string entry_key = $"{zipfile_key}\\{entry.Name}";
                                    if (trackingKeys.Contains(entry_key))
                                        continue;

                                    try
                                    {
                                        Stream stream = entry.Open();
                                        string entryContent = null;
                                        using (StreamReader reader = new StreamReader(stream))
                                        {
                                            entryContent = reader.ReadToEnd();
                                            reader.Close();                                           
                                        }
                                        stream.Close();

                                        if (string.IsNullOrWhiteSpace(entryContent))
                                        {
                                            File.AppendAllText(errorLogFile, $"No data in {entry_key}");
                                            continue; //foreach (ZipArchiveEntry entry in archive.Entries)
                                        }

                                        XElement xml = XElement.Parse(entryContent);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();

                                            SolrDoc solrDoc = new SolrDoc();
                                            solrDocs.Add(solrDoc);

                                            solrDoc.AddId(Guid.NewGuid().ToString());
                                            solrDoc.AddField("entry_type_s", $"raw-{entryType}");
                                            solrDoc.AddField("entry_src_s", entry_key);

                                            if(entryType == "movie")
                                            {
                                                AddMovie(solrDoc, new Movie(child));
                                                ++movieCount;
                                            }
                                            else if (entryType == "theater")
                                            {
                                                AddTheater(solrDoc, new Theater(child));
                                                ++theaterCount;
                                            }
                                            else if (entryType == "showtime")
                                            {
                                                AddShowtime(solrDoc, new Showtime(child));
                                                ++showtimeCount;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        File.AppendAllText(errorLogFile, $"EXCEPTION in {entry_key}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
                                    }

                                    //Indexing the documents
                                    if(solrService.Index(solrDocs).Wait(taskWaitTimeoutMilliseconds))
                                    {
                                        if (solrService.CommitAsync().Wait(taskWaitTimeoutMilliseconds))
                                            File.AppendAllText(trackingFile, $"{entry_key}{Environment.NewLine}");
                                        else
                                            File.AppendAllText(errorLogFile, $"Commit timed out for {entry_key}.{Environment.NewLine}{Environment.NewLine}");


                                    }
                                    else
                                        File.AppendAllText(errorLogFile, $"Indexing timed out for {entry_key}.{Environment.NewLine}{Environment.NewLine}");

                                    solrDocs.Clear();
                                    GC.Collect();

                                } //End: foreach (ZipArchiveEntry entry in archive.Entries)

                            } //End:  using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                            
                            //Mark that the current zip file is done processing
                            File.AppendAllText(trackingFile, $"{zipfile_key}{Environment.NewLine}");

                            File.AppendAllText(processingLogFile, $"    Movies: {movieCount}, Theaters: {theaterCount}, Showtimes: {showtimeCount}{Environment.NewLine}");
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(errorLogFile, $"EXCEPTION in {zipFile}: {ex.Message}{Environment.NewLine}");
                        }
                    } //End: foreach (var zipFile in zipFiles)

                    //Mark that the current batch is done processing
                    File.AppendAllText(trackingFile, $"{folder_key}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(errorLogFile, $"EXCEPTION in {batchFolder}: {ex.Message}{Environment.NewLine}");
                }
                //GC.Collect();

            }//End: foreach (var batchFolder in srcBatcheFolders)
        }


        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexFlattenedShowtimes
        [Fact]
        public void IndexFlattenedShowtimes()
        {
            DateTime start = DateTime.Now;

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:MaxParallelProcesses")?.Value, out int maxParallelProcess))
                maxParallelProcess = 1;

            string outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string srcFolderRoot = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SourceFolderRoot")?.Value;
            if (string.IsNullOrEmpty(srcFolderRoot))
                srcFolderRoot = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));

            var srcFolders = Directory.GetDirectories(srcFolderRoot);
            List<string[]> sourceBatches = new List<string[]>();
            int batchSize = (int)Math.Ceiling((double)srcFolders.Length / maxParallelProcess);
            int offset = 0;
            while(offset < srcFolders.Length)
            {
                sourceBatches.Add(srcFolders.Skip(offset).Take(batchSize).ToArray());
                offset += batchSize;
            }

            var tasks = sourceBatches.Select(x => IndexFlattenedShowtimesBatch(x, outputFolder, start));
            Task.WhenAll(tasks).Wait();
        }

        private async Task IndexFlattenedShowtimesBatch(string[] folderList, string outputFolder, DateTime start)
        {
            int srcFolderPathCharacterLength = folderList[0].LastIndexOf("\\") + 1;
            string first = folderList[0].Substring(srcFolderPathCharacterLength);
            string last = folderList[folderList.Length-1].Substring(srcFolderPathCharacterLength);
            
            string logFilePrefix = $"flattened-showtimes-{first}-{last}";

            string timestampStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}.txt");
            string errorLogFile      = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}-errors.txt");

            string trackingFile = Path.Combine(outputFolder, $"tracking-keys-{first}-{last}.txt");
            if (!File.Exists(trackingFile))
                File.Create(trackingFile).Close();
            string[] trackingKeys = File.ReadAllLines(trackingFile);


            var solrService = _testHelper.Solr;
            foreach (var srcFolder in folderList)
            {
                string folder_key = srcFolder.Substring(srcFolderPathCharacterLength);
                try
                {
                    if (trackingKeys.Contains(folder_key))
                        continue;

                    var zipFiles = Directory.GetFiles(srcFolder);
                    bool folderProcessingSuccessful = true;
                    foreach (var zipFile in zipFiles)
                    {
                        string zipfile_key = zipFile.Substring(srcFolderPathCharacterLength);

                        List<Movie> movies = new List<Movie>();
                        List<Theater> theaters = new List<Theater>();

                        try
                        {
                            if (trackingKeys.Contains(zipfile_key))
                                continue;

                            await File.AppendAllTextAsync(processingLogFile, $"Archive {zipFile}.{Environment.NewLine}");
                            bool zipFilerProcessingSuccessful = true;

                            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                            {
                                List<ZipArchiveEntry> moviesArchiveEntries = null;
                                List<ZipArchiveEntry> theatersArchiveEntries = null;
                                List<ZipArchiveEntry> showtimeArchiveEntries = null;

                                if (folder_key == "0_backfill")
                                {
                                    moviesArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("IMOVIES.XML")).ToList();
                                    theatersArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("THEATER.XML")).ToList();
                                    showtimeArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("SCREENS.XML")).ToList();
                                }
                                else
                                {
                                    moviesArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("I.XML")).ToList();
                                    theatersArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("T.XML")).ToList();
                                    showtimeArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("S.XML")).ToList();
                                }

                                //Loading movies
                                foreach (var moviesArchiveEntry in moviesArchiveEntries)
                                {
                                    try
                                    {
                                        XElement xml = LoadXmlFromZipEntry(moviesArchiveEntry);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();
                                            if (entryType == "movie")
                                                movies.Add(new Movie(child));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        zipFilerProcessingSuccessful = false;
                                        folderProcessingSuccessful = false;
                                        await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {moviesArchiveEntry.Name}: {ex.Message}{Environment.NewLine}");
                                    }
                                }

                                //Loading theaters
                                foreach (var theatersArchiveEntry in theatersArchiveEntries)
                                {
                                    try
                                    {
                                        XElement xml = LoadXmlFromZipEntry(theatersArchiveEntry);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();
                                            if (entryType == "theater")
                                                theaters.Add(new Theater(child));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        zipFilerProcessingSuccessful = false;
                                        folderProcessingSuccessful = false;
                                        await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {theatersArchiveEntry.Name}: {ex.Message}{Environment.NewLine}");
                                    }
                                }

                                //Iterating through showtime entries
                                foreach (var showtimeArchiveEntry in showtimeArchiveEntries)
                                {
                                    string entry_key = $"{zipfile_key}\\{showtimeArchiveEntry.Name}";
                                    if (trackingKeys.Contains(entry_key))
                                        continue;

                                    List<SolrDoc> solrDocs = new List<SolrDoc>();
                                    try
                                    {
                                        XElement xml = LoadXmlFromZipEntry(showtimeArchiveEntry);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();
                                            if (entryType == "showtime")
                                            {
                                                var showtime = new Showtime(child);

                                                var matchingMovies = movies.Where(m => m.movie_id == showtime.movie_id).ToList();
                                                if (matchingMovies.Count == 0 || matchingMovies.Count > 1)
                                                    await File.AppendAllTextAsync(errorLogFile, $"{entry_key}: {matchingMovies.Count} movies found for movie_id {showtime.movie_id} {Environment.NewLine}");

                                                var matchingTheaters = theaters.Where(t => t.theater_id == showtime.theater_id).ToList();
                                                if (matchingTheaters.Count == 0 || matchingTheaters.Count > 1)
                                                    await File.AppendAllTextAsync(errorLogFile, $"{entry_key}: {matchingTheaters.Count} theaters found for theater_id {showtime.theater_id} {Environment.NewLine}");

                                                SolrDoc solrDoc = new SolrDoc();
                                                solrDocs.Add(solrDoc);

                                                solrDoc.AddId(Guid.NewGuid().ToString());
                                                solrDoc.AddField("entry_type_s", "showtime");
                                                solrDoc.AddField("entry_src_s", entry_key);

                                                AddShowtime(solrDoc, showtime);

                                                if (matchingMovies.Any())
                                                    AddMovie(solrDoc, matchingMovies.First(), true);

                                                if(matchingTheaters.Any())
                                                    AddTheater(solrDoc, matchingTheaters.First(), true);
                                            }
                                        } //End: foreach (var child in xml.Elements())

                                        //Indexing the showtimes batch
                                        await solrService.Index(solrDocs);
                                        await solrService.CommitAsync();
                                        await File.AppendAllTextAsync(trackingFile, $"{entry_key}{Environment.NewLine}");
                                    }
                                    catch (Exception ex)
                                    {
                                        zipFilerProcessingSuccessful = false;
                                        folderProcessingSuccessful = false;
                                        await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {entry_key}: {ex.Message}{Environment.NewLine}");
                                    }

                                    solrDocs.Clear();
                                    GC.Collect();

                                } //End: foreach (var showtimeArchiveEntry in showtimeArchiveEntries)
                            } //End: using (ZipArchive archive = ZipFile.OpenRead(zipFile))

                            if (zipFilerProcessingSuccessful)
                                await File.AppendAllTextAsync(trackingFile, $"{zipfile_key}{Environment.NewLine}");
                        }
                        catch (Exception ex)
                        {
                            folderProcessingSuccessful = false;
                            await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile}: {ex.Message}{Environment.NewLine}");
                        }

                    } //End: foreach (var zipFile in zipFiles)

                    if (folderProcessingSuccessful)
                        await File.AppendAllTextAsync(trackingFile, $"{folder_key}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {folder_key}: {ex.Message}{Environment.NewLine}");
                }
                //GC.Collect();
               
            }//End: foreach (var batchFolder in srcBatcheFolders)
        }


        private XElement LoadXmlFromZipEntry(ZipArchiveEntry entry)
        {
            Stream stream = entry.Open();
            string entryContent = null;
            using (StreamReader reader = new StreamReader(stream))
            {
                entryContent = reader.ReadToEnd();
                reader.Close();
            }
            stream.Close();

            if (string.IsNullOrWhiteSpace(entryContent))
                return null;

            XElement xml = XElement.Parse(entryContent);
            return xml;
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.DuplicateCheck
        [Fact]
        public void DuplicateCheck()
        {
            DateTime start = DateTime.Now;
     
            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:DuplicateCheckBatchSize")?.Value, out int batchSize))
                batchSize = int.MaxValue;

            string entryType = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:DuplicateCheckEntryType")?.Value;
            Assert.False(string.IsNullOrWhiteSpace(entryType), "ShowtimeDbIngesionSettings:DuplicateCheckEntryType should be specified.");

            string identifierField = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:DuplicateCheckIdentifierField")?.Value;
            Assert.False(string.IsNullOrWhiteSpace(identifierField), "ShowtimeDbIngesionSettings:DuplicateCheckIdentifierField should be specified.");

            string outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string startTimeStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string filePrefix = $"duplicate-check-{entryType}s-{startTimeStr}";
            string processingLogFile = Path.Combine(outputFolder, $"{filePrefix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{filePrefix}-errors.txt");
            string duplicateOutputFile = Path.Combine(outputFolder, $"{filePrefix}-results.txt");
           
            var solrService = _testHelper.Solr;
            int taskWaitTimeoutMilliseconds = 60000;//10 minutes
            int batch = 0;
            int totalEntryCount = int.MaxValue;
            int offset = 0;
            string query = $"entry_type_s:{entryType}";
            string sortBy = $"{identifierField} asc";
            int totalProcessed = 0;
            int totalDuplicatesFound = 0;
            int uniqueDuplicateCount = 0;
            while (offset < totalEntryCount)
            {
                //We will be attempting to detect duplicate entries by comparing the identifierField value
                //in a given entry with the value of the same field in the next entry after retrieving the items
                //by identifierField. Therefore, we need to retrieve at least one extra entry than the specified batchSize
                var effectiveBatchLength = batchSize + 1;

                var task = solrService.ExecuteSearch(query, offset, effectiveBatchLength, null, sortBy, identifierField);
                if (!task.Wait(taskWaitTimeoutMilliseconds))
                {
                    File.AppendAllText(errorLogFile, $"Query loading timed out at batch {batch} (offset {offset}).{Environment.NewLine}");
                    continue; //while(offset < totalEntryCount)
                }
                SearchResult queryResult = task.Result;
                if (queryResult.TotalMatches < totalEntryCount)
                    totalEntryCount = queryResult.TotalMatches;

                //Iterate though 
                int duplicateCountInBatch = 0;
                string lastDuplicateIdentifierValue = null;
                for (int i= 0; i < queryResult.ResultEntries.Count-1; ++i)
                {
                    try
                    {
                        var currentIdentifierValue = queryResult.ResultEntries[i].Data.Where(d => d.Key == identifierField).Select(d => d.Value).First().ToString();
                        var nextIdentifierValue = queryResult.ResultEntries[i + 1].Data.Where(d => d.Key == identifierField).Select(d => d.Value).First().ToString();

                        if (currentIdentifierValue == nextIdentifierValue)
                        {
                            //We have found a duplicate
                            ++duplicateCountInBatch;
                            ++totalDuplicatesFound;

                            //If this is a new duplicate record that was not previously reported, record it.
                            if(lastDuplicateIdentifierValue != currentIdentifierValue)
                            {
                                ++uniqueDuplicateCount;
                                File.AppendAllText(duplicateOutputFile, $"{currentIdentifierValue}{Environment.NewLine}");
                                lastDuplicateIdentifierValue = currentIdentifierValue;
                            }
                        }
                        ++totalProcessed;
                    }
                    catch(Exception ex)
                    {
                        File.AppendAllText(errorLogFile, $"Error in batch {batch} > entry {i}:{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
                    }
                }
                File.AppendAllText(processingLogFile, $"Completed batch {batch}. Offset {offset}. Retrieved {queryResult.ResultEntries.Count} records. Found {duplicateCountInBatch} duplicates.{Environment.NewLine}");

                if (queryResult.ResultEntries.Count < batchSize)
                    break; //while(offset < totalEntryCount)

                offset = offset + batchSize;
                ++batch;
            }
            File.AppendAllText(processingLogFile, $"{Environment.NewLine}Processing completed in {(DateTime.Now - start).ToString()}.{Environment.NewLine}\tSuccessfully processed {totalProcessed + 1} entries.{Environment.NewLine}\tFound a total of {totalDuplicatesFound} duplicates of {uniqueDuplicateCount} records.{Environment.NewLine}");
        }


        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.DuplicateCheck2
        [Fact]
        public void DuplicateCheck2()
        {
            //Finding the total number of entries to be processed
            string entryType = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:DuplicateCheckEntryType")?.Value;
            Assert.False(string.IsNullOrWhiteSpace(entryType), "ShowtimeDbIngesionSettings:DuplicateCheckEntryType should be specified.");
            var task = _testHelper.Solr.ExecuteSearch($"entry_type_s:{entryType}", 0, 1);
            task.Wait();
            int totalCount = task.Result.TotalMatches;

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:MaxParallelProcesses")?.Value, out int maxParallelProcesses))
                maxParallelProcesses = 5;

            int range = (int) Math.Ceiling(totalCount / (double)maxParallelProcesses);

            List<int> offsets = new List<int>();
            for(int i = 0; i < maxParallelProcesses; ++i)
                offsets.Add(i * range);

            //DetectDuplicates(entryType, 0, range).Wait();
            var tasks = offsets.Select(x => DetectDuplicates(entryType, x, range));
            Task.WhenAll(tasks).Wait();
        }

        private async Task DetectDuplicates(string entryType, int offset, int maxCount)
        {
            DateTime start = DateTime.Now;

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:DuplicateCheckBatchSize")?.Value, out int batchSize))
                batchSize = int.MaxValue;

            string identifierField = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:DuplicateCheckIdentifierField")?.Value;
            Assert.False(string.IsNullOrWhiteSpace(identifierField), "ShowtimeDbIngesionSettings:DuplicateCheckIdentifierField should be specified.");

            string outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string startTimeStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string filePrefix = $"duplicate-check-{entryType}s_{offset}-to-{offset+maxCount}_{startTimeStr}";
            string processingLogFile = Path.Combine(outputFolder, $"{filePrefix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{filePrefix}-errors.txt");
            string duplicateOutputFile = Path.Combine(outputFolder, $"{filePrefix}-results.txt");

            var solrService = _testHelper.Solr;
            string query = $"entry_type_s:{entryType}";
            int totalProcessed = 0;
            int totalDuplicatesFound = 0;
            int maxOffset = offset + maxCount;
            while (offset < maxOffset)
            {
                int numEntriesToRetrieve = ((offset + batchSize) < maxOffset) ? batchSize : (maxOffset - offset);

                SearchResult queryResult = await solrService.ExecuteSearch(query, offset, numEntriesToRetrieve, null, null, identifierField);
                if (queryResult.ResultEntries.Count == 0)
                    break; // while (offset < maxOffset)

                //Iterate though query results
                int duplicateCountInBatch = 0;
                string identifierFieldValue = "";
                foreach (var entry in queryResult.ResultEntries)
                {
                    try
                    {
                        identifierFieldValue = entry.Data.Where(d => d.Key == identifierField).Select(d => d.Value).First().ToString();
                        string matchedIdentifierSearchQuery = $"{query} AND {identifierField}:\"{identifierFieldValue}\"";
                        SearchResult matchedIdentifierSearchResult = await solrService.ExecuteSearch(matchedIdentifierSearchQuery, offset, 2, null, null, identifierField);
                        if (matchedIdentifierSearchResult.ResultEntries.Count > 1)
                        {
                            //We have found a duplicate
                            ++duplicateCountInBatch;
                            ++totalDuplicatesFound;
                            File.AppendAllText(duplicateOutputFile, $"{identifierFieldValue}{Environment.NewLine}");
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(errorLogFile, $"Error in the entry.{identifierField} = {identifierFieldValue}.{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
                    }
                    ++totalProcessed;
                }
                File.AppendAllText(processingLogFile, $"Completed batch of offset from {offset} to {offset + numEntriesToRetrieve}. Retrieved {queryResult.ResultEntries.Count} records. Found {duplicateCountInBatch} duplicates.{Environment.NewLine}");


                offset = offset + batchSize;
            }
            File.AppendAllText(processingLogFile, $"{Environment.NewLine}Processing completed in {(DateTime.Now - start).ToString()}.{Environment.NewLine}\tSuccessfully processed {totalProcessed + 1} entries.{Environment.NewLine}\tFound a total of {totalDuplicatesFound} duplicates.{Environment.NewLine}");
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexData
        [Fact]
        public void IndexData()
        {
            DateTime start = DateTime.Now;

            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:IndexBatchSize")?.Value, out int batchSize))
                batchSize = 1000;

            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:MaxBatchesToProcess")?.Value, out int maxBatchesToProcess))
                maxBatchesToProcess = int.MaxValue;

            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:StartupShowtimeIdForIndexing")?.Value, out int startShowtimeId))
                startShowtimeId = 0;

            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:StopShowtimeIdForIndexing")?.Value, out int stopShowtimeId))
                stopShowtimeId = int.MaxValue;

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:ContextTimeoutInMinutes")?.Value, out int contextTimeoutInMinutes))
                contextTimeoutInMinutes = 3;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:AllowDuplicateShowtimeRecords")?.Value, out bool allowDuplicateShowtimeRecords))
                allowDuplicateShowtimeRecords = false;

            if (!bool.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:SaveSolrDocsInsteadOfPosting")?.Value, out bool saveSolrDocsInsteadOfPosting))
                saveSolrDocsInsteadOfPosting = false;

            string outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string indexType = allowDuplicateShowtimeRecords ? "-with-duplicates" : "";
            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"index-data-log{indexType}_{fileSuffix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"indexing-data-error{indexType}-log_{fileSuffix}.txt");

            string solrDocsFolderRelativePath = null;
            string solrDocsFolderAbsolutePath = null;
            if (saveSolrDocsInsteadOfPosting)
            {
                solrDocsFolderRelativePath = $"solr-docs{indexType}";
                solrDocsFolderAbsolutePath = Path.Combine(outputFolder, solrDocsFolderRelativePath);
                Directory.CreateDirectory(solrDocsFolderAbsolutePath);
            }


            List<SolrDoc> solrDocs = new List<SolrDoc>();
            int offset = 0;
            if (startShowtimeId > 0)
            {
                using (var context = _testHelper.CreateNewShowtimeDbContext())
                {
                    context.Database.SetCommandTimeout(contextTimeoutInMinutes * 60);
                    var firstRecord = context.ShowtimeRecords.FirstOrDefault();
                    int min_id = firstRecord == null ? 0 : firstRecord.id;
                    offset = startShowtimeId > min_id ? startShowtimeId - min_id : 0;
                }
            }

            int currentBatch = 0;
            bool continue_processing = true;
            while (continue_processing)
            {
                var connection_t0 = DateTime.Now;
                using (var context = _testHelper.CreateNewShowtimeDbContext())
                {
                    string batchStr = "";
                    context.Database.SetCommandTimeout(contextTimeoutInMinutes * 60);
                    var connection_t1 = DateTime.Now;
                    string log_message = $"Connection setup time: {(connection_t1 - connection_t0).TotalSeconds} seconds.";

                    try
                    {
                        var sql_t0 = DateTime.Now;
                        var showtimes = context!.ShowtimeRecords.Skip(offset).Take(batchSize).ToList();
                        var sql_t1 = DateTime.Now;

                        if (!showtimes.Any() || currentBatch >= maxBatchesToProcess)
                            break; //while(true)

                        if (showtimes.Last().id > stopShowtimeId)
                        {
                            var skip_set = showtimes.Where(st => st.id >= stopShowtimeId).ToList();
                            foreach (var st in skip_set)
                                showtimes.Remove(st);
                            continue_processing = false;
                        }

                        offset += showtimes.Count;
                        ++currentBatch;

                        batchStr = $"{showtimes.First().id} - {showtimes.Last().id}";

                        File.AppendAllText(processingLogFile, $"{log_message} Loaded showtime records with ids in the range {batchStr} in {(sql_t1 - sql_t0).TotalSeconds} seconds.");

                        //Creating solr docs
                        Movie movie = null;
                        Theater theater = null;
                        Showtime showtime = null;

                        for (int i = 0; i < showtimes.Count; ++i)
                        {
                            var showtimeRecord = showtimes[i];

                            try
                            {
                                showtime = JsonSerializer.Deserialize<Showtime>(showtimeRecord.content.ToString());

                                if (!showtimeRecord.movie_error.HasValue || !showtimeRecord.movie_error.Value)
                                {
                                    if (movie?.movie_id == showtime!.movie_id)
                                    {
                                        //Movie is already loaded in an immediate past iteration. Nothing to do here
                                    }
                                    else
                                    {
                                        MovieRecord movieRecord = context.MovieRecords.FirstOrDefault(m => m.movie_id == showtimeRecord.movie_id);
                                        if (movieRecord == null)
                                        {
                                            movie = null;
                                            foreach (var rec in showtimes.Where(sr => sr.movie_id == showtimeRecord.movie_id))
                                                rec.movie_error = true;
                                        }
                                        else
                                            movie = JsonSerializer.Deserialize<Movie>(movieRecord!.content);
                                    }
                                }

                                if (!showtimeRecord.theater_error.HasValue || !showtimeRecord.theater_error.Value)
                                {
                                    if (theater?.theater_id == showtime!.theater_id)
                                    {
                                        //Theater is already loaded in an immediate past iteration. Nothing to do here
                                    }
                                    else
                                    {
                                        TheaterRecord theaterRecord = context.TheaterRecords.FirstOrDefault(t => t.theater_id == showtimeRecord.theater_id);
                                        if (theaterRecord == null)
                                        {
                                            theater = null;
                                            foreach (var rec in showtimes.Where(sr => sr.theater_id == showtimeRecord.theater_id))
                                                rec.theater_error = true;
                                        }
                                        else
                                            theater = JsonSerializer.Deserialize<Theater>(theaterRecord!.content);
                                    }
                                }

                                SolrDoc doc = new SolrDoc();

                                AddShowtime(doc, showtime!, showtimeRecord.id, allowDuplicateShowtimeRecords);

                                if (movie != null)
                                    AddMovie(doc, movie);

                                if (theater != null)
                                    AddTheater(doc, theater);

                                solrDocs.Add(doc);
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText(errorLogFile, $"EXCEPTION in Showtime {showtimeRecord.id} <movie_id:{showtimeRecord.movie_id}, theater_id:{showtimeRecord.theater_id}, show_date:{showtimeRecord.show_date}>: {ex.Message}{Environment.NewLine}");
                            }

                        }//End: for (int i = 0; i < showtimes.Count; ++i)


                        string exportMessage = $"Data processing time: {(DateTime.Now - sql_t1).TotalSeconds} seconds. "
                            + (saveSolrDocsInsteadOfPosting ? "Saving" : "Indexing")
                            + $" {solrDocs.Count} records";

                        var solr_t0 = DateTime.Now;
                        if (solrDocs.Count > 0)
                        {
                            if (saveSolrDocsInsteadOfPosting)
                            {
                                try
                                {
                                    var solrDocsBatchFile = $"{showtimes[0].id}-{showtimes[showtimes.Count - 1].id}";
                                    File.AppendAllText(processingLogFile, $" {exportMessage} to {solrDocsFolderRelativePath}\\{solrDocsBatchFile}.zip");
                                    SaveToZipFile(solrDocs, solrDocsFolderAbsolutePath!, solrDocsBatchFile);
                                }
                                catch(Exception ex)
                                {
                                    File.AppendAllText(errorLogFile, $"EXCEPTION in saving to zip file {batchStr}.zip: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
                                }
                            }
                            else
                            {
                                try
                                {
                                    //Call solr service to index the batch of docs
                                    ISolrService solr = _testHelper.Solr;

                                    File.AppendAllText(processingLogFile, $" Indexing {solrDocs.Count} records");
                                    solr.Index(solrDocs).Wait(600000); //10 minute timeout
                                    solr.CommitAsync().Wait(600000); //10 minute timeout
                                }
                                catch (Exception ex)
                                {
                                    File.AppendAllText(errorLogFile, $"EXCEPTION in posting to solr index in batch {batchStr}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
                                }
                            }

                            //Clearning the bufffer
                            solrDocs.Clear();

                        }//End: if (solrDocs.Count > 0)

                        var solr_t1 = DateTime.Now;
                        File.AppendAllText(processingLogFile, $" completed in {(solr_t1 - solr_t0).TotalSeconds} seconds.");
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(errorLogFile, $"EXCEPTION in batch {batchStr}: {ex.Message}{Environment.NewLine}");
                    }
                }//End: using (var context = _testHelper.CreateNewShowtimeDbContext())
                GC.Collect();
                File.AppendAllText(processingLogFile, $" Batch execution time: {(DateTime.Now - connection_t0).TotalSeconds} seconds.{Environment.NewLine}");

            }//End: while(true)


            var timelapse = (DateTime.Now - start).ToString();
            string logText = $"Total indexing time: {timelapse}{Environment.NewLine}";
            File.AppendAllText(processingLogFile, logText);
        }


        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexPreprocessedData
        [Fact]
        public void IndexPreprocessedData()
        {
            DateTime start = DateTime.Now;

            string preprocessedFileFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:PreprocessedFileFolder")?.Value.TrimEnd('\\');
            if (string.IsNullOrEmpty(preprocessedFileFolder) || !Directory.Exists(preprocessedFileFolder))
                throw new Exception($"Preprocessed data folder does not exist: {preprocessedFileFolder}");

            string outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            if (!int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:ContextTimeoutInMinutes")?.Value, out int contextTimeoutInMinutes))
                contextTimeoutInMinutes = 3;

            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"preprocessed-data-index_{fileSuffix}-log.txt");
            string errorLogFile = Path.Combine(outputFolder, $"preprocessed-data-index_{fileSuffix}-errors.txt");

            ISolrService solr = _testHelper.Solr;
            int httpCallWaitTimeMilliseconds = 600000; //10 seconds

            string postprocessedFileFolder = preprocessedFileFolder + "-index-completed";
            Directory.CreateDirectory(postprocessedFileFolder);

            //Loading data file names in the source folder
            var srcFileNames = Directory.GetFiles(preprocessedFileFolder, "*.*");
            foreach (var absFileName in srcFileNames)
            {
                var filename = absFileName.Substring(absFileName.LastIndexOf("\\") + 1);
                try
                {
                    File.AppendAllText(processingLogFile, $" Processing {filename}");

                    var t0 = DateTime.Now;

                    if (filename.EndsWith(".xml"))
                    {
                        string xmlPayloadStr = File.ReadAllText(absFileName);
                        solr.AddUpdateAsync(xmlPayloadStr).Wait(httpCallWaitTimeMilliseconds);
                        solr.CommitAsync().Wait(httpCallWaitTimeMilliseconds);
                    }
                    else if (filename.EndsWith(".zip"))
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(absFileName))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.Name.EndsWith(".xml"))
                                {
                                    Stream stream = entry.Open();
                                    using (StreamReader sr = new StreamReader(stream))
                                    {
                                        var xmlPayloadStr = sr.ReadToEnd();
                                        solr.AddUpdateAsync(xmlPayloadStr).Wait(httpCallWaitTimeMilliseconds);
                                        solr.CommitAsync().Wait(httpCallWaitTimeMilliseconds);
                                        sr.Close();
                                    }
                                    stream.Close();
                                }
                            }
                        }
                    }
                    else
                        throw new Exception($"Don't know how to load data from the file {filename}{Environment.NewLine}");

                    File.Move(absFileName, Path.Combine(postprocessedFileFolder, filename));

                    var t1 = DateTime.Now;
                    File.AppendAllText(processingLogFile, $" completed in {(t1 - t0).TotalSeconds}.{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(errorLogFile, $"EXCEPTION in processing {filename}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
                }
                GC.Collect();
            }//End: foreach(var absFileName in srcFileNames)
        }

        private void SaveToZipFile(List<SolrDoc> solrDocs, string outputFolder, string filename)
        {
            string zipfile = Path.Combine(outputFolder, $"{filename}.zip");
            using (ZipArchive archive = ZipFile.Open(zipfile, ZipArchiveMode.Create))
            {
                ZipArchiveEntry entry = archive.CreateEntry($"{filename}.xml", CompressionLevel.Optimal);
                var output_stream = entry.Open();

                //Writing the wrapper opening element
                output_stream.Write(new UTF8Encoding(true).GetBytes("<add>\n"));

                //Writing all solr docs
                foreach (var doc in solrDocs)
                {
                    var xml = doc.Root.ToString();
                    output_stream.Write(new UTF8Encoding(true).GetBytes($"{xml}\n"));
                }

                //Writing the wrapper closing element
                output_stream.Write(new UTF8Encoding(true).GetBytes("</add>"));

                //Closing the stream
                output_stream.Close();
            }            
        }

        private void AddShowtime(SolrDoc doc, Showtime showtime)
        {
            var showdate_str = showtime.show_date.HasValue ? showtime.show_date.Value.ToString("yyyy-MM-dd") : "yyyy-mm-dd";
            doc.AddField("showtime_key_t", $"{showtime.movie_id}-{showtime.theater_id}-{showdate_str}");

            doc.AddField("movie_id_i", showtime.movie_id);
            doc.AddField("theater_id_i", showtime.theater_id);
            doc.AddField("movie_name_t", showtime.movie_name);
            doc.AddField("show_date_dt", showtime.show_date);
            if (showtime.showtimes != null)
                doc.AddField("showtime_count_i", showtime.showtimes!.Length);
            doc.AddField("showtimes_ts", showtime.showtimes);
            doc.AddField("showtime_minutes_is", showtime.showtime_minutes);
            doc.AddField("show_attributes_ts", showtime.show_attributes);
            doc.AddField("show_passes_t", showtime.show_passes);
            doc.AddField("show_festival_t", showtime.show_festival);
            doc.AddField("show_with_t", showtime.show_with);
            doc.AddField("show_sound_t", showtime.show_sound);
            doc.AddField("show_comments_ts", showtime.show_comments);
        }

        private void AddShowtime(SolrDoc doc, Showtime showtime, int showtimeDbId, bool allowDuplicateShowtimeRecords)
        {
            string showtime_id_date_str = (showtime!.show_date != null) ? showtime!.show_date.Value.ToString("yyyyMMdd") : Guid.NewGuid().ToString();
            var showtime_id = $"{showtime!.movie_id}-{showtime!.theater_id}-{showtime_id_date_str}";

            if (allowDuplicateShowtimeRecords)
            {
                doc.AddId(showtimeDbId.ToString());
                doc.AddField("showtime_id_s", showtime_id);
            }
            else
            {
                doc.AddId(showtime_id);
                doc.AddField("showtime_db_id_i", showtimeDbId);
            }

            AddShowtime(doc, showtime);
        }

        private void AddTheater(SolrDoc doc, Theater theater, bool skipId = false)
        {
            if (!skipId)
                doc.AddField("theater_id_i", theater.theater_id);
            doc.AddField("theater_name_t", theater.theater_name!);
            doc.AddField("theater_address_t", theater.theater_address);
            doc.AddField("theater_city_t", theater.theater_city);
            doc.AddField("theater_state_t", theater.theater_state);
            doc.AddField("theater_zip_t", theater.theater_zip);
            doc.AddField("theater_phone_t", theater.theater_phone);
            doc.AddField("theater_attributes_t", theater.theater_attributes);
            doc.AddField("theater_ticketing_t", theater.theater_ticketing);
            doc.AddField("theater_closed_reason_t", theater.theater_closed_reason);
            doc.AddField("theater_area_t", theater.theater_area);
            doc.AddField("theater_location_t", theater.theater_location);
            doc.AddField("theater_market_t", theater.theater_market);
            doc.AddField("theater_screens_i", theater.theater_screens);
            doc.AddField("theater_seating_t", theater.theater_seating);
            doc.AddField("theater_adult_t", theater.theater_adult);
            doc.AddField("theater_child_t", theater.theater_child);
            doc.AddField("theater_senior_t", theater.theater_senior);
            doc.AddField("theater_country_s", theater.theater_country);
            doc.AddField("theater_url_t", theater.theater_url);
            doc.AddField("theater_chain_id_t", theater.theater_chain_id);
            doc.AddField("theater_adult_bargain_t", theater.theater_adult_bargain);
            doc.AddField("theater_senior_bargain_t", theater.theater_senior_bargain);
            doc.AddField("theater_child_bargain_t", theater.theater_child_bargain);
            doc.AddField("theater_special_bargain_t", theater.theater_special_bargain);
            doc.AddField("theater_adult_super_t", theater.theater_adult_super);
            doc.AddField("theater_senior_super_t", theater.theater_senior_super);
            doc.AddField("theater_child_super_t", theater.theater_child_super);
            doc.AddField("theater_price_comment_t", theater.theater_price_comment);
            doc.AddField("theater_extra_t", theater.theater_extra);
            doc.AddField("theater_desc_t", theater.theater_desc);
            doc.AddField("theater_type_t", theater.theater_type);
            doc.AddField("theater_lon_d", theater.theater_lon);
            doc.AddField("theater_lat_d", theater.theater_lat);
        }

        private void AddMovie(SolrDoc doc, Movie movie, bool skipId = false)
        {
            if (!skipId)
                doc.AddField("movie_id_i", movie.movie_id);
            doc.AddField("parent_id_i", movie.parent_id);
            doc.AddField("title_t", movie.title);
            doc.AddField("pictures_ts", movie.pictures.ToArray());
            doc.AddField("hipictures_ts", movie.hipictures.ToArray());
            doc.AddField("rating_t", movie.rating);
            doc.AddField("advisory_t", movie.advisory);
            doc.AddField("genres_ts", movie.genres.ToArray());
            doc.AddField("casts_ts", movie.casts.ToArray());
            doc.AddField("directors_ts", movie.directors.ToArray());
            doc.AddField("release_date_dt", movie.release_date);
            doc.AddField("release_notes_t", movie.release_notes);
            doc.AddField("release_dvd_t", movie.release_dvd);
            doc.AddField("running_time_i", movie.running_time);
            doc.AddField("official_site_s", movie.official_site);
            doc.AddField("distributors_ts", movie.distributors.ToArray());
            doc.AddField("producers_ts", movie.producers.ToArray());
            doc.AddField("writers_ts", movie.writers.ToArray());
            doc.AddField("synopsis_t", movie.synopsis);
            doc.AddField("lang_s", movie.lang);
            doc.AddField("intl_country_s", movie.intl_country);
            doc.AddField("intl_name_t", movie.intl_name);
            doc.AddField("intl_cert_t", movie.intl_cert);
            doc.AddField("intl_advisory_t", movie.intl_advisory);
            doc.AddField("intl_release_dt", movie.intl_release);
            doc.AddField("intl_poster_t", movie.intl_poster);
        }      
    }

    /**
     * XmlDoc base class
     */
    public class XmlDoc
    {
        public XmlDoc() { }

        public XElement GetChildElement(XElement parent, string elementName) => parent.Element(elementName)!;
        public string? GetElementValueStr(XElement parent, string elementName) => GetChildElement(parent, elementName)?.Value;
        public string[]? GetElementValueStr(XElement parent, string elementName, string separator) => GetElementValueStr(parent, elementName)?.Split(separator).Select(str => str.Trim()).Where(str => !string.IsNullOrEmpty(str)).ToArray();
        public bool GetElementValueInt(XElement parent, string elementName, out int val) => int.TryParse(GetElementValueStr(parent, elementName), out val);
        public int GetElementValueInt(XElement parent, string elementName, int defaultValue) => GetElementValueInt(parent, elementName, out int n) ? n : defaultValue;
        public DateTime? GetElementValueDateTime(XElement parent, string elementName) => DateTime.TryParse(GetElementValueStr(parent, elementName), out DateTime val) ? val : null;
        public decimal? GetElementValueDecimal(XElement parent, string elementName) => decimal.TryParse(GetElementValueStr(parent, elementName), out decimal val) ? val : null;
        public List<string> GetGrandChildElementValues(XElement parent, string childElementName, string grandChieldElementName, bool alsoIncludeDirectChildrenWithGrantChildElementName)
        {
            var vals = GetChildElement(parent, childElementName)?.Elements(grandChieldElementName)
                .Where(grandchild => !string.IsNullOrEmpty(grandchild.Value))
                .Select(grandchild => grandchild.Value).ToList();

            if (alsoIncludeDirectChildrenWithGrantChildElementName)
            {
                var directVals = parent.Elements(grandChieldElementName)
                    .Where(child => !string.IsNullOrEmpty(child.Value))
                    .Select(child => child.Value).ToList();

                if (directVals?.Count > 0)
                {
                    if (vals != null)
                        vals = vals.Union(directVals).ToList();
                    else
                        vals = directVals;
                }
            }
            return vals != null ? vals : new List<string>();
        }
        public string? GetElementAttStr(XElement parent, string elementName, string attName) => GetChildElement(parent, elementName)?.Attribute(attName)?.Value;
        public DateTime? GetElementAttDateYYYYDDMM(XElement parent, string elementName, string attName)
        {
            var datestr = GetElementAttStr(parent, elementName, attName);
            if (string.IsNullOrEmpty(datestr))
                return null;

            return new DateTime(int.Parse(datestr!.Substring(0, 4)), int.Parse(datestr!.Substring(4, 2)), int.Parse(datestr!.Substring(6, 2)));
        }

        public string? MergeStrings(string? str1, string? str2, int instance)
        {
            if (string.IsNullOrEmpty(str1))
                return str2;
            else if (string.IsNullOrEmpty(str2))
                return str1;
            else if (Regex.Replace(str1, @"\s+", "") != Regex.Replace(str2, @"\s+", "")) //compares excluding white spaces
                return $"{str1} ||| {str2}";
            else
                return str1;
        }

        public List<string> MergeArrays(List<string> arr1, List<string> arr2, int instance)
        {
            //return arr1.Union(arr2.Select(str => $"#{instance}# {str}").ToList()).ToList();
            return arr1.Union(arr2).ToList();
        }
    }

    /**
     * Movie class
     */
    public class Movie : XmlDoc
    {
        public int movie_id { get; set; }
        public int parent_id { get; set; }
        public string? title { get; set; }
        public List<string> pictures { get; set; }
        public List<string> hipictures { get; set; }
        public string? rating { get; set; }
        public string? advisory { get; set; }
        public List<string> genres { get; set; }
        public List<string> casts { get; set; }
        public List<string> directors { get; set; }
        public DateTime? release_date { get; set; }
        public string? release_notes { get; set; }
        public string? release_dvd { get; set; }
        public int running_time { get; set; }
        public string? official_site { get; set; }
        public List<string> distributors { get; set; }
        public List<string> producers { get; set; }
        public List<string> writers { get; set; }
        public string? synopsis { get; set; }
        public string? lang { get; set; }
        public string? intl_country { get; set; }
        public string? intl_name { get; set; }
        public string? intl_cert { get; set; }
        public string? intl_advisory { get; set; }
        public DateTime? intl_release { get; set; }
        public string? intl_poster { get; set; }

        public Movie() { }
        public Movie(XElement xml)
        {
            movie_id = GetElementValueInt(xml, "movie_id", -1);
            parent_id = GetElementValueInt(xml, "parent_id", -1);
            title = GetElementValueStr(xml, "title");
            pictures = GetGrandChildElementValues(xml, "pictures", "photos", true);
            hipictures = GetGrandChildElementValues(xml, "hipictures", "photos", true);
            rating = GetElementValueStr(xml, "rating");
            advisory = GetElementValueStr(xml, "advisory");
            genres = GetGrandChildElementValues(xml, "genres", "genre", true);
            casts = GetGrandChildElementValues(xml, "casts", "cast", true);
            directors = GetGrandChildElementValues(xml, "directors", "director", true);
            release_date = GetElementValueDateTime(xml, "release_date");
            release_notes = GetElementValueStr(xml, "release_notes");
            release_dvd = GetElementValueStr(xml, "release_dvd");
            running_time = GetElementValueInt(xml, "running_time", -1);
            official_site = GetElementValueStr(xml, "official_site");
            distributors = GetGrandChildElementValues(xml, "distributors", "distributor", true);
            producers = GetGrandChildElementValues(xml, "producers", "producer", true);
            writers = GetGrandChildElementValues(xml, "writers", "writer", true);
            synopsis = GetElementValueStr(xml, "synopsis");
            lang = GetElementValueStr(xml, "lang");
            intl_country = GetElementAttStr(xml, "intl", "country");

            XElement intl = GetChildElement(xml, "intl");
            if (intl != null)
            {
                intl_name = GetElementValueStr(intl, "intl_name");
                intl_cert = GetElementValueStr(intl, "intl_cert");
                intl_advisory = GetElementValueStr(intl, "intl_advisory");
                intl_release = GetElementValueDateTime(intl, "intl_release");
                intl_poster = GetElementValueStr(intl, "intl_poster");
            }
        }

        public void Merge(Movie src, int instance)
        {
            //Merging values

            title = MergeStrings(title, src.title, instance);

            if (src.pictures?.Count > 0) pictures = MergeArrays(pictures, src.pictures, instance);
            if (src.hipictures?.Count > 0) hipictures = MergeArrays(hipictures, src.hipictures, instance); // hipictures.Union(src.hipictures).ToList();

            rating = MergeStrings(rating, src.rating, instance);
            advisory = MergeStrings(advisory, src.advisory, instance);

            if (src.genres?.Count > 0) genres = MergeArrays(genres, src.genres, instance);
            if (src.casts?.Count > 0) casts = MergeArrays(casts, src.casts, instance);
            if (src.directors?.Count > 0) directors = MergeArrays(directors, src.directors, instance);
            if (!release_date.HasValue) release_date = src.release_date;

            release_notes = MergeStrings(release_notes, src.release_notes, instance);
            release_dvd = MergeStrings(release_dvd, src.release_dvd, instance);

            if (running_time < 0) running_time = src.running_time;

            official_site = MergeStrings(official_site, src.official_site, instance);

            if (src.distributors?.Count > 0) distributors = MergeArrays(distributors, src.distributors, instance);
            if (src.producers?.Count > 0) producers = MergeArrays(producers, src.producers, instance);
            if (src.writers?.Count > 0) writers = MergeArrays(writers, src.writers, instance);

            synopsis = MergeStrings(synopsis, src.synopsis, instance);
            lang = MergeStrings(lang, src.lang, instance);
            intl_country = MergeStrings(intl_country, src.intl_country, instance);
            intl_name = MergeStrings(intl_name, src.intl_name, instance);
            intl_cert = MergeStrings(intl_cert, src.intl_cert, instance);
            intl_advisory = MergeStrings(intl_advisory, src.intl_advisory, instance);
            intl_poster = MergeStrings(intl_poster, src.intl_poster, instance);

            if (!intl_release.HasValue) intl_release = src.intl_release;
        }
    }

    /**
     * Theater class
     */
    public class Theater : XmlDoc
    {
        public int theater_id { get; set; }
        public string? theater_name { get; set; }
        public string? theater_address { get; set; }
        public string? theater_city { get; set; }
        public string? theater_state { get; set; }
        public string? theater_zip { get; set; }
        public string? theater_phone { get; set; }
        public string? theater_attributes { get; set; }
        public string? theater_ticketing { get; set; }
        public string? theater_closed_reason { get; set; }
        public string? theater_area { get; set; }
        public string? theater_location { get; set; }
        public string? theater_market { get; set; }
        public int? theater_screens { get; set; }
        public string? theater_seating { get; set; }
        public string? theater_adult { get; set; }
        public string? theater_child { get; set; }
        public string? theater_senior { get; set; }
        public string? theater_country { get; set; }
        public string? theater_url { get; set; }
        public string? theater_chain_id { get; set; }

        public string? theater_adult_bargain { get; set; }
        public string? theater_senior_bargain { get; set; }
        public string? theater_child_bargain { get; set; }
        public string? theater_special_bargain { get; set; }
        public string? theater_adult_super { get; set; }
        public string? theater_senior_super { get; set; }
        public string? theater_child_super { get; set; }
        public string? theater_price_comment { get; set; }
        public string? theater_extra { get; set; }
        public string? theater_desc { get; set; }
        public string? theater_type { get; set; }
        public decimal? theater_lat { get; set; }
        public decimal? theater_lon { get; set; }

        public Theater() { }

        public Theater(XElement xml)
        {
            theater_id = GetElementValueInt(xml, "theater_id", -1);
            theater_name = GetElementValueStr(xml, "theater_name");
            theater_address = GetElementValueStr(xml, "theater_address");
            theater_city = GetElementValueStr(xml, "theater_city");
            theater_state = GetElementValueStr(xml, "theater_state");
            theater_zip = GetElementValueStr(xml, "theater_zip");
            theater_phone = GetElementValueStr(xml, "theater_phone");
            theater_attributes = GetElementValueStr(xml, "theater_attributes");
            theater_ticketing = GetElementValueStr(xml, "theater_ticketing");
            theater_closed_reason = GetElementValueStr(xml, "theater_closed_reason");
            theater_area = GetElementValueStr(xml, "theater_area");
            theater_location = GetElementValueStr(xml, "theater_location");
            theater_market = GetElementValueStr(xml, "theater_market");
            theater_screens = GetElementValueInt(xml, "theater_screens", -1);
            theater_seating = GetElementValueStr(xml, "theater_seating");
            theater_adult = GetElementValueStr(xml, "theater_adult");
            theater_child = GetElementValueStr(xml, "theater_child");
            theater_senior = GetElementValueStr(xml, "theater_senior");
            theater_country = GetElementValueStr(xml, "theater_country");
            theater_url = GetElementValueStr(xml, "theater_url");
            theater_chain_id = GetElementAttStr(xml, "theater_clain", "id");

            theater_adult_bargain = GetElementValueStr(xml, "theater_adult_bargain");
            theater_senior_bargain = GetElementValueStr(xml, "theater_senior_bargain");
            theater_child_bargain = GetElementValueStr(xml, "theater_child_bargain");
            theater_special_bargain = GetElementValueStr(xml, "theater_special_bargain");
            theater_adult_super = GetElementValueStr(xml, "theater_adult_super");
            theater_senior_super = GetElementValueStr(xml, "theater_senior_super");
            theater_child_super = GetElementValueStr(xml, "theater_child_super");
            theater_price_comment = GetElementValueStr(xml, "theater_price_comment");
            theater_extra = GetElementValueStr(xml, "theater_extra");
            theater_desc = GetElementValueStr(xml, "theater_desc");
            theater_type = GetElementValueStr(xml, "theater_type");
            theater_lon = GetElementValueDecimal(xml, "theater_lon");
            theater_lat = GetElementValueDecimal(xml, "theater_lat");
        }

        public void Merge(Theater src, int instance)
        {
            theater_name = MergeStrings(theater_name, src.theater_name, instance);
            theater_address = MergeStrings(theater_address, src.theater_address, instance);
            theater_city = MergeStrings(theater_city, src.theater_city, instance);
            theater_state = MergeStrings(theater_state, src.theater_state, instance);
            theater_zip = MergeStrings(theater_zip, src.theater_zip, instance);
            theater_phone = MergeStrings(theater_phone, src.theater_phone, instance);
            theater_attributes = MergeStrings(theater_attributes, src.theater_attributes, instance);
            theater_ticketing = MergeStrings(theater_ticketing, src.theater_ticketing, instance);
            theater_closed_reason = MergeStrings(theater_closed_reason, src.theater_closed_reason, instance);
            theater_area = MergeStrings(theater_area, src.theater_area, instance);
            theater_location = MergeStrings(theater_location, src.theater_location, instance);
            theater_market = MergeStrings(theater_market, src.theater_market, instance);
             
            if (!theater_screens.HasValue) theater_screens = src.theater_screens;

            theater_seating = MergeStrings(theater_seating, src.theater_seating, instance);
            theater_adult = MergeStrings(theater_adult, src.theater_adult, instance);
            theater_child = MergeStrings(theater_child, src.theater_child, instance);
            theater_senior = MergeStrings(theater_senior, src.theater_senior, instance);
            theater_country = MergeStrings(theater_country, src.theater_country, instance);
            theater_url = MergeStrings(theater_url, src.theater_url, instance);
            theater_chain_id = MergeStrings(theater_chain_id, src.theater_chain_id, instance);
            theater_adult_bargain = MergeStrings(theater_adult_bargain, src.theater_adult_bargain, instance);
            theater_senior_bargain = MergeStrings(theater_senior_bargain, src.theater_senior_bargain, instance);
            theater_child_bargain = MergeStrings(theater_child_bargain, src.theater_child_bargain, instance);
            theater_special_bargain = MergeStrings(theater_special_bargain, src.theater_special_bargain, instance);
            theater_adult_super = MergeStrings(theater_adult_super, src.theater_adult_super, instance);
            theater_senior_super = MergeStrings(theater_senior_super, src.theater_senior_super, instance);
            theater_child_super = MergeStrings(theater_child_super, src.theater_child_super, instance);
            theater_price_comment = MergeStrings(theater_price_comment, src.theater_price_comment, instance);
            theater_extra = MergeStrings(theater_extra, src.theater_extra, instance);
            theater_desc = MergeStrings(theater_desc, src.theater_desc, instance);
            theater_type = MergeStrings(theater_type, src.theater_type, instance);

            if (!theater_lat.HasValue) theater_lat = src.theater_lat;
            if (!theater_lon.HasValue) theater_lon = src.theater_lon;
        }
    }

    /**
     * Showtime class
     */
    public class Showtime: XmlDoc
    {

        public int movie_id { get; set; }
        public string? movie_name { get; set; }
        public int theater_id { get; set; }
        public DateTime? show_date { get; set; }
        public string[]? showtimes { get; set; }
        public int[]? showtime_minutes { get; set; } = null;
        public string[]? show_attributes { get; set; }
        public string? show_passes { get; set; }
        public string? show_festival { get; set; }
        public string? show_with { get; set; }
        public string? show_sound { get; set; }
        public string[]? show_comments { get; set; }

        public Showtime() { }

        public Showtime(XElement xml)
        {
            movie_id = GetElementValueInt(xml, "movie_id", -1);
            movie_name = GetElementValueStr(xml, "movie_name");
            theater_id = GetElementValueInt(xml, "theater_id", -1);

            //Most of the time show-date is defined in a date attribute of a chiled element called show_date
            show_date = GetElementAttDateYYYYDDMM(xml, "show_date", "date");

            if(!show_date.HasValue)
            {
                //some times, the show date is defined as the element value 
                var dateStr = GetElementValueStr(xml, "show_date");
                if (!string.IsNullOrEmpty(dateStr))
                {
                    if (DateTime.TryParse(dateStr, out DateTime d))
                        show_date = d;
                    else
                        throw new Exception($"Date string {dateStr} cannot be parsed as a DateTime object");
                }
            }
            
            //Most of the time, the showtimes element is encapsulated in the show_date element
            XElement show_date_element = GetChildElement(xml, "show_date");
            showtimes = GetElementValueStr(show_date_element, "showtimes", ",");
            if(showtimes == null || showtimes?.Length == 0)
            {
                //sometime, the showtimes element is directly in the main showtime element
                showtimes = GetElementValueStr(xml, "showtimes", ",");
            }

            var showTimeMunitesList = new List<int>();
            if (showtimes != null)
            {
                foreach (var st in showtimes!)
                {
                    var hhmm = st.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim('.').Trim(','))).ToArray();
                    if (hhmm?.Length > 0)
                        showTimeMunitesList.Add(hhmm[0] * 60 + hhmm[1]);
                }
            }
            showtime_minutes= showTimeMunitesList.ToArray();

            show_attributes = GetElementValueStr(show_date_element, "show_attributes", ",");
            show_passes = GetElementValueStr(show_date_element, "show_passes");
            show_festival = GetElementValueStr(show_date_element, "show_festival");
            show_with = GetElementValueStr(show_date_element, "show_with");
            show_sound = GetElementValueStr(show_date_element, "show_sound");
            show_comments = GetElementValueStr(show_date_element, "show_comments", ";");

        }

    }


    public class MovieRecord
    {
        public int id { get; set; }
        public int movie_id { get; set; }
        public int batch { get; set; }
        public int instances { get; set; }
        public string content { get; set; }

        public void Merge(Movie src)
        {
            ++instances;
            Movie myMovie = JsonSerializer.Deserialize<Movie>(content);
            myMovie!.Merge(src, instances);
            content = JsonSerializer.Serialize(myMovie);
        }
    }
    public class TheaterRecord
    {
        public int id { get; set; }
        public int theater_id { get; set; }
        public int batch { get; set; }
        public int instances { get; set; }
        public string content { get; set; }

        public void Merge(Theater src)
        {
            ++instances;
            Theater myTheater = JsonSerializer.Deserialize<Theater>(content);
            myTheater!.Merge(src, instances);
            content = JsonSerializer.Serialize(myTheater);
        }
    }

    public class ShowtimeRecord
    {
        public int id { get; set; }
        public int movie_id { get; set; }
        public int theater_id { get; set; }
        public DateTime? show_date { get; set; }
        public int batch { get; set; }
        public bool? movie_error { get; set; }
        public bool? theater_error { get; set; }
        public bool? is_validated { get; set; }
        public string content { get; set; }
    }

    public class TrackingKey
    {
        public int id { get; set; }
        public string entry_key { get; set; }
    }

    public class ShowtimeDbContext : DbContext
    {
        public DbSet<MovieRecord> MovieRecords { get; set; }
        public DbSet<TheaterRecord> TheaterRecords { get; set; }
        public DbSet<ShowtimeRecord> ShowtimeRecords { get; set; }
        public DbSet<TrackingKey> TrackingKeys { get; set; }

        public ShowtimeDbContext(DbContextOptions<ShowtimeDbContext> options)
            : base(options)
        {
        }
    }


    public class TransactionalTestDatabaseFixture
    {
        private const string ConnectionString = @"Server=.\\;Database=showtime;User Id=showtime;Password=password;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ShowtimeDbContext CreateContext()
            => new ShowtimeDbContext(
                new DbContextOptionsBuilder<ShowtimeDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

        public TransactionalTestDatabaseFixture()
        {
            using var context = CreateContext();
           // context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //Cleanup();
        }

        public void Cleanup()
        {
            using var context = CreateContext();

            ////    context.AddRange(
            ////        new Blog { Name = "Blog1", Url = "http://blog1.com" },
            ////        new Blog { Name = "Blog2", Url = "http://blog2.com" });
            ////    context.SaveChanges();
        }
    }

        


    }
