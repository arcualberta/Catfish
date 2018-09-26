using Catfish.Core.Models;
using Catfish.Core.Models.Ingestion;
using Catfish.Core.Services;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Data.Entity;

namespace Catfish.DataHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Check for a SolrConnection
            string solrString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            if (!string.IsNullOrEmpty(solrString))
            {
                SolrService.Init(solrString);
            }
            else
            {
                Console.Error.WriteLine("Could not initialize connection to the Solr server. Please verify that your SolrServer app property is correct.");
                return;
            }

            if (args.Length > 0)
            {
                Database.SetInitializer<CatfishDbContext>(null);
                using (CatfishDbContext Db = new CatfishDbContext())
                {
                   
                    if (args[0] == "import")
                    {
                        import(Db);
                    }
                    else if (args[0] == "export")
                    {
                        export(Db);
                    }
                }
            }
        }

        private static void import(CatfishDbContext Db)
        {
#if DEBUG
            Console.Error.WriteLine("Starting ingestion import...");
#endif

            Console.InputEncoding = Encoding.UTF8;

            IngestionService srv = new IngestionService(Db);
            srv.Import(Console.OpenStandardInput(), 4);

            Db.SaveChanges();

        }

        private static void export(CatfishDbContext Db)
        {
            IngestionService srv = new IngestionService(Db);
            Stream stream = Console.OpenStandardOutput();
            srv.Export(stream);

            StreamWriter standardOutput = new StreamWriter(stream);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
           
        }
    }
}
