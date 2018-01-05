using Catfish.Core.Models;
using SolrNet;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    class SolrService
    {
        public static bool IsInitialized { get; private set; }

        protected CatfishDbContext Db { get; set; }

        private static SolrConnection mSolr { get; set; }

        public static void Init()
        {
            IsInitialized = false;

            string server = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            if (!string.IsNullOrEmpty(server))
            {
                mSolr = new SolrConnection(server);
                Startup.Init<SolrIndex>(mSolr);


                //TODO: Should we update the database here or have it in an external cron job
            }
            else
            {
                throw new InvalidOperationException("The app parameter SolrServer has not been defined.");
            }
        }

        public SolrService(CatfishDbContext db)
        {
            Db = db;
        }
    }

    public class SolrIndex
    {
        public int Id { get; set; }

    }
}
