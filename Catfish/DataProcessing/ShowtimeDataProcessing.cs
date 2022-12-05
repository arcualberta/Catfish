using Catfish.API.Repository.Solr;
using Catfish.Test.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataProcessing
{
    public class ShowtimeDataProcessing
    {
        public readonly TestHelper _testHelper;
        public int MAX_RECORDS = 1; //DEBUG ONLY -- set it to 0 or -1 to ignore it

        //public class ShowtimeDataProcessingFixture: IDisposable
        //{
        //    public ShowtimeDataProcessingFixture()
        //    {
               
        //    }

        //    public void Dispose()
        //    {
        //        // ... clean up test data from the database ...
        //    }
        //}
        public ShowtimeDataProcessing()
        {
            _testHelper = new TestHelper();
        }

        [Fact]
        public void IndexData()
        {
            DateTime start = DateTime.Now;

            string srcFolderRoot = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));
            string outputFolder = "C:\\Projects\\Showtime Database\\output";
            Directory.CreateDirectory(outputFolder);

            string fileSuffix = start.ToString("yyyy-MM-dd_HH-mm-ss");
            string processingLogFile = Path.Combine(outputFolder, $"processing-log-{fileSuffix}.txt");
            string errorLogFile = Path.Combine(outputFolder, $"error-log-{fileSuffix}.txt");

            int indexingBatchSize = 1000;
            List<SolrDoc> solrDocs = new List<SolrDoc>();
            int totalShowtimeRecordCount = 0;
            int totalIndexedRecordCount = 0;
            var srcBatcheFolders = Directory.GetDirectories(srcFolderRoot);
            int batchCount = 0;
            foreach (var batchFolder in srcBatcheFolders)
            {
                var batchName = batchFolder.Substring(batchFolder.LastIndexOf("\\") + 1);
                var zipFiles = Directory.GetFiles(batchFolder);
                foreach (var zipFile in zipFiles)
                {
                    List<Movie> movies = new List<Movie>();
                    List<Theater> theaters = new List<Theater>();
                    List<Showtime> showtimes = new List<Showtime>();

                    File.AppendAllText(processingLogFile, $"Archive {zipFile}{Environment.NewLine}");

                    using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            try
                            {

                                var extractFile = Path.Combine(outputFolder, entry.Name);
                                if (File.Exists(extractFile))
                                    File.Delete(extractFile);

                                entry.ExtractToFile(extractFile);

                                XElement xml = XElement.Load(extractFile);
                                if (xml.Name == "times")
                                {
                                    //This is a showtime data set
                                    foreach (var child in xml.Elements("showtime"))
                                        showtimes.Add(new Showtime(child));
                                }
                                else if (xml.Name == "movies")
                                {
                                    //This is a movies data set. 
                                    foreach (var child in xml.Elements("movie"))
                                        movies.Add(new Movie(child));
                                }
                                else if (xml.Name == "houses")
                                {
                                    //This is a theatres data set
                                    foreach (var child in xml.Elements("theater"))
                                        theaters.Add(new Theater(child));
                                }

                                File.Delete(extractFile);
                            }
                            catch(Exception ex)
                            {
                                File.AppendAllText(errorLogFile, $"EXCEPTION in {entry.Name}: {ex.Message}{Environment.NewLine}");
                            }

                        } //End: foreach (ZipArchiveEntry entry in archive.Entries)

                    } //End:  using (ZipArchive archive = ZipFile.OpenRead(zipFile))

                    totalShowtimeRecordCount += showtimes.Count;
                    File.AppendAllText(processingLogFile, $"Movies: {movies.Count}, Theaters: {theaters.Count}, Showtimes: {showtimes.Count}{Environment.NewLine}");

                    //Creating solr docs
                    foreach (var showtime in showtimes)
                    {
                        try
                        {


                            Movie movie = movies.FirstOrDefault(mv => mv.movie_id == showtime.movie_id);
                            Theater theater = theaters.FirstOrDefault(th => th.theater_id == showtime.theater_id);

                            SolrDoc doc = new SolrDoc();
                            doc.AddId(Guid.NewGuid());

                            if (movie == null)
                                File.AppendAllText(errorLogFile, $"Movie {showtime.movie_id} Not founnd in {zipFile}{Environment.NewLine}");
                            else
                            {
                                //TODO:add the full list of movie fields

                                doc.AddField("title_t", movie.title!);
                                doc.AddField("genres_ts", movie.genres.ToArray());
                            }



                            if (theater == null)
                                File.AppendAllText(errorLogFile, $"Theater {showtime.theater_id} Not founnd in {zipFile}{Environment.NewLine}");
                            else
                            {
                                //TODO: add the full list of theater fields
                                doc.AddField("theater_name_t", theater.theater_name!);

                            }

                            solrDocs.Add(doc);

                            if (solrDocs.Count >= indexingBatchSize)
                            {
                                //TODO: call solr service to index the batch of docs


                                //Clearning the bufffer
                                totalIndexedRecordCount += solrDocs.Count;
                                solrDocs.Clear();

                            }
                        }
                        catch(Exception ex)
                        {
                            File.AppendAllText(errorLogFile, $"EXCEPTION in showtime date {showtime.show_date}, movie {showtime.movie_id}, theater {showtime.theater_id}: {ex.Message}{Environment.NewLine}");
                        }

                        
                    }
                }

                //DEBUG ONLY -- ONLY PROCESS SMALL NUMBER OF BATCH
                batchCount++;

                //DEBUG -- ONLY PROCESS SMALL AMOUNT OF DOCS
                if ( MAX_RECORDS > 0 && batchCount >= MAX_RECORDS)
                    break;
            }

            if(solrDocs.Count > 0)
            {
                //TODO: Index this remaining batch of solr docs.

                totalIndexedRecordCount += solrDocs.Count;

            }

            File.AppendAllText(processingLogFile, $"Total showtime records: {totalShowtimeRecordCount}, Successfully indexed: {totalIndexedRecordCount}, Total time: {(DateTime.Now - start).ToString("hh:mm:ss")}");

          
        }
    }

    /**
     * XmlDoc base class
     */
    public class XmlDoc
    {
        private XElement _xml;
        public XmlDoc(XElement xml)
        {
            _xml = xml;
        }

        public string? GetElementValueStr(string elementName) => _xml.Element(elementName)?.Value;
        public string[]? GetElementValueStr(string elementName, string separator) => GetElementValueStr(elementName)?.Split(separator).Select(str => str.Trim()).Where(str => !string.IsNullOrEmpty(str)).ToArray();
        public bool GetElementValueInt(string elementName, out int val) => int.TryParse(_xml.Element(elementName)?.Value, out val);
        public int GetElementValueInt(string elementName, int defaultValue) => GetElementValueInt(elementName, out int n) ? n : defaultValue;
        public DateTime? GetElementValueDateTime(string elementName) => DateTime.TryParse(_xml.Element(elementName)?.Value, out DateTime val) ? val : null;
        public decimal? GetElementValueDecimal(string elementName) => decimal.TryParse(_xml.Element(elementName)?.Value, out decimal val) ? val : null;
        public List<string> GetElementValues(string elementName, string childName)
        {
            var vals = _xml!.Element(elementName)?.Elements(childName)
                .Where(child => !string.IsNullOrEmpty(child.Value))
                .Select(child => child.Value).ToList();

            return vals != null ? vals : new List<string>();
        }
        public string? GetElementAttStr(string elementName, string attName) => _xml.Element(elementName)?.Attribute(attName)?.Value;
        public DateTime? GetElementAttDate(string elementName, string attName) => DateTime.TryParse(GetElementAttStr(elementName, attName), out DateTime t) ? t : null;

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

        public Movie(XElement xml) : base(xml)
        {
            movie_id = GetElementValueInt("movie_id", -1);
            parent_id = GetElementValueInt("parent_id", -1);
            title = GetElementValueStr("title");
            pictures = GetElementValues("pictures", "photos");
            hipictures = GetElementValues("hipictures", "photos");
            rating = GetElementValueStr("rating");
            advisory = GetElementValueStr("advisory");
            genres = GetElementValues("genres", "genre");
            casts = GetElementValues("casts", "cast");
            directors = GetElementValues("directors", "director");
            release_date = GetElementValueDateTime("release_date");
            release_notes = GetElementValueStr("release_notes");
            release_dvd = GetElementValueStr("release_dvd");
            running_time = GetElementValueInt("running_time", -1);
            official_site = GetElementValueStr("official_site");
            distributors = GetElementValues("distributors", "distributor");
            producers = GetElementValues("producers", "producer");
            writers = GetElementValues("writers", "writer");
            synopsis = GetElementValueStr("synopsis");
            lang = GetElementValueStr("lang");
            intl_country = GetElementAttStr("intl", "country");
            intl_name = GetElementValueStr("intl_name");
            intl_cert = GetElementValueStr("intl_cert");
            intl_advisory = GetElementValueStr("intl_advisory");
            intl_release = GetElementValueDateTime("intl_release");
            intl_poster = GetElementValueStr("intl_poster");
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
        public string[]? theater_attributes { get; set; }
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
        public string? theater_description { get; set; }
        public string? theater_type { get; set; }
        public decimal? theater_lat { get; set; }
        public decimal? theater_lon { get; set; }
        public Theater(XElement xml): base(xml)
        {
            theater_id = GetElementValueInt("theater_id", -1);
            theater_name = GetElementValueStr("theater_name");
            theater_address = GetElementValueStr("theater_address");
            theater_city = GetElementValueStr("theater_city");
            theater_state = GetElementValueStr("theater_state");
            theater_zip = GetElementValueStr("theater_zip");
            theater_phone = GetElementValueStr("theater_phone");
            theater_attributes = GetElementValueStr("theater_attributes", ","); //NEED TO CONFIRM WHAT WAS AS SEPARATOR!!!!


            theater_ticketing = GetElementValueStr("theater_ticketing");
            theater_closed_reason = GetElementValueStr("theater_closed_reason");
             theater_area = GetElementValueStr("theater_area");
           theater_location= GetElementValueStr("theater_location");
           theater_market = GetElementValueStr("theater_market");
             theater_screens = GetElementValueInt("theater_screens", -1);
            theater_seating = GetElementValueStr("theater_seating");
        theater_adult = GetElementValueStr("theater_adult");
        theater_child = GetElementValueStr("theater_child");
        theater_senior = GetElementValueStr("theater_senior");
          theater_country = GetElementValueStr("theater_country");
        theater_url = GetElementValueStr("theater_url");
        theater_chain_id = GetElementAttStr("theater_clain", "id");

            theater_adult_bargain = GetElementValueStr("theater_adult_bargain");
            theater_senior_bargain = GetElementValueStr("theater_senior_bargain");
            theater_child_bargain = GetElementValueStr("theater_child_bargain");
            theater_special_bargain = GetElementValueStr("theater_special_bargain");
            theater_adult_super = GetElementValueStr("theater_adult_super");
            theater_senior_super = GetElementValueStr("theater_senior_super");
            theater_child_super = GetElementValueStr("theater_child_super");
            theater_price_comment = GetElementValueStr("theater_price_comment");
            theater_extra = GetElementValueStr("theater_extra");
            theater_description = GetElementValueStr("theater_description");
            theater_type = GetElementValueStr("theater_type");

            theater_lon = GetElementValueDecimal("theater_lon");

            theater_lat = GetElementValueDecimal("theater_lat");
        }
    }

    /**
     * Showtime class
     */
    public class Showtime: XmlDoc
    {

        public int movie_id => GetElementValueInt("movie_id", -1);
        public int theater_id => GetElementValueInt("theater_id", -1);
        public DateTime? show_date => GetElementAttDate("show_date", "date");
        public string[]? showtimes => GetElementValueStr("showtimes", ",");
        public int[]? showtime_minutes { get; set; } = null;
        public string[]? show_attributes => GetElementValueStr("show_attributes", ",");
        public string? show_passes => GetElementValueStr("show_passes");
        public string? show_festival => GetElementValueStr("show_festival");
        public string? show_with => GetElementValueStr("show_with");
        public string? show_sound => GetElementValueStr("show_sound");
        public string[]? show_comments => GetElementValueStr("show_comments", ";");


        public Showtime(XElement xml) : base(xml)
        {
            if(showtimes != null)
            {
                showtime_minutes = new int[showtimes.Length];
                int i = 0;
                foreach (var showtime in showtimes)
                {
                    var hhmm = showtime.Split(":").Select(s => int.Parse(s)).ToArray();
                    showtime_minutes[i++] = hhmm[0] * 60 + hhmm[1];
                }
            }
        }

    }
}
