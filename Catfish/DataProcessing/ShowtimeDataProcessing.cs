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

namespace DataProcessing
{
    [Collection("Database collection")]
    public class ShowtimeDataProcessing : IClassFixture<TransactionalTestDatabaseFixture>
    {
        public TransactionalTestDatabaseFixture _fixture { get; }

        public readonly TestHelper _testHelper;
        public int MAX_RECORDS = 1; //DEBUG ONLY -- set it to 0 or -1 to ignore it

        public ShowtimeDataProcessing(TransactionalTestDatabaseFixture fixture)
        {
            _fixture = fixture;
            _testHelper = new TestHelper();
        }

        
        [Fact/*(Skip = "Don't want to re-create the db records now")*/]
        public void CreateDbRecords()
        {
            DateTime start = DateTime.Now;

            bool skipShowtimeRecords = false;
            bool skipMovieRecords = false;
            bool skipTheaterRecords = false;
            int maxShowtimeBatchesToProcess = 1;// int.MaxValue;
            
            var context = _testHelper.ShowtimeDb;

            string srcFolderRoot = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));
            string outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"processing-log-{fileSuffix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"error-log-{fileSuffix}.txt");

            var srcBatcheFolders = Directory.GetDirectories(srcFolderRoot);

            int batch = 0;
            int movieLastProcessedBatch = context.MovieRecords.Any() ? context.MovieRecords.Select(m => m.batch).Max() : 0;
            int theaterLastProcessedBatch = context.TheaterRecords.Any() ? context.TheaterRecords.Select(m => m.batch).Max() : 0;
            int showtimeLastProcessedBatch = context.ShowtimeRecords.Any() ? context.ShowtimeRecords.Select(m => m.batch).Max() : 0;
            int lastProcessedBatch = Math.Max(Math.Max(movieLastProcessedBatch, theaterLastProcessedBatch), showtimeLastProcessedBatch);

            //  if (lastProcessedBatch == 12)
            //      lastProcessedBatch = 13;

            //deleteing the last batch processed since it might have been interrupted half-way through
            if (lastProcessedBatch > 0)
            {
                context.MovieRecords.RemoveRange(context.MovieRecords.Where(rec => rec.batch == lastProcessedBatch));
                context.TheaterRecords.RemoveRange(context.TheaterRecords.Where(rec => rec.batch == lastProcessedBatch));
                context.ShowtimeRecords.RemoveRange(context.ShowtimeRecords.Where(rec => rec.batch == lastProcessedBatch));

                context.SaveChanges();
            }

            foreach (var batchFolder in srcBatcheFolders)
            {
                ++batch;

                if (lastProcessedBatch > 0 && batch < lastProcessedBatch)
                    continue;

                var batchName = batchFolder.Substring(batchFolder.LastIndexOf("\\") + 1);
                var zipFiles = Directory.GetFiles(batchFolder);
                foreach (var zipFile in zipFiles)
                {

                    File.AppendAllText(processingLogFile, $"Archive {zipFile}{Environment.NewLine}");
                    int showtimeCount = 0, theaterCount = 0, movieCount = 0;

                    using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            try
                            {
                               if ((skipShowtimeRecords || maxShowtimeBatchesToProcess < batch) && entry.Name.EndsWith("S.XML")) 
                                    continue;

                                if (skipMovieRecords && entry.Name.EndsWith("I.XML"))
                                    continue;

                                if (skipTheaterRecords && entry.Name.EndsWith("T.XML"))
                                    continue;

                                var extractFile = Path.Combine(outputFolder, entry.Name);
                                if (File.Exists(extractFile))
                                    File.Delete(extractFile);

                                entry.ExtractToFile(extractFile);

                                XElement xml = XElement.Load(extractFile);
                                if (xml.Name == "times")
                                {
                                    //This is a showtime data set
                                    foreach (var child in xml.Elements("showtime"))
                                    {
                                        Showtime showtime = new Showtime(child);
                                        context.ShowtimeRecords.Add(new ShowtimeRecord() { batch = batch,  movie_id = showtime.movie_id, theater_id = showtime.theater_id, show_date = showtime.show_date, content = JsonSerializer.Serialize(showtime) });
                                        ++showtimeCount;
                                        //context.SaveChanges();
                                    }
                                }
                                else if (xml.Name == "movies")
                                {
                                    //This is a movies data set. 
                                    foreach (var child in xml.Elements("movie"))
                                    {
                                        Movie movie = new Movie(child);
                                        context.MovieRecords.Add(new MovieRecord() { batch = batch, movie_id = movie.movie_id, content = JsonSerializer.Serialize(movie) });
                                        ++movieCount;
                                        //context.SaveChanges();
                                    }
                                }
                                else if (xml.Name == "houses")
                                {
                                    //This is a theatres data set
                                    foreach (var child in xml.Elements("theater"))
                                    {
                                        Theater theater = new Theater(child);
                                        context.TheaterRecords.Add(new TheaterRecord() { batch = batch, theater_id = theater.theater_id, content = JsonSerializer.Serialize(theater) });
                                        ++theaterCount;                                   
                                        //context.SaveChanges();
                                    }
                                }

                                File.AppendAllText(processingLogFile, $"    Movies: {movieCount}, Theaters: {theaterCount}, Showtimes: {showtimeCount}{Environment.NewLine}{Environment.NewLine}");

                                context.SaveChanges();
                                File.Delete(extractFile);
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText(errorLogFile, $"EXCEPTION in {entry.Name}: {ex.Message}{Environment.NewLine}");
                            }

                        } //End: foreach (ZipArchiveEntry entry in archive.Entries)

                    } //End:  using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                }
            }
        }
        [Fact]
        public void IndexData()
        {
            DateTime start = DateTime.Now;

            int batchSize = 500;
            int maxBatchesToProcess = int.MaxValue;

            var context = _testHelper.ShowtimeDb;

           string outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"indexing-log-{fileSuffix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"indexing-error-log-{fileSuffix}.txt");

            List<SolrDoc> solrDocs = new List<SolrDoc>();
            int offset = 0;
            int currentBatch = 0;
            while (true)
            {
                var ShowtimeRecords = context!.ShowtimeRecords.Skip(offset).Take(batchSize).ToList();

                if (!ShowtimeRecords.Any() || currentBatch >= maxBatchesToProcess)
                    break; //while(true)

                File.AppendAllText(processingLogFile, $"Processing records {offset + 1} - {offset + ShowtimeRecords.Count} {Environment.NewLine}");

                offset += ShowtimeRecords.Count;
                ++currentBatch;

                //Creating solr docs
                foreach (var showtimeRecord in ShowtimeRecords)
                {
                    try
                    {
                        Showtime showtime = JsonSerializer.Deserialize<Showtime>(showtimeRecord.content.ToString());

                        MovieRecord movieRecord = context.MovieRecords.FirstOrDefault(m => m.movie_id == showtimeRecord.movie_id);
                        Movie movie = JsonSerializer.Deserialize<Movie>(movieRecord!.content);

                        TheaterRecord theaterRecord = context.TheaterRecords.FirstOrDefault(m => m.theater_id == showtimeRecord.theater_id);
                        Theater theater = JsonSerializer.Deserialize<Theater>(theaterRecord!.content);  //theaters.FirstOrDefault(th => th.theater_id == showtime.theater_id);

                        SolrDoc doc = new SolrDoc();

                        var showtime_id = $"{showtime!.movie_id}-{showtime!.theater_id}-{showtime!.show_date.ToString("yyyyMMdd")}";
                        doc.AddId(showtime_id);

                        //showtime properties
                        doc.AddField("movie_name_t", showtime!.movie_name!);
                        doc.AddField("show_date_dt", showtime.show_date);

                        if (showtime.showtimes?.Length > 0)
                            doc.AddField("showtimes_ts", showtime.showtimes);

                        if (showtime.showtime_minutes?.Length > 0)
                            doc.AddField("showtime_minutes_is", showtime.showtime_minutes.ToArray());

                        if (showtime.show_attributes?.Length > 0)
                            doc.AddField("show_attributes_ts", showtime.show_attributes);

                        if (!string.IsNullOrEmpty(showtime.show_passes))
                            doc.AddField("show_passes_t", showtime.show_passes);

                        if (!string.IsNullOrEmpty(showtime.show_festival))
                            doc.AddField("show_festival_t", showtime.show_festival);

                        if (!string.IsNullOrEmpty(showtime.show_with))
                            doc.AddField("show_with_t", showtime.show_with);

                        if (!string.IsNullOrEmpty(showtime.show_sound))
                            doc.AddField("show_sound_t", showtime.show_sound);

                        if (showtime.show_comments?.Length > 0)
                            doc.AddField("show_comments_ts", showtime.show_comments);

                        if (movie == null)
                            File.AppendAllText(errorLogFile, $"Movie {showtime.movie_id} Not founnd in batch {showtimeRecord.batch}{Environment.NewLine}");
                        else
                            doc = AddMovie(doc, movie);


                        if (theater == null)
                            File.AppendAllText(errorLogFile, $"Theater {showtime.theater_id} Not founnd in batch {showtimeRecord.batch}{Environment.NewLine}");
                        else
                            doc = AddTheater(doc, theater);

                        solrDocs.Add(doc);

                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(errorLogFile, $"EXCEPTION in showtime id {showtimeRecord.id}, movie {showtimeRecord.movie_id}, theater {showtimeRecord.theater_id}: {ex.Message}{Environment.NewLine}");
                    }

                }//End: foreach (var showtimeRecord in ShowtimeRecords)

                //Call solr service to index the batch of docs
                ISolrService solr = _testHelper.Solr;
                solr.Index(solrDocs).Wait();

                solr.CommitAsync().Wait();

                //Clearning the bufffer
                solrDocs.Clear();

            }//End: while(true)


            var timelapse = (DateTime.Now - start).ToString();
            string logText = $"Total indexing time: {timelapse}{Environment.NewLine}";
            File.AppendAllText(processingLogFile, logText);

        }


        private SolrDoc AddTheater(SolrDoc doc, Theater theater)
        {
            doc.AddField("theater_name_t", theater.theater_name!);
            doc.AddField("theater_id_i", theater.theater_id);

            if(!string.IsNullOrEmpty(theater.theater_address))
                doc.AddField("theater_address_t", theater.theater_address);

            if (!string.IsNullOrEmpty(theater.theater_city))
                doc.AddField("theater_city_t", theater.theater_city);

            if (!string.IsNullOrEmpty(theater.theater_state))
                doc.AddField("theater_state_t", theater.theater_state);

            if (!string.IsNullOrEmpty(theater.theater_zip))
                doc.AddField("theater_zip_t", theater.theater_zip);

            if (!string.IsNullOrEmpty(theater.theater_phone))
                doc.AddField("theater_phone_t", theater.theater_phone);

            if(theater.theater_attributes?.Length > 0)
                doc.AddField("theater_attributes_ts", theater.theater_attributes.ToString());

            if (!string.IsNullOrEmpty(theater.theater_ticketing))
                doc.AddField("theater_ticketing_t", theater.theater_ticketing);

            if (!string.IsNullOrEmpty(theater.theater_closed_reason))
                doc.AddField("theater_closed_reason_t", theater.theater_closed_reason);

            if (!string.IsNullOrEmpty(theater.theater_area))
                doc.AddField("theater_area_t", theater.theater_area);

            if (!string.IsNullOrEmpty(theater.theater_location))
                doc.AddField("theater_location_t", theater.theater_location);

            if (!string.IsNullOrEmpty(theater.theater_market))
                doc.AddField("theater_market_t", theater.theater_market);

            if (theater.theater_screens.HasValue)
                doc.AddField("theater_screens_i", theater.theater_screens.Value);

            if (!string.IsNullOrEmpty(theater.theater_seating))
                doc.AddField("theater_seating_t", theater.theater_seating);

            if (!string.IsNullOrEmpty(theater.theater_adult))
                doc.AddField("theater_adult_t", theater.theater_adult);

            if (!string.IsNullOrEmpty(theater.theater_child))
                doc.AddField("theater_child_t", theater.theater_child);

            if (!string.IsNullOrEmpty(theater.theater_senior))
                doc.AddField("theater_senior_t", theater.theater_senior);

            if (!string.IsNullOrEmpty(theater.theater_country))
                doc.AddField("theater_country_t", theater.theater_country);

            if (!string.IsNullOrEmpty(theater.theater_url))
                doc.AddField("theater_url_t", theater.theater_url);

            if (!string.IsNullOrEmpty(theater.theater_chain_id))
                doc.AddField("theater_chain_id_t", theater.theater_chain_id);

            if (!string.IsNullOrEmpty(theater.theater_adult_bargain))
                doc.AddField("theater_adult_bargain_t", theater.theater_adult_bargain);

            if (!string.IsNullOrEmpty(theater.theater_senior_bargain))
                doc.AddField("theater_senior_bargain_t", theater.theater_senior_bargain);
            if (!string.IsNullOrEmpty(theater.theater_child_bargain))
                doc.AddField("theater_child_bargain_t", theater.theater_child_bargain); ;
            if (!string.IsNullOrEmpty(theater.theater_special_bargain))
                doc.AddField("theater_special_bargain_t", theater.theater_special_bargain);

            if (!string.IsNullOrEmpty(theater.theater_adult_super))
                doc.AddField("theater_adult_super_t", theater.theater_adult_super);

            if (!string.IsNullOrEmpty(theater.theater_senior_super))
                doc.AddField("theater_senior_super_t", theater.theater_senior_super);
            if (!string.IsNullOrEmpty(theater.theater_child_super))
                doc.AddField("theater_child_super_t", theater.theater_child_super);
            if (!string.IsNullOrEmpty(theater.theater_price_comment))
                doc.AddField("theater_price_comment_t", theater.theater_price_comment);
            if (!string.IsNullOrEmpty(theater.theater_extra))
                doc.AddField("theater_extra_t", theater.theater_extra);
            if (!string.IsNullOrEmpty(theater.theater_desc))
                doc.AddField("theater_desc_t", theater.theater_desc);
            if (!string.IsNullOrEmpty(theater.theater_type))
                doc.AddField("theater_type_t", theater.theater_type);
            if (theater.theater_lat.HasValue)
                doc.AddField("theater_lat_d", theater.theater_lat.Value);
            if (theater.theater_lon.HasValue)
                doc.AddField("theater_lon_d", theater.theater_lon.Value);

            return doc;
        }

        private SolrDoc AddMovie(SolrDoc doc, Movie movie)
        {
            doc.AddField("movie_id_i", movie.movie_id);
            doc.AddField("parent_id_i", movie.parent_id);
            doc.AddField("title_t", movie.title!);
            doc.AddField("genres_ts", movie.genres.ToArray());
            doc.AddField("pictures_ts", movie.pictures.ToArray());
            doc.AddField("hipictures_ts", movie.hipictures.ToArray());
            if (!string.IsNullOrEmpty(movie.rating))
                doc.AddField("rating_t", movie.rating);

            if (!string.IsNullOrEmpty(movie.advisory))
                doc.AddField("advisory_t", movie.advisory);

            doc.AddField("casts_ts", movie.casts.ToArray());

            doc.AddField("directors_ts", movie.directors.ToArray());

            if(movie.release_date.HasValue)
                doc.AddField("release_date_dt", val: movie.release_date.Value);
            if(!string.IsNullOrEmpty(movie.release_notes))
                doc.AddField("release_notes_t", movie.release_notes);

            if (!string.IsNullOrEmpty(movie.release_dvd))
                doc.AddField("release_dvd_t", movie.release_dvd);

            doc.AddField("running_time_i", movie.running_time);

            if (!string.IsNullOrEmpty(movie.official_site))
                doc.AddField("official_site_t", movie.official_site);

            doc.AddField("distributors_ts", movie.distributors.ToArray());
            doc.AddField("producers_ts", movie.producers.ToArray());
            doc.AddField("writers_ts", movie.writers.ToArray());

            if (!string.IsNullOrEmpty(movie.synopsis))
                doc.AddField("synopsis_t", movie.synopsis);
            if (!string.IsNullOrEmpty(movie.lang))
                doc.AddField("lang_t", movie.lang);
            if (!string.IsNullOrEmpty(movie.intl_name))
                doc.AddField("intl_name_t", movie.intl_name);

            if (!string.IsNullOrEmpty(movie.intl_country))
                doc.AddField("intl_country_t", movie.intl_country);
            if (!string.IsNullOrEmpty(movie.intl_cert))
                doc.AddField("intl_cert_t", movie.intl_cert);
            if (!string.IsNullOrEmpty(movie.intl_advisory))
                doc.AddField("intl_advisory_t", movie.intl_advisory);
            if (movie.intl_release.HasValue)
                doc.AddField("intl_release_dt", movie.intl_release.Value);
            if (!string.IsNullOrEmpty(movie.intl_poster))
                doc.AddField("intl_poster_t", movie.intl_poster);
      
            return doc;
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
        public List<string> GetGrandChildElementValues(XElement parent, string childElementName, string grandChieldElementName)
        {
            var vals = GetChildElement(parent, childElementName)?.Elements(grandChieldElementName)
                .Where(grandchild => !string.IsNullOrEmpty(grandchild.Value))
                .Select(grandchild => grandchild.Value).ToList();

            return vals != null ? vals : new List<string>();
        }
        public string? GetElementAttStr(XElement parent, string elementName, string attName) => GetChildElement(parent, elementName)?.Attribute(attName)?.Value;
        public DateTime GetElementAttDateYYYYDDMM(XElement parent, string elementName, string attName)
        {
            var datestr = GetElementAttStr(parent, elementName, attName);
            return new DateTime(int.Parse(datestr!.Substring(0, 4)), int.Parse(datestr!.Substring(4, 2)), int.Parse(datestr!.Substring(6, 2)));
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
            pictures = GetGrandChildElementValues(xml, "pictures", "photos");
            hipictures = GetGrandChildElementValues(xml, "hipictures", "photos");
            rating = GetElementValueStr(xml, "rating");
            advisory = GetElementValueStr(xml, "advisory");
            genres = GetGrandChildElementValues(xml, "genres", "genre");
            casts = GetGrandChildElementValues(xml, "casts", "cast");
            directors = GetGrandChildElementValues(xml, "directors", "director");
            release_date = GetElementValueDateTime(xml, "release_date");
            release_notes = GetElementValueStr(xml, "release_notes");
            release_dvd = GetElementValueStr(xml, "release_dvd");
            running_time = GetElementValueInt(xml, "running_time", -1);
            official_site = GetElementValueStr(xml, "official_site");
            distributors = GetGrandChildElementValues(xml, "distributors", "distributor");
            producers = GetGrandChildElementValues(xml, "producers", "producer");
            writers = GetGrandChildElementValues(xml, "writers", "writer");
            synopsis = GetElementValueStr(xml, "synopsis");
            lang = GetElementValueStr(xml, "lang");
            intl_country = GetElementAttStr(xml, "intl", "country");

            XElement intl = GetChildElement(xml, "intl");
            intl_name = GetElementValueStr(intl, "intl_name");
            intl_cert = GetElementValueStr(intl, "intl_cert");
            intl_advisory = GetElementValueStr(intl, "intl_advisory");
            intl_release = GetElementValueDateTime(intl, "intl_release");
            intl_poster = GetElementValueStr(intl, "intl_poster");
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
    }

    /**
     * Showtime class
     */
    public class Showtime: XmlDoc
    {

        public int movie_id { get; set; }
        public string? movie_name { get; set; }
        public int theater_id { get; set; }
        public DateTime show_date { get; set; }
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
            show_date = GetElementAttDateYYYYDDMM(xml, "show_date", "date");

            XElement show_date_element = GetChildElement(xml, "show_date");
            showtimes = GetElementValueStr(show_date_element, "showtimes", ",");

            var showTimeMunitesList = new List<int>();
            foreach(var st in showtimes!)
            {
                var hhmm = st.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim('.'))).ToArray();
                if (hhmm?.Length > 0)
                    showTimeMunitesList.Add(hhmm[0] * 60 + hhmm[1]);
            }
            showtime_minutes= showTimeMunitesList.ToArray();

            show_attributes = GetElementValueStr(show_date_element, "show_attributes", ",");
            show_passes = GetElementValueStr(show_date_element, "show_passes");
            show_festival = GetElementValueStr(show_date_element, "show_festival");
            show_with = GetElementValueStr(show_date_element, "show_with");
            show_sound = GetElementValueStr(show_date_element, "show_sound");
            show_comments = GetElementValueStr(show_date_element, "show_comments", ";");

        }

    }


    public class MovieRecord
    {
        public int id { get; set; }
        public int movie_id { get; set; }
        public int batch { get; set; }
        public string content { get; set; }
    }
    public class TheaterRecord
    {
        public int id { get; set; }
        public int theater_id { get; set; }
        public int batch { get; set; }
        public string content { get; set; }
    }

    public class ShowtimeRecord
    {
        public int id { get; set; }
        public int movie_id { get; set; }
        public int theater_id { get; set; }
        public DateTime show_date { get; set; }
        public int batch { get; set; }
        public string content { get; set; }
    }

    public class ShowtimeDbContext : DbContext
    {
        public DbSet<MovieRecord> MovieRecords { get; set; }
        public DbSet<TheaterRecord> TheaterRecords { get; set; }
        public DbSet<ShowtimeRecord> ShowtimeRecords { get; set; }

        public ShowtimeDbContext(DbContextOptions<ShowtimeDbContext> options)
            : base(options)
        {
        }
    }


    public class TransactionalTestDatabaseFixture
    {
        private const string ConnectionString = @"Server=.\\;Database=showtime;User Id=showtime;Password=password;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ShowtimeDbContext CreateContext()
            => new ShowtimeDbContext(
                new DbContextOptionsBuilder<ShowtimeDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

        public TransactionalTestDatabaseFixture()
        {
            using var context = CreateContext();
           // context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //Cleanup();
        }

        public void Cleanup()
        {
            using var context = CreateContext();

            ////    context.AddRange(
            ////        new Blog { Name = "Blog1", Url = "http://blog1.com" },
            ////        new Blog { Name = "Blog2", Url = "http://blog2.com" });
            ////    context.SaveChanges();
        }
    }

        /*
        public class DatabaseFixture : IDisposable
        {
            public DatabaseFixture()
            {
                Db = new SqlConnection("Server=.\\;Database=showtime;User Id=catfishd;Password=password;Trusted_Connection=True;MultipleActiveResultSets=true");

                // ... initialize data in the test database ...
            }

            public void Dispose()
            {
                // ... clean up test data from the database ...
            }

            public SqlConnection Db { get; private set; }
        }

        [CollectionDefinition("Database collection")]
        public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }

        */
    }
