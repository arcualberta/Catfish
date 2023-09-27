using Catfish.API.Repository.Solr;
using CliWrap;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class IndexShowtimes : IndexBase
    {

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
        protected int _solrDocBufferSize = 10000;
        protected string _indexedShowtimeIdTrackerFolder;
        protected List<string> _solrObjectTrackingKeys = new List<string>();

        protected bool _uploadToIndexingServer;
        protected string _uploadApi;
        protected string _basciApiSecurityCredentials;
        protected int _firstFileIndex;
        protected int _fileBatchSize;

        protected int _fileMoveFolderCount;
        protected bool _isDryRun;
        protected string _loadBalanceOutFile;
        protected string _balancerFolderRoot;
        protected int _loadBalanceFileMoveSleepMilliseconds;

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

        public async Task<bool> IsStopFlagSet()
        {
            string stopFlagFile = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:StopFlagFile").Value;
            return File.Exists(stopFlagFile) && (await File.ReadAllTextAsync(stopFlagFile)).StartsWith("1");
        }

        #region Main Batch Processing Delegator Method

        protected async Task ProcessFileBatch (
            string srcFolder,
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

            string[] srcFfiles = Directory.GetFiles(srcFolder).OrderBy(x => x).Skip(_firstFileIndex).Take(_fileBatchSize).ToArray();

            List<string> ingestedFiles = File.Exists(trackerFile) ? new List<string>(await File.ReadAllLinesAsync(trackerFile)) : new List<string>();

            var started = DateTime.Now;
            await File.AppendAllTextAsync(progressLogFile, $"Started at: {started}\n");
            int fileIndex = 0;

            foreach (string srcFile in srcFfiles)
            {
                if (await IsStopFlagSet())
                    break;

                if (ingestedFiles.Contains(srcFile))
                    continue;

                try
                {
                    var t1 = DateTime.Now;
                    if (await fileProcessingDelegate(srcFile, outputFolder, ++fileIndex, errorLogFile))
                    {
                        ingestedFiles.Add(srcFile);
                        await File.AppendAllTextAsync(trackerFile, $"{srcFile}\n");
                    }
                    var t2 = DateTime.Now;
                    
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
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;

            string trackerFile = "mysql-ingestion-tracker.txt";
            string errorLogFile = "mysql-ingestion-errors.txt";
            string progressLogFile = "mysql-ingestion-progress.txt";

            await ProcessFileBatch(
                srcFolder,
                logFolder,
                null, //Nothing to output into an output folder since the output goes to the MySql database.
                trackerFile,
                errorLogFile,
                progressLogFile,
                IngestFile);
        }

        protected async Task<bool> IngestFile(string sqlFile, string? _, int? __, string? ___)
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

            return true;
        }

        #endregion

        #region Extracting Insert Data from a Batch of Files to Text Data Files

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.ExtractInsertDataFromSplitMySqlFilesToTextFiles
        [Fact]
        public async Task ExtractInsertDataFromSplitMySqlFilesToTextFiles()
        {
            //Source folder for this method is the output folder of insert split files
            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:InsertFileFolder").Value;
            string outptFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:TextDataFolder").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;

            string trackerFile = "text-export-tracker.txt";
            string errorLogFile = "text-export-errors.txt";
            string progressLogFile = "text-export-progress.txt";

            await ProcessFileBatch(
                srcFolder,
                logFolder,
                outptFolder,
                trackerFile,
                errorLogFile,
                progressLogFile,
                ExportTextFromInserts);
        }

        protected async Task<bool> ExportTextFromInserts(string sqlFile, string? outputFolder, int? fileIndex, string? ___)
        {
            string outFile = $"showtime.txt_{string.Format("{0:d3}", fileIndex)}";
            outFile = Path.Combine(outputFolder!, outFile);

            return await ExportTextFromInserts(sqlFile, "Showtime", outFile);
        }

        #endregion


        #region Uploading Batch of Text Data to MySql Database

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.UploadTextDataFilesToMySqlDatabase
        [Fact]
        public async Task UploadTextDataFilesToMySqlDatabase()
        {
            //Source folder for this method is the output folder of insert split files
            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:TextDataFolder").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;

            string trackerFile = "mysql-text-data-ingestion-tracker.txt";
            string errorLogFile = "mysql-text-data-ingestion-errors.txt";
            string progressLogFile = "mysql-text-data-ingestion-progress.txt";

            await ProcessFileBatch(
                srcFolder,
                logFolder,
                null, //Nothing to output into an output folder since the output goes to the MySql database.
                trackerFile,
                errorLogFile,
                progressLogFile,
                IngestTextDataFile);
        }

        protected async Task<bool> IngestTextDataFile(string txtDataFile, string? _, int? __, string? ___)
        {
            string database = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:Database").Value;
            await IngestTextDataFile(txtDataFile, database);       
            return true;
        }
        #endregion


        #region Direct Solr Ingestion of Batch of Showtime Data from Text Data Files

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.IndexTextDataToSolr
        [Fact]
        public async Task IndexTextDataToSolr()
        {
            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:FirstFileIndex").Value, out _firstFileIndex))
                _firstFileIndex = 0;

            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:FileBatchSize").Value, out _fileBatchSize))
                _fileBatchSize = int.MaxValue;

            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:TextDataFolder").Value;
            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;
            logFolder = Path.Combine(logFolder, _firstFileIndex.ToString());

            _indexedShowtimeIdTrackerFolder = Path.Combine(logFolder, "solr-showtime-indexing-tracker-files");
            Directory.CreateDirectory(_indexedShowtimeIdTrackerFolder);

            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:SolrDocBufferSize").Value, out _solrDocBufferSize))
                _solrDocBufferSize = 10000;

            //Setting extended timeouts for all MySql connecitons
            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:MySqlServer:ConnectionTimeoutMinutes").Value, out int mySqlConnectionTimeOutMinutes))
                mySqlConnectionTimeOutMinutes = 5;
            _testHelper.SetMySqlConnectionTimeouts(mySqlConnectionTimeOutMinutes);

            //Setting the solr HTTP connection tymeout
            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:SolrHttpConnectionTimeoutMinutes").Value, out int myHttpConnectionTimeOutMinutes))
                myHttpConnectionTimeOutMinutes = 5;
            _testHelper.Solr.SetHttpClientTimeoutSeconds(myHttpConnectionTimeOutMinutes * 60);
            
            if(!bool.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:UploadToIndexingServer").Value, out _uploadToIndexingServer))
                _uploadToIndexingServer = false;
            if (_uploadToIndexingServer)
            {
                _uploadApi = _testHelper.Configuration.GetSection("SolarConfiguration:SolrDocUploadApi").Value;
                Assert.False(string.IsNullOrEmpty(_uploadApi));
            }
            _basciApiSecurityCredentials = _testHelper.Configuration.GetSection("SolarConfiguration:BasciApiSecurityCredentials").Value;


            string trackerFile = "solr-showtime-indexing-tracker.txt";
            string errorLogFile = "solr-showtime-indexing-errors.txt";
            string progressLogFile = "solr-showtime-indexing-progress.txt";

            //Pre-loading related data models that are needed by the IndexTextDataFileToSolr method from MySql database 
            _countryOrigins = _testHelper.countryDbContext.Data.ToList();
            _distribuitons = _testHelper.distributionDbContext.Data.ToList();
            _movies = _testHelper.movieDbContext.Data.ToList();
            _movieCasts = _testHelper.movieCastDbContext.Data.ToList();
            _movieGenres = _testHelper.movieGenreDbContext.Data.ToList();
            _theaters = _testHelper.theaterDbContext.Data.ToList();

            await ProcessFileBatch(
                srcFolder,
                logFolder,
                null, //Nothing to output into an output folder since the output goes to the MySql database.
                trackerFile,
                errorLogFile,
                progressLogFile,
                IndexTextDataFileToSolr);
        }

        protected async Task<bool> IndexTextDataFileToSolr(string txtDataFile, string? _, int? __, string? errorLogFile)
        {
            string indexedSolrObjectTrackerFile = Path.Combine(_indexedShowtimeIdTrackerFolder, txtDataFile.Substring(txtDataFile.LastIndexOf("\\") + 1) + ".txt");

            if (!File.Exists(indexedSolrObjectTrackerFile))
                File.Create(indexedSolrObjectTrackerFile).Close();
           
            bool isFileCompleted = false;

            try
            {
                int lineNumber = 0;

                string[] previouslyIndexedSolrObjectTrackingKeys = await File.ReadAllLinesAsync(indexedSolrObjectTrackerFile);

                using (StreamReader sr = File.OpenText(txtDataFile))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++lineNumber;

                        if (line.Length == 0)
                            continue;

                        //Check whether stop has been request after processing every X number of lines 
                        if (lineNumber % 5000 == 0 && await IsStopFlagSet())
                            break;

                        try
                        {
                            MySqlShowtime showtime = MySqlShowtime.CreateInstance(line);

                            string showtimeTrackingKey = GetShowtimeKey(showtime);
                            if (previouslyIndexedSolrObjectTrackingKeys.Contains(showtimeTrackingKey))
                                continue;

                            MySqlMovie movie = _movies.FirstOrDefault(x => x.Movie_ID == showtime.MovieId);
                            if (movie == null)
                                throw new Exception($"Movie {showtime.MovieId} not found");

                            MySqlTheater theater = _theaters.First(x => x.Venue_ID == showtime.TheaterId);
                            if(theater == null)
                                throw new Exception($"Theater {showtime.TheaterId} not found");

                            //Updating the solr doc with info of the showtime and related models
                            SolrDoc doc = CreateSolrDoc(showtime,
                                movie!,
                                theater!,
                                _distribuitons.FirstOrDefault(x => x.Movie_ID == showtime.MovieId),
                                _movieCasts.Where(x => x.Movie_ID == showtime.MovieId),
                                _movieGenres.Where(x => x.Movie_ID == showtime.MovieId),
                                _countryOrigins.FirstOrDefault(x => x.Movie_ID == showtime.MovieId));

                            //If everything is successful, add the solr doc to the list of solr docs.
                            _solrDocs.Add(doc);
                            _solrObjectTrackingKeys.Add(showtimeTrackingKey);
                        }
                        catch(Exception ex)
                        {
                            File.AppendAllText(errorLogFile!, $"{ex.Message}\nFile: {txtDataFile}\nLine: {lineNumber}\n_DATA_: {line}\n\n");
                        }

                        //If a sufficient number of the solr docs is collected in the list, index them and clear the list.
                        if (_solrDocs.Count >= _solrDocBufferSize)
                            await PushToSolrIndex(indexedSolrObjectTrackerFile);
                    }

                    isFileCompleted = line == null;
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                //Push any solr docs successfully created.
                await PushToSolrIndex(indexedSolrObjectTrackerFile);
                throw ex;
            }

            //Push any solr docs still not indexed.
            await PushToSolrIndex(indexedSolrObjectTrackerFile);

            return isFileCompleted;
        }

        protected SolrDoc CreateSolrDoc(
            MySqlShowtime showtime,
            MySqlMovie movie,
            MySqlTheater theater,
            MySqlDistribution? distribution,
            IEnumerable<MySqlMovieCast> casts,
            IEnumerable<MySqlMovieGenre> genre,
            MySqlCountryOrigin? origin)
        {
            SolrDoc doc = new SolrDoc();

            doc.AddId(showtime.Id);
            doc.AddField("entry_type_s", "showtime");
            doc.AddField("data_src_s", "kinomatics");
            doc.AddField("showtime_key_t", $"{movie.Movie_ID}-{theater.Venue_ID}-{showtime.ShowDate.Year}-{showtime.ShowDate.Month}-{showtime.ShowDate.Day}");
            doc.AddField("movie_id_i", movie.Movie_ID);
            doc.AddField("movie_name_t", movie.Movie_Title);
            doc.AddField("parent_id_i",movie.Parent_ID);
            doc.AddField("rating_t", movie.Rating);
            doc.AddField("directors_ts", GetStringArray(movie.Director));
            doc.AddField("producers_ts", GetStringArray(movie.Producer));
            doc.AddField("actors_ts", GetStringArray(movie.Actor));         //NEW: VERIFY
            doc.AddField("writers_ts", GetStringArray(movie.Writer));
            doc.AddField("distributors_ts", GetStringArray(movie.Distributor));
            doc.AddField("release_date_dt", movie.Release_Date);
            doc.AddField("release_notes_t", movie.Release_Notes);
            doc.AddField("running_time_i", movie.Running_Time);


            doc.AddField("title_t", movie.Movie_Title);
            doc.AddField("official_site_s", movie.URL);
            doc.AddField("star_rating_d", movie.Star_Rating.HasValue ? (decimal)movie.Star_Rating.Value : null);

            doc.AddField("casts_ts",        casts.Select(x => x.Cast_Name == null ? "" : x.Cast_Name).Distinct().ToArray());
            doc.AddField("cast_types_ts",   casts.Select(x => x.Cast_Type == null ? "" : x.Cast_Type).Distinct().ToArray());

            //set the union of move.genre and genre.Movie_Genre array as the genres_ts field's value
            string[] genre_vals = string.IsNullOrEmpty(movie.Genre)
                ? genre.Where(x => !string.IsNullOrEmpty(x.Movie_Genre)).Select(x => x.Movie_Genre!).Distinct().ToArray()
                : genre.Where(x => !string.IsNullOrEmpty(x.Movie_Genre)).Select(x => x.Movie_Genre!).Union(new string[] { movie.Genre }).Distinct().ToArray();
            doc.AddField("genres_ts", genre_vals);

            doc.AddField("show_date_dt", showtime.ShowDate);
            doc.AddField("showtimes_ts", showtime.ShowTimes);
            doc.AddField("showtime_minutes_is", showtime.ShowTimeMinutes);
            doc.AddField("show_attributes_ts", GetStringArray(showtime.SpecialAttributes));
            doc.AddField("show_festival_t", showtime.Festival);
            doc.AddField("show_with_t", _movies.FirstOrDefault(x => x.Movie_ID == showtime.WithMovieId)?.Movie_Title);
            doc.AddField("show_sound_t", showtime.ShowSound);
            doc.AddField("show_passes_t", showtime.ShowPasses);
            doc.AddField("show_comments_ts", GetStringArray(showtime.ShowComments));
            doc.AddField("theater_id_i", showtime.TheaterId);
            doc.AddField("theater_name_t", theater.Venue_Name);
            doc.AddField("theater_country_s", theater.Country);
            doc.AddField("theater_address_t", theater.Address);
            doc.AddField("theater_city_t", theater.City);
            doc.AddField("theater_county_t", theater.Country);
            doc.AddField("theater_state_t", theater.State);
            doc.AddField("theater_zip_t", theater.Postcode);
            doc.AddField("theater_lat_d", GetDecimal(theater.Lattitude));
            doc.AddField("theater_lon_d", GetDecimal(theater.Longitude));
            doc.AddField("theater_type_t", theater.Venue_Type);
            doc.AddField("theater_screens_i", theater.Screens);
            doc.AddField("theater_closed_reason_t", theater.Closed);
            doc.AddField("theater_attributes_t", theater.Attributes);
            doc.AddField("theater_seating_t", theater.Seating);
            doc.AddField("theater_market_t", theater.DMA_Market);
            doc.AddField("theater_adult_t", theater.Ticket_Adult);
            doc.AddField("theater_child_t", theater.Ticket_Child);
            doc.AddField("theater_senior_t", theater.Ticket_Senior);
            doc.AddField("theater_online_t", theater.Ticket_Online);
            doc.AddField("theater_bargain_t", theater.Ticket_Bargain);
            doc.AddField("theater_comments_t", theater.Ticket_Comment);

            doc.AddField("intl_release_dt", distribution?.Intl_Release_Date);
            doc.AddField("intl_name_t", distribution?.Intl_Title);
            doc.AddField("intl_country_s", distribution?.Intl_Country);
            doc.AddField("intl_rating_t", distribution?.Intl_Rating);

            doc.AddField("origin_movie_title_t", origin?.Movie_Title);
            doc.AddField("origin_imdb_id_i", origin?.IMDb_ID);
            doc.AddField("origin_imdb_title_t", origin?.IMDb_Title);
            doc.AddField("origin_movie_origin_t", origin?.Movie_Origin);
            doc.AddField("origin_parent_id_i", origin?.Parent_ID);
            doc.AddField("origin_year_t", origin?.Year);

            return doc;
        }
        protected async Task PushToSolrIndex(string indexedSolrObjectTrackerFile)
        {
            if (_solrDocs.Count >= 0)
            {
                if (_uploadToIndexingServer)
                {
                    await _testHelper.Solr.UploadIndexingSolrDocs(_solrDocs, _basciApiSecurityCredentials);
                }
                else
                {
                    await _testHelper.Solr.Index(_solrDocs);
                    await _testHelper.Solr.CommitAsync();
                }

                await File.AppendAllLinesAsync(indexedSolrObjectTrackerFile, _solrObjectTrackingKeys);

                _solrDocs.Clear();
                _solrObjectTrackingKeys.Clear();

                GC.Collect();
            }
        }

        protected string GetShowtimeKey(MySqlShowtime showtime) =>
            $"{showtime.MovieId}-{showtime.TheaterId}-{showtime.ShowDate.Year}{string.Format("{0:d2}", showtime.ShowDate.Month)}{string.Format("{0:d2}", showtime.ShowDate.Day)}-{string.Join("_", showtime.ShowTimeMinutes.Select(x => x.ToString()))}";

        public string[]? GetStringArray(string? concatenatedValues) =>
            concatenatedValues?
            .Split(";", StringSplitOptions.TrimEntries)
            .Where(x => x?.Length > 0)
            .ToArray();

        public int[]? GetIntArray(string? concatenatedValues) =>
            GetStringArray(concatenatedValues)?.Select(x => int.Parse(x)).ToArray();

        public decimal[]? GetDecimalArray(string? concatenatedValues) =>
            GetStringArray(concatenatedValues)?.Select(x => decimal.Parse(x)).ToArray();

        public DateTime[]? GetDateTimeArray(string? concatenatedValues) =>
            GetStringArray(concatenatedValues)?.Select(x => DateTime.Parse(x)).ToArray();

        public decimal? GetDecimal(string? value) => 
            string.IsNullOrEmpty(value) ? null : decimal.Parse(value);

        public decimal? GetDecimal(float? value) =>
            value.HasValue ? (decimal) value : null;

        #endregion


        #region Ingesting Solr XML Documents to Solr
        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.IngestSolrXmlToSolr
        [Fact]
        public async Task IngestSolrXmlToSolr()
        {
            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:FirstFileIndex").Value, out _firstFileIndex))
                _firstFileIndex = 0;

            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:FileBatchSize").Value, out _fileBatchSize))
                _fileBatchSize = int.MaxValue;

            //Setting the solr HTTP connection tymeout
            if (!int.TryParse(_testHelper.Configuration.GetSection("SolarConfiguration:SolrHttpConnectionTimeoutMinutes").Value, out int myHttpConnectionTimeOutMinutes))
                myHttpConnectionTimeOutMinutes = 5;
            _testHelper.Solr.SetHttpClientTimeoutSeconds(myHttpConnectionTimeOutMinutes * 60);

            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:SolrXmlFolder").Value;
            string outputFolder = Path.Combine(srcFolder, "completed");
            Directory.CreateDirectory(outputFolder);

            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;
            logFolder = Path.Combine(logFolder, $"solr-docs-{_firstFileIndex}");

            string trackerFile = "xml-solr-upload-tracker.txt";
            string errorLogFile = "xml-solr-upload-errors.txt";
            string progressLogFile = "xml-solr-upload-progress.txt";

            string progressFileFullPathName = Path.Combine(logFolder, progressLogFile);
            //Repeat the process until the stop flag is set. After processing each bactch, wait for 5 minutes before repeating
            int sleepTimeMinutes = 5;
            while (!(await IsStopFlagSet()))
            {
                DateTime t1 = DateTime.Now;

                await ProcessFileBatch(
                    srcFolder,
                    logFolder,
                    outputFolder,
                    trackerFile,
                    errorLogFile,
                    progressLogFile,
                    UploadXmlFileToSolr);

                DateTime t2 = DateTime.Now;
                if((t2-t1).Seconds < 1)
                {
                    await File.AppendAllTextAsync(progressFileFullPathName, $"No more files to process now. Sleeping for {sleepTimeMinutes} minutes. Set the stop-flag to 1 to terminate when wake up.");
                    await Task.Delay(sleepTimeMinutes * 60000);
                }
            }
        }

        protected async Task<bool> UploadXmlFileToSolr(string xmlFile, string? outputFolder, int? __, string? errorLogFile)
        {
            try
            {
                string xmlPayload = await File.ReadAllTextAsync(xmlFile);
                await _testHelper.Solr.AddUpdateAsync(xmlPayload);

                try
                {
                    string outFile = Path.Combine(outputFolder!, xmlFile.Substring(xmlFile.LastIndexOf("\\") + 1));
                    File.Move(xmlFile, outFile);
                }
                catch(Exception ex) 
                {
                    File.AppendAllText(errorLogFile!, $"{ex.Message}\nFile: {xmlFile}\n\n");
                }

                return true;
            }
            catch(Exception ex)
            {
                File.AppendAllText(errorLogFile!, $"{ex.Message}\nFile: {xmlFile}\n\n");
                return false;
            }
        }

        #endregion


        #region XML File Upload Load Balancing

        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexShowtimes.BalanceXmlFileUploadWorkload
        [Fact]
        public async Task BalanceXmlFileUploadWorkload()
        {
            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:FirstFileIndex").Value, out _firstFileIndex))
                _firstFileIndex = 0;

            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:FileBatchSize").Value, out _fileBatchSize))
                _fileBatchSize = int.MaxValue;

            string srcFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:SolrXmlFolder").Value;

            string logFolder = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LogFolder").Value;
            logFolder = Path.Combine(logFolder, $"solr-docs-{_firstFileIndex}");

            if (!bool.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:IsDryRun").Value, out _isDryRun))
                _isDryRun = true;

            if (!int.TryParse(_testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LoadBalanceFileMoveSleepMilliseconds").Value, out _loadBalanceFileMoveSleepMilliseconds))
                _loadBalanceFileMoveSleepMilliseconds = 0;

            string parentFolder = srcFolder.TrimEnd('\\');
            parentFolder = parentFolder.Substring(0, parentFolder.LastIndexOf('\\'));
            _fileMoveFolderCount = Directory.GetDirectories(parentFolder).Length;

            _loadBalanceOutFile = _testHelper.Configuration.GetSection("OldShowtimeDataIngestion:LoadBalanceOutFile").Value;

            string trackerFile = "xml-file-upload-balance-tracker.txt";
            string errorLogFile = "xml-file-upload-balance-errors.txt";
            string progressLogFile = "xml-file-upload-balance-progress.txt";

            string progressFileFullPathName = Path.Combine(logFolder, progressLogFile);
            //Repeat the process until the stop flag is set. After processing each bactch, wait for 5 minutes before repeating
            int sleepTimeMinutes = 15;
            while (!(await IsStopFlagSet()))
            {
                DateTime t1 = DateTime.Now;

                await ProcessFileBatch(
                    srcFolder,
                    logFolder,
                    null,
                    trackerFile,
                    errorLogFile,
                    progressLogFile,
                    MoveFile);

                DateTime t2 = DateTime.Now;
                if ((t2 - t1).Seconds < 1)
                {
                    await File.AppendAllTextAsync(progressFileFullPathName, $"No more files to process now. Sleeping for {sleepTimeMinutes} minutes. Set the stop-flag to 1 to terminate when wake up.");              
                    await Task.Delay(sleepTimeMinutes * 60000);
                }
            }
        }

        protected async Task<bool> MoveFile(string xmlFile, string? _, int? fileIndex, string? errorLogFile)
        {
            try
            {
                string dstFolder = xmlFile.Substring(0, xmlFile.LastIndexOf("\\")).TrimEnd('\'');
                dstFolder = xmlFile.Substring(0, dstFolder.LastIndexOf("\\")).TrimEnd('\'');

                int dstFolderIndex = (fileIndex!.Value % (_fileMoveFolderCount - 1)) + 1;
                dstFolder = Path.Combine(dstFolder, $"{dstFolderIndex}");

                try
                {
                    string outFile = Path.Combine(dstFolder!, xmlFile.Substring(xmlFile.LastIndexOf("\\") + 1));

                    if (!string.IsNullOrEmpty(_loadBalanceOutFile))
                        await File.AppendAllTextAsync(_loadBalanceOutFile, $"{outFile}\n");

                    if (!_isDryRun)
                    {
                        File.Move(xmlFile, outFile);

                        if (_loadBalanceFileMoveSleepMilliseconds > 0)
                            await Task.Delay(_loadBalanceFileMoveSleepMilliseconds); //We don't want the file copying to make th server super busy.
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(errorLogFile!, $"{ex.Message}\nFile: {xmlFile}\n\n");
                }

                return true;
            }
            catch (Exception ex)
            {
                File.AppendAllText(errorLogFile!, $"{ex.Message}\nFile: {xmlFile}\n\n");
                return false;
            }
        }

        #endregion

    }
}
