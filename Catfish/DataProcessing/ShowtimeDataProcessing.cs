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
        public ShowtimeDataProcessing()
        {
            _testHelper = new TestHelper();
        }

        [Fact]
        public void IndexData()
        {
            string srcFolderRoot = "C:\\Projects\\Showtime Database\\cinema-source.com";
            Assert.True(Directory.Exists(srcFolderRoot));
            string tmpFolder = "C:\\Projects\\Showtime Database\\tmp";
            Directory.CreateDirectory(tmpFolder);

            int indexingBatchSize = 1000;
            List<SolrDoc> solrDocs = new List<SolrDoc>();

            var srcBatcheFolders = Directory.GetDirectories(srcFolderRoot);
            foreach (var batchFolder in srcBatcheFolders)
            {
                var batchName = batchFolder.Substring(batchFolder.LastIndexOf("\\") + 1);
                var zipFiles = Directory.GetFiles(batchFolder);
                foreach (var zipFile in zipFiles)
                {
                    List<Movie> movies = new List<Movie>();
                    List<Theatre> theatres = new List<Theatre>();
                    List<Showtime> showtimes = new List<Showtime>();

                    using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            var extractFile = Path.Combine(tmpFolder, entry.Name);
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
                                foreach (var child in xml.Elements("theatres"))
                                    theatres.Add(new Theatre(child));
                            }

                            File.Delete(extractFile);

                        } //End: foreach (ZipArchiveEntry entry in archive.Entries)
                    } //End:  using (ZipArchive archive = ZipFile.OpenRead(zipFile))

                    //Creating solr docs
                    foreach (var showtime in showtimes)
                    {
                        Theatre theatre = theatres.First(th => th.theater_id == showtime.theater_id);
                        Movie movie = movies.First(mv => mv.movie_id == showtime.movie_id);

                        SolrDoc doc = new SolrDoc();
                        doc.AddId(Guid.NewGuid());

                        if (!string.IsNullOrEmpty(movie.title)) doc.AddField("title_t", movie.title);
                        doc.AddField("genres_ts", movie.genres.ToArray());

                        //TODO: initialize the rest of the solr fields



                        solrDocs.Add(doc);

                        if (solrDocs.Count >= indexingBatchSize)
                        {
                            //TODO: call solr service to index the batch of docs


                            //Clearning the bufffer
                            solrDocs.Clear();
                        }
                    }
                }
            }

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
     * Theatre class
     */
    public class Theatre: XmlDoc
    {
        public int theater_id { get; set; }
        public string? theater_name { get; set; }
        public string? theater_address { get; set; }



        public decimal? theater_lat { get; set; }
        public Theatre(XElement xml): base(xml)
        {
            theater_id = GetElementValueInt("theater_id", -1);
            theater_name = GetElementValueStr("theater_name");
            theater_address = GetElementValueStr("theater_address");


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
        public int[]? showrime_minutes { get; set; } = null;
        public string? show_attributes => GetElementValueStr("show_attributes");
        public string? show_passes => GetElementValueStr("show_passes");
        public string? show_festival => GetElementValueStr("show_festival");
        public string? show_with => GetElementValueStr("show_with");
        public string? show_sound => GetElementValueStr("show_sound");
        public string[]? show_comments => GetElementValueStr("show_comments", ";");


        public Showtime(XElement xml) : base(xml)
        {
            if(showtimes != null)
            {
                showrime_minutes = new int[showtimes.Length];
                int i = 0;
                foreach (var showtime in showtimes)
                {
                    var hhmm = showtime.Split(":").Select(s => int.Parse(s)).ToArray();
                    showrime_minutes[i++] = hhmm[0] * 60 + hhmm[1];
                }
            }
        }

    }
}
