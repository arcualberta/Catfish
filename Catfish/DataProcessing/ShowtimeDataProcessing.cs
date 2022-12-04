using Catfish.Test.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
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

            var srcBatcheFolders = Directory.GetDirectories(srcFolderRoot);
            foreach(var batchFolder in srcBatcheFolders)
            {
                var batchName = batchFolder.Substring(batchFolder.LastIndexOf("\\") + 1);
                var zipFiles = Directory.GetFiles(batchFolder);
                foreach(var zipFile in zipFiles)
                {
                    List<XElement> showtimes = new List<XElement>();
                    XElement moviesElement = null;
                    XElement theatres = null;

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
                                showtimes.Add(xml);
                            }
                            else if (xml.Name == "movies")
                            {
                                //This is a movies data set. 
                                if (moviesElement == null)
                                    moviesElement = xml;
                                else
                                    foreach (var ele in xml.Elements("movie"))
                                        moviesElement.Add(ele);
                            }
                            else if (xml.Name == "houses")
                            {
                                //This is a theatres data set
                                if (theatres == null)
                                    theatres = xml;
                                else
                                    foreach (var ele in xml.Elements("theater"))
                                        theatres.Add(ele);
                            }

                            File.Delete(extractFile);
                           
                        } //End: foreach (ZipArchiveEntry entry in archive.Entries)
                    } //End:  using (ZipArchive archive = ZipFile.OpenRead(zipFile))

                    //Processing Data
                    var movies = new List<Movie>();
                    foreach (var m in moviesElement.Elements("movie"))
                        movies.Add(new Movie(m));

                    foreach (var showtimeDataSet in showtimes)
                    {
                        var showtimeSubset = showtimeDataSet.Elements("showtime");
                        foreach(var showtime in showtimeSubset)
                        {
                        }
                    }
                }

            }
        }
    }

    public class Movie
    {
        public int id { get; set; } = -1;
        public int parent_id { get; set; }
        public string title { get; set; }
        public List<string> pictures { get; set; } = new List<string>();
        public List<string> hipictures { get; set; } = new List<string>();
        public string rating { get; set; }
        public string advisory { get; set; }
        public List<string> genres { get; set; } = new List<string>();
        public List<string> casts { get; set; } = new List<string>();
        public List<string> directors { get; set; } = new List<string>();
        public DateTime release_date { get; set; }
        public string release_notes { get; set; }
        public string release_dvd { get; set; }
        public int running_time { get; set; }
        public string official_site { get; set; }
        public List<string> distributors { get; set; } = new List<string>();
        public List<string> producers { get; set; } = new List<string>();
        public List<string> writers { get; set; } = new List<string>();
        public string synopsis { get; set; }
        public string lang { get; set; }
        public string intl_country { get; set; }
        public string intl_name { get; set; }
        public string intl_cert { get; set; }
        public string intl_advisory { get; set; }
        public DateTime intl_release { get; set; }
        public string intl_poster { get; set; }

        public Movie(XElement xml)
        {
            if (int.TryParse(xml.Element("movie_id")?.Value, out int n))
                id = n;
            if (int.TryParse(xml.Element("parent_id")?.Value, out n))
                parent_id = n;

            title = xml.Element("title")!.Value;

        }
    }

    public class Theatre
    {

    }

    public class Showtime
    {

    }
}
