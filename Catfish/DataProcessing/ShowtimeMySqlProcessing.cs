using Catfish.API.Repository.Services;
using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
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

                var records = db.Movies.Skip(offset).Take(batchSize).ToList();
                foreach ( var rec in records)
                {
                    var doc = new SolrDoc();
                    solrDocs.Add(doc);

                    doc.AddId(Guid.NewGuid());
                    doc.AddField(this.MOVIE_ID, rec.Movie_ID);
                    doc.AddField(this.MOVIE_TITLE, rec.Movie_Title);
                    doc.AddField(this.PARENT_ID, rec.Parent_ID);
                    doc.AddField(this.RELEASE_DATE, rec.Release_Date);
                    doc.AddField(this.RELEASE_NOTES, rec.Release_Notes); ;
                    doc.AddField(this.RUNNING_TIME, rec.Running_Time);
                    doc.AddField(this.STAR_RATING, rec.Rating);
                    doc.AddField(this.URL_NEW, rec.URL);

                    foreach (var x in rec.Writer?.Split(";", StringSplitOptions.TrimEntries).ToList())
                        doc.AddField(this.WRITER_TS, x);

                    foreach (var x in rec.Distributor?.Split(";", StringSplitOptions.TrimEntries).ToList())
                        doc.AddField(this.DISTRIBUTOR_TS, x);

                    foreach (var x in rec.Director?.Split(";", StringSplitOptions.TrimEntries).ToList())
                        doc.AddField(this.DIRECTOR_TS, x);

                    foreach (var x in rec.Genre?.Split(";", StringSplitOptions.TrimEntries).ToList())
                        doc.AddField(this.GENRE_TS, x);

                    foreach (var x in rec.Producer?.Split(";", StringSplitOptions.TrimEntries).ToList())
                        doc.AddField(this.PRODUCER_TS, x);

                    foreach (var x in rec.Actor?.Split(";", StringSplitOptions.TrimEntries).ToList())
                        doc.AddField(this.ACTOR_NEW_TS, x);
                }

                await solrService.Index(solrDocs);
                await solrService.CommitAsync();

                offset += records.Count;
            }

        }
    }
}
