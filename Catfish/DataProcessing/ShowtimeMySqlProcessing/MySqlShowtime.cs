using Catfish.API.Repository.Solr;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class MySqlShowtime : MySqlModel
    {
        #region Properties
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public DateTime ShowDate { get; set; } = DateTime.MinValue;
        public string[] ShowTimes { get; set; } = new string[0];
        public int[] ShowTimeMinutes { get; set; } = new int[0];
        public string? SpecialAttributes { get; set; }
        public string? Festival { get; set; }
        public int? WithMovieId { get; set; }
        public string? ShowSound { get; set; }
        public string? ShowPasses { get; set; }
        public string? ShowComments { get; set; }
        #endregion

        #region Solr Fields

        #endregion

        public static MySqlShowtime CreateInstance(string concatenatedFieldValues)
        {
            /*
             * Field values are passed on in the sequence to the following MySQL table-column definitions
             *    `Movie_ID` int(10) NOT NULL,
             *    `Venue_ID` int(10) NOT NULL,
             *    `Show_Date` date NOT NULL,
             *    `Show_Time` time NOT NULL,
             *    `Special_Attributes` varchar(50) DEFAULT NULL,
             *    `Festival` char(1) DEFAULT NULL,
             *    `With_Movie_ID` int(10) DEFAULT NULL,
             *    `Show_Sound` varchar(30) DEFAULT NULL,
             *    `Show_Passes` char(1) DEFAULT NULL,
             *    `Show_Comments` varchar(254) DEFAULT NULL
             */

            MySqlShowtime showtime = new MySqlShowtime();

            var doubleQuoteSeparatedStr = concatenatedFieldValues
                .Replace("\\\"", "").Replace("\"", "") //If the string has double quotes and escaped double quotes, we run into regex split issues, so we remove them here
                .Replace("','", "\",\"")//separation between two string values
                .Replace("',", "\",")   //separation bewteen a string and numberor NULL
                .Replace(",'", ",\"")   //separation between a number or NULL and a string
                .Replace("\\'", "'");    //replace escaped singel quotes with just single quotes 

            if (doubleQuoteSeparatedStr.StartsWith("'"))
                doubleQuoteSeparatedStr = "\"" + doubleQuoteSeparatedStr.Substring(1);

            if (doubleQuoteSeparatedStr.EndsWith("'"))
                doubleQuoteSeparatedStr = doubleQuoteSeparatedStr.Substring(0, doubleQuoteSeparatedStr.Length - 1) + "\"";

            var parts = _csvSplitRegx.Split(doubleQuoteSeparatedStr)
                .Select(s => s.Trim('\"')) // removing leading or trailing double quotes
                .ToArray();


            int idx = 0;
            //Movie ID
            string movieIdStr = parts[idx++];
            try { showtime.MovieId = int.Parse(movieIdStr); }
            catch(Exception ex) { throw ex; }
            

            //Theater Id
            string theaterIdStr = parts[idx++];
            try { showtime.TheaterId = int.Parse(theaterIdStr); }
            catch (Exception ex) { throw ex; }

            //Show Date
            string showDateStr = parts[idx++];
            if (!showDateStr.StartsWith("0000"))
            {
                try { showtime.ShowDate = DateTime.Parse(showDateStr); }
                catch (Exception ex) { throw ex; }
            }


            //Show Time
            //This will only be one time string according to the old schema.
            //However, we index this as a string array in Solr.
            //We also generate an int array for the show time in minutes.
            string showTimeStr = parts[idx++];
            if (showTimeStr?.Length > 0)
            {
                try
                {

                    showtime.ShowTimes = new string[] { showTimeStr };

                    string[] hms = showTimeStr.Split(':').Select(x => x.Trim('\'')).ToArray();
                    showtime.ShowTimeMinutes = new int[] { int.Parse(hms[0]) * 60 + int.Parse(hms[1]) };
                }
                catch(Exception ex){ throw ex; }
            }


            //Sepcial Attributes
            showtime.SpecialAttributes = parts[idx++];

            //Festival
            showtime.Festival = parts[idx++];

            //WithMoviedId, which is the next nullable integer
            string withMovideIdStr = parts[idx++];
            if (withMovideIdStr?.Length > 0 && withMovideIdStr != "NULL")
            {
                try { showtime.WithMovieId = int.Parse(withMovideIdStr); }
                catch (Exception ex) { throw ex; }
            }
            

            //ShowSound
            showtime.ShowSound = parts[idx++];

            //ShowPass
            showtime.ShowPasses = parts[idx++];

            showtime.ShowComments = parts[idx++];

            return showtime;
        }

        public static MySqlShowtime CreateInstance2(string concatenatedFieldValues)
        {
            /*
             * Field values are passed on in the sequence to the following MySQL table-column definitions
             *    `Movie_ID` int(10) NOT NULL,
             *    `Venue_ID` int(10) NOT NULL,
             *    `Show_Date` date NOT NULL,
             *    `Show_Time` time NOT NULL,
             *    `Special_Attributes` varchar(50) DEFAULT NULL,
             *    `Festival` char(1) DEFAULT NULL,
             *    `With_Movie_ID` int(10) DEFAULT NULL,
             *    `Show_Sound` varchar(30) DEFAULT NULL,
             *    `Show_Passes` char(1) DEFAULT NULL,
             *    `Show_Comments` varchar(254) DEFAULT NULL
             */

            MySqlShowtime showtime = new MySqlShowtime();

            int offset = 0;

            //Movie ID
            string movieIdStr = getFieldValue(concatenatedFieldValues, offset, NUM2NUM, out offset);
            try { showtime.MovieId = int.Parse(movieIdStr); }
            catch(Exception ex) { throw ex; }
            

            //Theater Id
            string theaterIdStr = getFieldValue(concatenatedFieldValues, offset, NUM2STR, out offset);
            try { showtime.TheaterId = int.Parse(theaterIdStr); }
            catch(Exception ex) { throw ex; }

            //Show Date
            string showDateStr = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);
            if (!showDateStr.StartsWith("0000"))
            {
                try { showtime.ShowDate = DateTime.Parse(showDateStr); }
                catch(Exception ex) { throw ex; }
            }
                

            //Show Time
            //This will only be one time string according to the old schema.
            //However, we index this as a string array in Solr.
            //We also generate an int array for the show time in minutes.
            //Next field is also a string
            string showTimeStr = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);
            if (showTimeStr?.Length > 0)
            {
                try
                {
                    showtime.ShowTimes = new string[] { showTimeStr };

                    string[] hms = showTimeStr.Split(':').Select(x => x.Trim('\'')).ToArray();
                    showtime.ShowTimeMinutes = new int[] { int.Parse(hms[0]) * 60 + int.Parse(hms[1]) };
                }
                catch(Exception ex) { throw ex; }
            }

            //Sepcial Attributes
            showtime.SpecialAttributes = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

            //Festival
            showtime.Festival = getFieldValue(concatenatedFieldValues, offset, STR2NUM, out offset);

            //WithMoviedId, which is the next nullable integer
            string withMovideIdStr = getFieldValue(concatenatedFieldValues, offset, NUM2STR, out offset);
            if (withMovideIdStr?.Length > 0 && withMovideIdStr != "NULL")
            {
                try { showtime.WithMovieId = int.Parse(withMovideIdStr); }
                catch(Exception ex) { throw ex; }
            }
                

            //ShowSound
            showtime.ShowSound = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

            //ShowPass
            showtime.ShowPasses = getFieldValue(concatenatedFieldValues, offset, STR2STR, out offset);

            showtime.ShowComments = getFieldValue(concatenatedFieldValues, offset, null, out offset);

            return showtime;
        }

    }
}
