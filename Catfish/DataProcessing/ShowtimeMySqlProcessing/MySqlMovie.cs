using Catfish.API.Repository.Solr;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class MySqlMovie : MySqlModel
    {
        #region Properties
        public int Movie_ID { get; set; }
        public string? Movie_Title { get; set; }
        public int? Parent_ID { get; set; }
        public string? Genre { get; set; }
        public string? Rating { get; set; }
        public string? Director { get; set; }
        public string? Producer { get; set; }
        public string? Actor { get; set; }
        public string? Writer { get; set; }
        public string? Distributor { get; set; }
        public DateTime? Release_Date { get; set; }
        public string? Release_Notes { get; set; }
        public int? Running_Time { get; set; }
        public float? Star_Rating { get; set; }
        public string? URL { get; set; }
        #endregion

        #region Solr Fields
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
        #endregion


        public static MySqlMovie CreateInstance(string concatenatedFieldValues)
        {
            MySqlMovie movie = new MySqlMovie();

            return movie;
        }

        public void UpdateSolrFields(SolrDoc doc)
        {
            doc.AddId(Guid.NewGuid());
            doc.AddField(DATASET_NUMBER, 1);
            doc.AddField("entry_type_s", "movie");
            doc.AddField(MOVIE_ID, Movie_ID);
            doc.AddField(MOVIE_TITLE, Movie_Title);
            doc.AddField(PARENT_ID, Parent_ID);
            doc.AddField(RELEASE_DATE, Release_Date);
            doc.AddField(RELEASE_NOTES, Release_Notes); ;
            doc.AddField(RUNNING_TIME, Running_Time);
            doc.AddField(STAR_RATING, Rating);
            doc.AddField(URL_NEW, URL);

            AddArrayField(doc, WRITER_TS, Writer);
            AddArrayField(doc, DISTRIBUTOR_TS, Distributor);
            AddArrayField(doc, DIRECTOR_TS, Director);
            AddArrayField(doc, GENRE_TS, Genre);
            AddArrayField(doc, PRODUCER_TS, Producer);
            AddArrayField(doc, ACTOR_NEW_TS, Actor);
        }

    }
}
