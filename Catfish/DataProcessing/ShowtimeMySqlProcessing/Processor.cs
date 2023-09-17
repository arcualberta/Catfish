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
using static DataProcessing.ShowtimeMySqlProcessing.Processor;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class Processor
    {
        private readonly TestHelper _testHelper = new TestHelper();

        public delegate void CreateMySqlModel(string concatenatedFieldValues);

        protected List<SolrDoc> _docList = new List<SolrDoc>();

        protected void ExtractSqlInserts(string sourceSqlDumpFileName, string modelName, CreateMySqlModel createInstance, string errorLogFile, string progressLogFile)
        {
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
                            end = line.IndexOf(')', offset + 1);

                            if (end > 0)
                            {
                                string valueSet = line.Substring(offset + 1, end - offset - 1);

                                try
                                {
                                    ++count;
                                    createInstance(valueSet);

                                    //MySqlShowtime showtime = MySqlShowtime.CreateInstance(valueSet);
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
        }

        /*
        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexMovies
        [Fact]
        public async Task IndexMovies()
        {
            int batchSize = 1000;
            var db = _testHelper.MySqlMoviesDbContext;
            var solrService = _testHelper.Solr;

            var totalRecordCount = db.Movies.Count();

            int offset = 0;
            while (offset < totalRecordCount)
            {
                var solrDocs = new List<SolrDoc>();
                List<MySqlMovie> records = db.Movies.Skip(offset).Take(batchSize).ToList();

                foreach (var rec in records)
                {
                    var doc = new SolrDoc();
                    solrDocs.Add(doc);


                }

                await solrService.Index(solrDocs);
                await solrService.CommitAsync();

                offset += records.Count;
            }

        }
        */

        private void CreateShwotime(string concatenatedFieldValues)
        {
            var showtime = MySqlShowtime.CreateInstance(concatenatedFieldValues);

        }

        [Fact]
        public async void IndexShowtimes()
        {
            string sourceSqlDumpFileName = "C:\\Projects\\Showtime Database\\07-showtime.sql";
            string errorLogFile = sourceSqlDumpFileName + "-ingestion-errors.txt";
            string progressLogFile = sourceSqlDumpFileName + "-progress.txt";

            ExtractSqlInserts(sourceSqlDumpFileName, "Showtime", CreateShwotime, errorLogFile, progressLogFile);

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
