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
    public class ShowtimeDataProcessing
    {

        public readonly TestHelper _testHelper;
        public int MAX_RECORDS = 1; //DEBUG ONLY -- set it to 0 or -1 to ignore it

        public ShowtimeDataProcessing()
        {
            _testHelper = new TestHelper();
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexFlattenedShowtimes
        [Fact]
        public void IndexFlattenedShowtimes()
        {
            DateTime start = DateTime.Now;

            PrepareForIndexing(out string srcFolderRoot, out string outputFolder, out int maxParallelProcess, out List<string[]> sourceBatches, out string[] skipFiles);

            var tasks = sourceBatches.Select(x => IndexFlattenedShowtimesBatch(x, outputFolder, start, skipFiles));
            Task.WhenAll(tasks).Wait();
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexMovies
        [Fact]
        public void IndexMovies()
        {
            DateTime start = DateTime.Now;

            PrepareForIndexing(out string srcFolderRoot, out string outputFolder, out int maxParallelProcess, out List<string[]> sourceBatches, out string[] skipFiles);

            var tasks = sourceBatches.Select(x => IndexMoviesBatch(x, outputFolder, start));
            Task.WhenAll(tasks).Wait();
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexTheaters
        [Fact]
        public async void IndexTheaters()
        {
            DateTime start = DateTime.Now;

            PrepareForIndexing(out string srcFolderRoot, out string outputFolder, out int maxParallelProcess, out List<string[]> sourceBatches, out string[] skipFiles, false);

            //Theaters should NOT be processed in parallel since we want to skip duplicate theater records
            //that appear across batches. Therefore, if the configureation specifies more than one parallel
            //batch, we override it.
            Assert.True(maxParallelProcess == 1, "Expected to have no parallel processes");

            string[] sources = sourceBatches[0];
            ////if (maxParallelProcess > 1)
            ////{
            ////    List<string> mergedSources = new List<string>();
            ////    sourceBatches.ForEach(src => mergedSources.AddRange(src));
            ////    sources = mergedSources.ToArray();
            ////}
            ////else
            ////    sources = sourceBatches[0];

            await IndexTheatersBatch(sources, outputFolder, start);
        }

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeDataProcessing.IndexChineseRecords
        [Fact]
        public async void IndexChineseRecords()
        {
            //Since the chinese data set is already flattened, we try to extract them here.

            DateTime start = DateTime.Now;
            string logFilePrefix = $"chinese-records-";

            PrepareForIndexing(out string srcFolderRoot, out string outputFolder, out int maxParallelProcess, out List<string[]> sourceBatches, out string[] skipFiles, false);
           
            Assert.True(maxParallelProcess == 1, "Expected to have no parallel processes");

            string timestampStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}-errors.txt");

            var solrService = _testHelper.Solr;

            string[] sources = sourceBatches[0];
            foreach (var srcFolder in sources)
            {
                int srcFolderPathCharacterLength = srcFolder.LastIndexOf("\\") + 1;
                var zipFiles = Directory.GetFiles(srcFolder);
                foreach (var zipFile in zipFiles.Where(file => file.EndsWith("chn.zip")))
                {
                    string zipfile_key = zipFile.Substring(srcFolderPathCharacterLength);

                    using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            List<SolrDoc> solrDocs = new List<SolrDoc>();
                            int n = 0;
                            try
                            {
                                string entry_key = $"{zipfile_key}\\{entry.Name}";
                                XElement xml = LoadXmlFromZipEntry(entry);
                                foreach (var child in xml.Elements())
                                {
                                    string entryType = child.Name.ToString().ToLower();
                                    if (entryType == "showtime")
                                    {
                                        SolrDoc solrDoc = new SolrDoc();
                                        solrDocs.Add(solrDoc);

                                        solrDoc.AddId(Guid.NewGuid().ToString());
                                        solrDoc.AddField("entry_type_s", "showtime");
                                        solrDoc.AddField("entry_src_s", entry_key);

                                        var showtime = new Showtime(child);
                                        AddShowtime(solrDoc, showtime);

                                        solrDoc.AddField("movie_cnname_t", showtime.GetElementValueStr(child, "movie_cnname"));
                                        solrDoc.AddField("theater_cnname_t", showtime.GetElementValueStr(child, "theater_cnname"));
                                        solrDoc.AddField("theater_name_t", showtime.GetElementValueStr(child, "theater_name"));
                                        solrDoc.AddField("theater_state_t", showtime.GetElementValueStr(child, "theater_province"));
                                        solrDoc.AddField("theater_city_t", showtime.GetElementValueStr(child, "theater_city"));
                                        solrDoc.AddField("theater_area_t", showtime.GetElementValueStr(child, "theater_area"));
                                        solrDoc.AddField("release_date_dt", showtime.GetElementValueStr(child, "release_date"));

                                    }

                                    //Indexing the showtimes batch
                                    if (solrDocs.Count >= 1000)
                                    {
                                        await solrService.Index(solrDocs);
                                        await solrService.CommitAsync();

                                        await File.AppendAllTextAsync(processingLogFile, $"Indexed {n+1} to {n + solrDocs.Count} entries in {entry_key}{Environment.NewLine}");
                                        n += solrDocs.Count;

                                        solrDocs.Clear();
                                        GC.Collect();
                                    }
                                } //End: foreach (var child in xml.Elements())

                                if (solrDocs.Count >= 0)
                                {
                                    await solrService.Index(solrDocs);
                                    await solrService.CommitAsync();

                                    await File.AppendAllTextAsync(processingLogFile, $"Indexed {n + 1} to {n + solrDocs.Count} entries in {entry_key}{Environment.NewLine}");
                                    n += solrDocs.Count;

                                    solrDocs.Clear();
                                    GC.Collect();
                                }
                            }
                            catch(Exception ex)
                            {
                                await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {entry.Name}: {ex.Message}{Environment.NewLine}");
                                solrDocs.Clear();
                                GC.Collect();
                            }                           
                        }
                    }
                }
            }
        }

        private void PrepareForIndexing(out string srcFolderRoot, out string outputFolder, out int maxParallelProcess, out List<string[]> sourceBatches, out string[] skipFiles, bool preapreForParellelProcessing = true)
        {
            if (!preapreForParellelProcessing || !int.TryParse(_testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:MaxParallelProcesses")?.Value, out maxParallelProcess))
                maxParallelProcess = 1;

            outputFolder = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:OutputFolder")?.Value!;
            if (string.IsNullOrEmpty(outputFolder))
                outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            srcFolderRoot = _testHelper.Configuration.GetSection("ShowtimeDbIngesionSettings:SourceFolderRoot")?.Value!;
            if (string.IsNullOrEmpty(srcFolderRoot))
                srcFolderRoot = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));

            var skipFilesLog = _testHelper!.Configuration.GetSection("ShowtimeDbIngesionSettings:SkipFilesLogFile")?.Value!;
            skipFiles = string.IsNullOrEmpty(skipFilesLog) ? new string[0] : File.ReadAllLines(skipFilesLog);

            var srcFolders = Directory.GetDirectories(srcFolderRoot);
            sourceBatches = new List<string[]>();
            int batchSize = (int)Math.Ceiling((double)srcFolders.Length / maxParallelProcess);
            int offset = 0;
            while (offset < srcFolders.Length)
            {
                sourceBatches.Add(srcFolders.Skip(offset).Take(batchSize).ToArray());
                offset += batchSize;
            }
        }

        private async Task IndexFlattenedShowtimesBatch(string[] folderList, string outputFolder, DateTime start, string[] skipFiles)
        {
            int srcFolderPathCharacterLength = folderList[0].LastIndexOf("\\") + 1;
            string first = folderList[0].Substring(srcFolderPathCharacterLength);
            string last = folderList[folderList.Length - 1].Substring(srcFolderPathCharacterLength);

            string logFilePrefix = $"flattened-showtimes-{first}-{last}";

            string timestampStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}-errors.txt");

            string trackingFile = Path.Combine(outputFolder, $"{logFilePrefix}-tracking-keys.txt");
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

                    foreach (var skip in skipFiles)
                        zipFiles = zipFiles.Where(file => !file.EndsWith(skip)).ToArray();

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

                                                if (matchingTheaters.Any())
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

        private async Task IndexMoviesBatch(string[] folderList, string outputFolder, DateTime start)
        {
            int srcFolderPathCharacterLength = folderList[0].LastIndexOf("\\") + 1;
            string first = folderList[0].Substring(srcFolderPathCharacterLength);
            string last = folderList[folderList.Length - 1].Substring(srcFolderPathCharacterLength);

            string logFilePrefix = $"movies-{first}-{last}";

            string timestampStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}-errors.txt");

            string trackingFile = Path.Combine(outputFolder, $"{logFilePrefix}-tracking-keys.txt");
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

                        try
                        {
                            if (trackingKeys.Contains(zipfile_key))
                                continue;

                            await File.AppendAllTextAsync(processingLogFile, $"Archive {zipFile}.{Environment.NewLine}");
                            bool zipFilerProcessingSuccessful = true;

                            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                            {
                                List<ZipArchiveEntry> moviesArchiveEntries = null;

                                if (folder_key == "0_backfill")
                                    moviesArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("IMOVIES.XML")).ToList();
                                else
                                    moviesArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("I.XML")).ToList();

                                //Loading movies
                                foreach (var archiveEntry in moviesArchiveEntries)
                                {
                                    string entry_key = $"{zipfile_key}\\{archiveEntry.Name}";
                                    if (trackingKeys.Contains(entry_key))
                                        continue;

                                    List<SolrDoc> solrDocs = new List<SolrDoc>();
                                    try
                                    {
                                        XElement xml = LoadXmlFromZipEntry(archiveEntry);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();
                                            if (entryType == "movie")
                                            {
                                                SolrDoc solrDoc = new SolrDoc();
                                                solrDocs.Add(solrDoc);

                                                solrDoc.AddId(Guid.NewGuid().ToString());
                                                solrDoc.AddField("entry_type_s", "movie");
                                                solrDoc.AddField("entry_src_s", entry_key);
                                                AddMovie(solrDoc, new Movie(child));
                                            }
                                        }

                                        //Indexing the movies batch
                                        await solrService.Index(solrDocs);
                                        await solrService.CommitAsync();
                                        await File.AppendAllTextAsync(trackingFile, $"{entry_key}{Environment.NewLine}");

                                    }
                                    catch (Exception ex)
                                    {
                                        zipFilerProcessingSuccessful = false;
                                        folderProcessingSuccessful = false;
                                        await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {archiveEntry.Name}: {ex.Message}{Environment.NewLine}");
                                    }

                                    solrDocs.Clear();
                                    GC.Collect();
                                }
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

        private async Task IndexTheatersBatchOld(string[] folderList, string outputFolder, DateTime start)
        {
            int srcFolderPathCharacterLength = folderList[0].LastIndexOf("\\") + 1;
            string first = folderList[0].Substring(srcFolderPathCharacterLength);
            string last = folderList[folderList.Length - 1].Substring(srcFolderPathCharacterLength);

            string logFilePrefix = $"theaters-{first}-{last}";

            string timestampStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}-errors.txt");

            string trackingFile = Path.Combine(outputFolder, $"{logFilePrefix}-tracking-keys.txt");
            if (!File.Exists(trackingFile))
                File.Create(trackingFile).Close();
            string[] trackingKeys = File.ReadAllLines(trackingFile);


            //Theater ingestion will not be able to avoid duplicates in previosuly ingested theater batches.
            //Therefore, put a warning in the error file if theaters are already found in the index
            string query = "entry_type_s:theater";
            SearchResult queryresult = await _testHelper.Solr.ExecuteSearch(query, 0, 1);
            if (queryresult.TotalMatches > 0)
                await File.WriteAllTextAsync(errorLogFile, $"WARNING: {queryresult.TotalMatches} theater records already found in the index. These entries will remain as duplicates.");


            var solrService = _testHelper.Solr;
            List<Theater> uniqueTheaters = new List<Theater>();
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

                        try
                        {
                            if (trackingKeys.Contains(zipfile_key))
                                continue;

                            await File.AppendAllTextAsync(processingLogFile, $"Archive {zipFile}.{Environment.NewLine}");
                            bool zipFilerProcessingSuccessful = true;

                            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                            {
                                List<ZipArchiveEntry> theatersArchiveEntries = null;

                                if (folder_key == "0_backfill")
                                    theatersArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("THEATER.XML")).ToList();
                                else
                                    theatersArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("T.XML")).ToList();

                                //Loading movies
                                foreach (var archiveEntry in theatersArchiveEntries)
                                {
                                    string entry_key = $"{zipfile_key}\\{archiveEntry.Name}";
                                    if (trackingKeys.Contains(entry_key))
                                        continue;

                                    List<SolrDoc> solrDocs = new List<SolrDoc>();
                                    try
                                    {
                                        XElement xml = LoadXmlFromZipEntry(archiveEntry);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();
                                            if (entryType == "theater")
                                            {
                                                Theater theater = new Theater(child);

                                                var matchingEntries = uniqueTheaters.Where(th => th.theater_id == theater.theater_id).ToList();
                                                bool isNewTheater = true;
                                                for (int i = 0; i < matchingEntries.Count && isNewTheater; i++)
                                                    isNewTheater = !theater.IsSameAs(matchingEntries[i]);

                                                if (isNewTheater)
                                                {
                                                    uniqueTheaters.Add(theater);

                                                    SolrDoc solrDoc = new SolrDoc();
                                                    solrDocs.Add(solrDoc);

                                                    solrDoc.AddId(Guid.NewGuid().ToString());
                                                    solrDoc.AddField("entry_type_s", "theater");
                                                    solrDoc.AddField("entry_src_s", entry_key);
                                                    solrDoc.AddField("entry_occurrence_i", uniqueTheaters.Count);
                                                    AddTheater(solrDoc, theater);
                                                }
                                            }
                                        }

                                        //Indexing the theater batch
                                        if (solrDocs.Count > 0)
                                        {
                                            int waitTimeTimeoutMills = 10 * 60 * 1000;
                                            solrService.Index(solrDocs).Wait(waitTimeTimeoutMills);
                                            solrService.CommitAsync().Wait(waitTimeTimeoutMills);
                                        }
                                        await File.AppendAllTextAsync(trackingFile, $"{entry_key}{Environment.NewLine}");

                                    }
                                    catch (Exception ex)
                                    {
                                        zipFilerProcessingSuccessful = false;
                                        folderProcessingSuccessful = false;
                                        await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {archiveEntry.Name}: {ex.Message}{Environment.NewLine}");
                                    }

                                    solrDocs.Clear();
                                    GC.Collect();
                                }
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

        private async Task IndexTheatersBatch(string[] folderList, string outputFolder, DateTime start)
        {
            int srcFolderPathCharacterLength = folderList[0].LastIndexOf("\\") + 1;
            string first = folderList[0].Substring(srcFolderPathCharacterLength);
            string last = folderList[folderList.Length - 1].Substring(srcFolderPathCharacterLength);

            string logFilePrefix = $"theaters-{first}-{last}";

            string timestampStr = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"{logFilePrefix}-processing-{timestampStr}-errors.txt");

            string trackingFile = Path.Combine(outputFolder, $"{logFilePrefix}-tracking-keys.txt");
            if (!File.Exists(trackingFile))
                File.Create(trackingFile).Close();
            string[] trackingKeys = File.ReadAllLines(trackingFile);

            Dictionary<int, Theater> theaters = new Dictionary<int, Theater>();

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

                        try
                        {
                            if (trackingKeys.Contains(zipfile_key))
                                continue;

                            await File.AppendAllTextAsync(processingLogFile, $"Archive {zipFile}.{Environment.NewLine}");
                            bool zipFilerProcessingSuccessful = true;

                            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                            {
                                List<ZipArchiveEntry> theatersArchiveEntries = null;

                                if (folder_key == "0_backfill")
                                    theatersArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("THEATER.XML")).ToList();
                                else
                                    theatersArchiveEntries = archive.Entries.Where(entry => entry.Name.ToUpper().EndsWith("T.XML")).ToList();

                                //Loading movies
                                foreach (var archiveEntry in theatersArchiveEntries)
                                {
                                    string entry_key = $"{zipfile_key}\\{archiveEntry.Name}";
                                    if (trackingKeys.Contains(entry_key))
                                        continue;

                                    List<SolrDoc> solrDocs = new List<SolrDoc>();
                                    try
                                    {
                                        XElement xml = LoadXmlFromZipEntry(archiveEntry);
                                        foreach (var child in xml.Elements())
                                        {
                                            string entryType = child.Name.ToString().ToLower();
                                            if (entryType == "theater")
                                            {
                                                Theater theater = new Theater(child);

                                                //Check whether this theater already
                                                if(theaters.ContainsKey(theater.theater_id))
                                                {
                                                    var existing = theaters[theater.theater_id];
                                                    existing.Merge(theater);
                                                }
                                                else
                                                    theaters.Add(theater.theater_id, theater);

                                                SolrDoc solrDoc = new SolrDoc() ;
                                                solrDocs.Add(solrDoc);

                                                solrDoc.AddId(theater.theater_id);
                                                solrDoc.AddField("entry_type_s", "theater");
                                                solrDoc.AddField("entry_src_s", entry_key);
                                                AddTheater(solrDoc, theater);                                               
                                            }
                                        }

                                        //Indexing the theater batch
                                        if (solrDocs.Count > 0)
                                        {
                                            int waitTimeTimeoutMills = 10 * 60 * 1000;
                                            solrService.Index(solrDocs).Wait(waitTimeTimeoutMills);
                                            solrService.CommitAsync().Wait(waitTimeTimeoutMills);
                                        }
                                        await File.AppendAllTextAsync(trackingFile, $"{entry_key}{Environment.NewLine}");

                                    }
                                    catch (Exception ex)
                                    {
                                        zipFilerProcessingSuccessful = false;
                                        folderProcessingSuccessful = false;
                                        await File.AppendAllTextAsync(errorLogFile, $"EXCEPTION in {zipFile} > {archiveEntry.Name}: {ex.Message}{Environment.NewLine}");
                                    }

                                    solrDocs.Clear();
                                    GC.Collect();
                                }
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

            int range = (int)Math.Ceiling(totalCount / (double)maxParallelProcesses);

            List<int> offsets = new List<int>();
            for (int i = 0; i < maxParallelProcesses; ++i)
                offsets.Add(i * range);

            //DetectDuplicates(entryType, 0, range).Wait();
            var tasks = offsets.Select(x => DetectDuplicates(entryType, x, range));
            Task.WhenAll(tasks).Wait();
        }

        [Fact]
        public void ExtractUniqueErrorFiles()
        {
            var srcFolder = "C:\\Projects\\Showtime Database\\error-logs";
            var dstFolder = "C:\\Projects\\Showtime Database\\error-logs-unique";
            var startPrefix = "2";

            Directory.CreateDirectory(dstFolder);
            var srcFiles = Directory.GetFiles(srcFolder);
            var uniqueEntries = new List<string>();

            foreach (var file in srcFiles)
            {
                var lines = File.ReadAllLines(file);

                foreach(var line in lines)
                {
                    if (!string.IsNullOrEmpty(startPrefix) && !line.StartsWith(startPrefix))
                        continue;

                    var subLine = line.Substring(0, line.IndexOf(".zip") + 4);
                    if(!uniqueEntries.Contains(subLine))
                        uniqueEntries.Add(subLine);
                }
            }

            var outputFile = Path.Combine(dstFolder, "unique-errors.txt");
            File.WriteAllLines(outputFile, uniqueEntries);
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
            string filePrefix = $"duplicate-check-{entryType}s_{offset}-to-{offset + maxCount}_{startTimeStr}";
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

        private void AddShowtime(SolrDoc doc, Showtime showtime)
        {
            var showdate_str = showtime.show_date.HasValue ? showtime.show_date.Value.ToString("yyyy-MM-dd") : "yyyy-mm-dd";
            doc.AddField("showtime_key_t", $"{showtime.movie_id}-{showtime.theater_id}-{showdate_str}");

            doc.AddField("movie_id_i", showtime.movie_id);
            doc.AddField("theater_id_i", showtime.theater_id);
            doc.AddField("movie_name_t", showtime.movie_name);
            doc.AddField("show_date_dt", showtime.show_date);
            if (showtime.showtimes != null)
                doc.AddField("showtimes_count_i", showtime.showtimes!.Length);
            doc.AddField("showtimes_ts", showtime.showtimes);
            doc.AddField("showtime_minutes_is", showtime.showtime_minutes);
            doc.AddField("show_attributes_ts", showtime.show_attributes);
            doc.AddField("show_passes_t", showtime.show_passes);
            doc.AddField("show_festival_t", showtime.show_festival);
            doc.AddField("show_with_t", showtime.show_with);
            doc.AddField("show_sound_t", showtime.show_sound);
            doc.AddField("show_comments_ts", showtime.show_comments);
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

        public string? MergeStrings(string? str1, string? str2)
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

        public List<string> MergeArrays(List<string> arr1, List<string> arr2)
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

            title = MergeStrings(title, src.title);

            if (src.pictures?.Count > 0) pictures = MergeArrays(pictures, src.pictures);
            if (src.hipictures?.Count > 0) hipictures = MergeArrays(hipictures, src.hipictures); // hipictures.Union(src.hipictures).ToList();

            rating = MergeStrings(rating, src.rating);
            advisory = MergeStrings(advisory, src.advisory);

            if (src.genres?.Count > 0) genres = MergeArrays(genres, src.genres);
            if (src.casts?.Count > 0) casts = MergeArrays(casts, src.casts);
            if (src.directors?.Count > 0) directors = MergeArrays(directors, src.directors);
            if (!release_date.HasValue) release_date = src.release_date;

            release_notes = MergeStrings(release_notes, src.release_notes);
            release_dvd = MergeStrings(release_dvd, src.release_dvd);

            if (running_time < 0) running_time = src.running_time;

            official_site = MergeStrings(official_site, src.official_site);

            if (src.distributors?.Count > 0) distributors = MergeArrays(distributors, src.distributors);
            if (src.producers?.Count > 0) producers = MergeArrays(producers, src.producers);
            if (src.writers?.Count > 0) writers = MergeArrays(writers, src.writers);

            synopsis = MergeStrings(synopsis, src.synopsis);
            lang = MergeStrings(lang, src.lang);
            intl_country = MergeStrings(intl_country, src.intl_country);
            intl_name = MergeStrings(intl_name, src.intl_name);
            intl_cert = MergeStrings(intl_cert, src.intl_cert);
            intl_advisory = MergeStrings(intl_advisory, src.intl_advisory);
            intl_poster = MergeStrings(intl_poster, src.intl_poster);

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

        public bool IsSameAs(Theater other)
        {
            return
            theater_id == other.theater_id &&
            theater_name == other.theater_name &&
            theater_address == other.theater_address &&
            theater_city == other.theater_city &&
            theater_state == other.theater_state &&
            theater_zip == other.theater_zip &&
            theater_phone == other.theater_phone &&
            theater_attributes == other.theater_attributes &&
            theater_ticketing == other.theater_ticketing &&
            theater_closed_reason == other.theater_closed_reason &&
            theater_area == other.theater_area &&
            theater_location == other.theater_location &&
            theater_market == other.theater_market &&
            theater_screens == other.theater_screens &&
            theater_seating == other.theater_seating &&
            theater_adult == other.theater_adult &&
            theater_child == other.theater_child &&
            theater_senior == other.theater_senior &&
            theater_country == other.theater_country &&
            theater_url == other.theater_url &&
            theater_chain_id == other.theater_chain_id &&

            theater_adult_bargain == other.theater_adult_bargain &&
            theater_senior_bargain == other.theater_senior_bargain &&
            theater_child_bargain == other.theater_child_bargain &&
            theater_special_bargain == other.theater_special_bargain &&
            theater_adult_super == other.theater_adult_super &&
            theater_senior_super == other.theater_senior_super &&
            theater_child_super == other.theater_child_super &&
            theater_price_comment == other.theater_price_comment &&
            theater_extra == other.theater_extra &&
            theater_desc == other.theater_desc &&
            theater_type == other.theater_type &&
            theater_lon == other.theater_lon &&
            theater_lat == other.theater_lat;
        }
        public void Merge(Theater src)
        {
            theater_name = MergeStrings(theater_name, src.theater_name);
            theater_address = MergeStrings(theater_address, src.theater_address);
            theater_city = MergeStrings(theater_city, src.theater_city);
            theater_state = MergeStrings(theater_state, src.theater_state);
            theater_zip = MergeStrings(theater_zip, src.theater_zip);
            theater_phone = MergeStrings(theater_phone, src.theater_phone);
            theater_attributes = MergeStrings(theater_attributes, src.theater_attributes);
            theater_ticketing = MergeStrings(theater_ticketing, src.theater_ticketing);
            theater_closed_reason = MergeStrings(theater_closed_reason, src.theater_closed_reason);
            theater_area = MergeStrings(theater_area, src.theater_area);
            theater_location = MergeStrings(theater_location, src.theater_location);
            theater_market = MergeStrings(theater_market, src.theater_market);

            if (!theater_screens.HasValue) theater_screens = src.theater_screens;

            theater_seating = MergeStrings(theater_seating, src.theater_seating);
            theater_adult = MergeStrings(theater_adult, src.theater_adult);
            theater_child = MergeStrings(theater_child, src.theater_child);
            theater_senior = MergeStrings(theater_senior, src.theater_senior);
            theater_country = MergeStrings(theater_country, src.theater_country);
            theater_url = MergeStrings(theater_url, src.theater_url);
            theater_chain_id = MergeStrings(theater_chain_id, src.theater_chain_id);
            theater_adult_bargain = MergeStrings(theater_adult_bargain, src.theater_adult_bargain);
            theater_senior_bargain = MergeStrings(theater_senior_bargain, src.theater_senior_bargain);
            theater_child_bargain = MergeStrings(theater_child_bargain, src.theater_child_bargain);
            theater_special_bargain = MergeStrings(theater_special_bargain, src.theater_special_bargain);
            theater_adult_super = MergeStrings(theater_adult_super, src.theater_adult_super);
            theater_senior_super = MergeStrings(theater_senior_super, src.theater_senior_super);
            theater_child_super = MergeStrings(theater_child_super, src.theater_child_super);
            theater_price_comment = MergeStrings(theater_price_comment, src.theater_price_comment);
            theater_extra = MergeStrings(theater_extra, src.theater_extra);
            theater_desc = MergeStrings(theater_desc, src.theater_desc);
            theater_type = MergeStrings(theater_type, src.theater_type);

            if (!theater_lat.HasValue) theater_lat = src.theater_lat;
            if (!theater_lon.HasValue) theater_lon = src.theater_lon;
        }
    }

    /**
     * Showtime class
     */
    public class Showtime : XmlDoc
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

            if (!show_date.HasValue)
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
            if (showtimes == null || showtimes?.Length == 0)
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
            showtime_minutes = showTimeMunitesList.ToArray();

            show_attributes = GetElementValueStr(show_date_element, "show_attributes", ",");
            show_passes = GetElementValueStr(show_date_element, "show_passes");
            show_festival = GetElementValueStr(show_date_element, "show_festival");
            show_with = GetElementValueStr(show_date_element, "show_with");
            show_sound = GetElementValueStr(show_date_element, "show_sound");
            show_comments = GetElementValueStr(show_date_element, "show_comments", ";");

        }

    }

}
