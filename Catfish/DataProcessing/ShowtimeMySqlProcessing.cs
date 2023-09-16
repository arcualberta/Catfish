using Catfish.API.Repository.Services;
using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class ShowtimeMySqlProcessing
    {
        private readonly TestHelper _testHelper = new TestHelper();

        private readonly string DATASET_NUMBER = "dataset_number_i";
        private readonly string MOVIE_ID = "movie_id_i";
        private readonly string MOVIE_TITLE = "title_t";
        private readonly string PARENT_ID = "parent_id_i";
        private readonly string GENRE_TS = "genres_ts";
        private readonly string DIRECTOR_TS = "directors_ts";
        private readonly string PRODUCER_TS = "producers_ts";
        private readonly string ACTOR_NEW_TS = "actors_ts";
        private readonly string WRITER_TS = "writers_ts";
        private readonly string DISTRIBUTOR_TS = "distributors_ts";
        private readonly string RELEASE_DATE = "release_date_dt";
        private readonly string RELEASE_NOTES = "release_notes_t";
        private readonly string RUNNING_TIME = "running_time_i";
        private readonly string STAR_RATING = "rating_t";
        private readonly string URL_NEW = "url_s";


        //CMD: C:\PATH\TO\Catfish\DataProcessing> dotnet test DataProcessing.csproj --filter DataProcessing.ShowtimeMySqlProcessing.IndexMovies
        [Fact]
        public async Task IndexMovies()
        {
            int batchSize = 1000;
            var db = _testHelper.MySqlMoviesDbContext;
            var solrService = _testHelper.Solr;

            var totalRecordCount = db.Movies.Count();

            int offset = 0;
            while(offset < totalRecordCount)
            {
                var solrDocs = new List<SolrDoc>();
                List<MySqlMovie> records = db.Movies.Skip(offset).Take(batchSize).ToList();

                foreach ( var rec in records)
                {
                    var doc = new SolrDoc();
                    solrDocs.Add(doc);

                    doc.AddId(Guid.NewGuid());
                    doc.AddField(DATASET_NUMBER, 1);
                    doc.AddField("entry_type_s", "movie");
                    doc.AddField(this.MOVIE_ID, rec.Movie_ID);
                    doc.AddField(this.MOVIE_TITLE, rec.Movie_Title);
                    doc.AddField(this.PARENT_ID, rec.Parent_ID);
                    doc.AddField(this.RELEASE_DATE, rec.Release_Date);
                    doc.AddField(this.RELEASE_NOTES, rec.Release_Notes); ;
                    doc.AddField(this.RUNNING_TIME, rec.Running_Time);
                    doc.AddField(this.STAR_RATING, rec.Rating);
                    doc.AddField(this.URL_NEW, rec.URL);

                    AddArrayField(doc, this.WRITER_TS, rec.Writer);
                    AddArrayField(doc, this.DISTRIBUTOR_TS, rec.Distributor);
                    AddArrayField(doc, this.DIRECTOR_TS, rec.Director);
                    AddArrayField(doc, this.GENRE_TS, rec.Genre);
                    AddArrayField(doc, this.PRODUCER_TS, rec.Producer);
                    AddArrayField(doc, this.ACTOR_NEW_TS, rec.Actor);
                }

                await solrService.Index(solrDocs);
                await solrService.CommitAsync();

                offset += records.Count;
            }

        }

        private void AddArrayField(SolrDoc doc, string fieldName, string? concatenatedFieldValue)
        {
            if (!string.IsNullOrEmpty(concatenatedFieldValue))
                foreach (var x in concatenatedFieldValue.Split(";", StringSplitOptions.TrimEntries).ToList())
                    doc.AddField(fieldName, x);
        }
    }
}
