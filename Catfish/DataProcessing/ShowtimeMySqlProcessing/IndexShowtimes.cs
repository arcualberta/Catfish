using Catfish.API.Repository.Services;
using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
using DataProcessing.ShowtimeMySqlProcessing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var showtime = MySqlShowtime.CreateInstance(concatenatedFieldValues);


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
            _countryOrigins = _testHelper.countryDbContext.Data.ToList();
            _distribuitons = _testHelper.distributionDbContext.Data.ToList();
            _movies = _testHelper.movieDbContext.Data.ToList();
            _movieCasts = _testHelper.movieCastDbContext.Data.ToList();
            _movieGenres = _testHelper.movieGenreDbContext.Data.ToList();
            _theaters = _testHelper.theaterDbContext.Data.ToList();


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

    }
}
