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
    public class IndexMovies
    {
        private readonly TestHelper _testHelper = new TestHelper();

        public delegate void CreateMySqlModel(string concatenatedFieldValues);

        protected List<MySqlMovie> _movieList = new List<MySqlMovie>();
        protected List<MySqlTheater> _theaterList = new List<MySqlTheater>();
        protected List<SolrDoc> _docList = new List<SolrDoc>();

        protected void ProcessSqlInserts(string sourceSqlDumpFileName, string modelName, CreateMySqlModel createInstance)
        {
            string dataFolder = sourceSqlDumpFileName.Substring(0, sourceSqlDumpFileName.LastIndexOf("\\"));
            string tmp = sourceSqlDumpFileName.Substring(sourceSqlDumpFileName.LastIndexOf("\\") + 1);
            tmp = tmp.Substring(0, tmp.Length - 4);
            string errorLogFile = $"{dataFolder}\\zz_{tmp}-errors.txt";
            string progressLogFile = $"{dataFolder}\\zz_{tmp}-progress.txt";

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
                            string valueSet = end > 0 ? line.Substring(offset + 1, end - offset - 1) : line.Substring(offset + 1).TrimEnd(new char[] { ')', ';' });
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
        }


        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexMovies.Execute
        [Fact]
        public async Task Execute()
        {


            /*
            int batchSize = 1000;
            var db = _testHelper.MySqlMoviesDbContext;
            var solrService = _testHelper.Solr;

            var totalRecordCount = db.Movies.Count();

            int offset = 0;
            while (offset < totalRecordCount)
            {
                var solrDocs = new List<SolrDoc>();
                List<MySqlMovie> records = LoadMovies(db.Movies.Skip(offset).Take(batchSize).ToList();

                foreach (var rec in records)
                {
                    var doc = new SolrDoc();
                    solrDocs.Add(doc);


                }

                await solrService.Index(solrDocs);
                await solrService.CommitAsync();

                offset += records.Count;
            }
            */
        }

        private void ProcessShwotime(string concatenatedFieldValues)
        {
            var showtime = MySqlShowtime.CreateInstance(concatenatedFieldValues);

        }

    }
}
