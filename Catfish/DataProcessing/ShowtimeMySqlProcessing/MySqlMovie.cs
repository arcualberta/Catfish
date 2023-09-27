using Catfish.API.Repository.Solr;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
//using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    [Table("movie")]
    public class MySqlMovie : MySqlModel
    {
        #region Properties
        [Key]
        public int Movie_ID { get; set; }
        public string? Movie_Title { get; set; }
        public int? Parent_ID { get; set; } = null;
        public string? Genre { get; set; }
        public string? Rating { get; set; }
        public string? Director { get; set; }
        public string? Producer { get; set; }
        public string? Actor { get; set; }
        public string? Writer { get; set; }
        public string? Distributor { get; set; }
        public DateTime? Release_Date { get; set; } = null;
        public string? Release_Notes { get; set; }
        public int? Running_Time { get; set; } = null;
        public float? Star_Rating { get; set; } = null;
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
        
        ////protected static Regex _csvSplitRegx = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        ////public static MySqlMovie CreateInstance(string concatenatedFieldValues)
        ////{
        ////    /*
        ////     *    `Movie_ID` int(10) NOT NULL COMMENT 'The Unique ID for a movie',
        ////          `Movie_Title` varchar(100) DEFAULT NULL,
        ////          `Parent_ID` int(10) NOT NULL COMMENT 'The parent ID of the movie',
        ////          `Genre` varchar(25) DEFAULT NULL COMMENT 'The main (first) genre for the movie',
        ////          `Rating` varchar(25) DEFAULT NULL COMMENT 'MPAA Rating',
        ////          `Director` varchar(90) DEFAULT NULL COMMENT 'The director, comma separated',
        ////          `Producer` varchar(90) DEFAULT NULL COMMENT 'The Producer comma separated list',
        ////          `Actor` varchar(35) DEFAULT NULL COMMENT 'First Billed Actor',
        ////          `Writer` varchar(90) DEFAULT NULL COMMENT 'Writer, comma separated list',
        ////          `Distributor` varchar(40) DEFAULT NULL COMMENT 'Distribution Company',
        ////          `Release_Date` date DEFAULT NULL COMMENT 'Theatrical Release Date',
        ////          `Release_Notes` varchar(25) DEFAULT NULL COMMENT 'Release pattern notes',
        ////          `Running_Time` int(5) DEFAULT NULL COMMENT 'Running Time of Movie',
        ////          `Star_Rating` float DEFAULT NULL COMMENT 'Rating in half increments eg. 3.5 (Max is 4)',
        ////          `URL` varchar(80) DEFAULT NULL COMMENT 'The Official Website including the http prefix'
        ////     */

        ////    MySqlMovie movie = new MySqlMovie();

        ////    var doubleQuoteSeparatedStr = concatenatedFieldValues
        ////        .Replace("','", "\",\"")//separation between two string values
        ////        .Replace("',", "\",")   //separation bewteen a string and numberor NULL
        ////        .Replace(",'", ",\"")   //separation between a number or NULL and a string
        ////        .Replace("\\'", "'");    //replace escaped singel quotes with just single quotes 
                
        ////    if(doubleQuoteSeparatedStr.StartsWith("'"))
        ////        doubleQuoteSeparatedStr = "\"" + doubleQuoteSeparatedStr.Substring(1);

        ////    if (doubleQuoteSeparatedStr.EndsWith("'"))
        ////        doubleQuoteSeparatedStr = doubleQuoteSeparatedStr.Substring(0, doubleQuoteSeparatedStr.Length-1) + "\"";

        ////    var parts = _csvSplitRegx.Split(doubleQuoteSeparatedStr)
        ////        //.Select(s => s.Trim('\"')) // removing leading or trailing double quotes
        ////        .ToArray();
        ////    int idx = 0;

        ////    //Movie ID
        ////    string movieIdStr = parts[idx++];
        ////    try { movie.Movie_ID = int.Parse(movieIdStr); }
        ////    catch (Exception ex) { throw ex; }


        ////    //Movie title
        ////    movie.Movie_Title = parts[idx++];

        ////    //Parent ID
        ////    string parentIdStr = parts[idx++];
        ////    if (parentIdStr?.Length > 0)
        ////    {
        ////        try { movie.Parent_ID = int.Parse(parentIdStr); }
        ////        catch (Exception ex)
        ////        {
        ////            throw ex;
        ////        }
        ////    }


        ////    //Genre
        ////    movie.Genre = parts[idx++];

        ////    //Rating
        ////    movie.Rating = parts[idx++];

        ////    //Director
        ////    movie.Director = parts[idx++];

        ////    //Producer
        ////    movie.Producer = parts[idx++];

        ////    //Actor
        ////    movie.Actor = parts[idx++];

        ////    //Writer
        ////    movie.Writer = parts[idx++];

        ////    //Distributor
        ////    movie.Distributor = parts[idx++];

        ////    //Release_Date
        ////    var releaseDateStr = parts[idx++];
        ////    if (releaseDateStr?.Length > 0 && !releaseDateStr.StartsWith("0000") && (releaseDateStr != "NULL"))
        ////    {
        ////        try { movie.Release_Date = DateTime.Parse(releaseDateStr); }
        ////        catch (Exception ex) { throw ex; }
        ////    }


        ////    //Release_Notes
        ////    movie.Release_Notes = parts[idx++];

        ////    //Star rating
        ////    string starRatingStr = parts[idx++];
        ////    if (starRatingStr?.Length > 0 && starRatingStr != "NULL")
        ////    {
        ////        try { movie.Star_Rating = float.Parse(starRatingStr); }
        ////        catch (Exception ex) { throw ex; }
        ////    }

        ////    //URL
        ////    movie.URL = parts[idx++];

        ////    return movie;
        ////}

        ////public static MySqlMovie CreateInstance2(string concatenatedFieldValues)
        ////{
        ////    /*
        ////     *    `Movie_ID` int(10) NOT NULL COMMENT 'The Unique ID for a movie',
        ////          `Movie_Title` varchar(100) DEFAULT NULL,
        ////          `Parent_ID` int(10) NOT NULL COMMENT 'The parent ID of the movie',
        ////          `Genre` varchar(25) DEFAULT NULL COMMENT 'The main (first) genre for the movie',
        ////          `Rating` varchar(25) DEFAULT NULL COMMENT 'MPAA Rating',
        ////          `Director` varchar(90) DEFAULT NULL COMMENT 'The director, comma separated',
        ////          `Producer` varchar(90) DEFAULT NULL COMMENT 'The Producer comma separated list',
        ////          `Actor` varchar(35) DEFAULT NULL COMMENT 'First Billed Actor',
        ////          `Writer` varchar(90) DEFAULT NULL COMMENT 'Writer, comma separated list',
        ////          `Distributor` varchar(40) DEFAULT NULL COMMENT 'Distribution Company',
        ////          `Release_Date` date DEFAULT NULL COMMENT 'Theatrical Release Date',
        ////          `Release_Notes` varchar(25) DEFAULT NULL COMMENT 'Release pattern notes',
        ////          `Running_Time` int(5) DEFAULT NULL COMMENT 'Running Time of Movie',
        ////          `Star_Rating` float DEFAULT NULL COMMENT 'Rating in half increments eg. 3.5 (Max is 4)',
        ////          `URL` varchar(80) DEFAULT NULL COMMENT 'The Official Website including the http prefix'
        ////     */

        ////    MySqlMovie movie = new MySqlMovie();


        ////    int offset = 0;

        ////    //Movie ID
        ////    string movieIdStr = getFieldValue(concatenatedFieldValues, offset, NUM2STR, out offset);
        ////    try { movie.Movie_ID = int.Parse(movieIdStr); }
        ////    catch(Exception ex) { throw ex; }
            

        ////    //Movie title
        ////    movie.Movie_Title = getFieldValue(concatenatedFieldValues, offset, STR2NUM, out offset);

        ////    //Parent ID
        ////    string parentIdStr = getFieldValue(concatenatedFieldValues, offset, NUM2STR, out offset);
        ////    if (parentIdStr?.Length > 0)
        ////    {
        ////        try { movie.Parent_ID = int.Parse(parentIdStr); }
        ////        catch (Exception ex) 
        ////        {
        ////            //Regex csvSplitRegx = new Regex("," + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        ////            Regex csvSplitRegx = new Regex("," + "(?=(?:[^']*'[^']*')*(?![^']*'))");
        ////            var parts = csvSplitRegx.Split(concatenatedFieldValues);
        ////            throw ex; 
        ////        }
        ////    }
            

        ////    //Genre
        ////    movie.Genre = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Rating
        ////    movie.Rating = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Director
        ////    movie.Director = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Producer
        ////    movie.Producer = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Actor
        ////    movie.Actor = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Writer
        ////    movie.Writer = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Distributor
        ////    movie.Distributor = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

        ////    //Release_Date
        ////    var releaseDateStr = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);
        ////    if (releaseDateStr?.Length > 0 && !releaseDateStr.StartsWith("0000"))
        ////    {
        ////        try { movie.Release_Date = DateTime.Parse(releaseDateStr); }
        ////        catch (Exception ex) { throw ex; }
        ////    }
            

        ////    //Release_Notes
        ////    movie.Release_Notes = getFieldValue(concatenatedFieldValues, offset, STR2NUM, out offset);

        ////    //Star rating
        ////    string starRatingStr = getFieldValue(concatenatedFieldValues, offset, NUM2NUM, out offset);
        ////    if (starRatingStr?.Length > 0)
        ////    {
        ////        try { movie.Star_Rating = float.Parse(starRatingStr); }
        ////        catch (Exception ex) { throw ex; }
        ////    }

        ////    //URL
        ////    movie.URL = getFieldValue(concatenatedFieldValues, offset, null, out _);

        ////    return movie;
        ////}

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
