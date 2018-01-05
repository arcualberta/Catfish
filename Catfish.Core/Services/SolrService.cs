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
    public class SolrService
    {
        public static bool IsInitialized { get; private set; }

        private static SolrConnection mSolr { get; set; }

        public static void Init(string server)
        {
            IsInitialized = false;
            if (!string.IsNullOrEmpty(server))
            {
                mSolr = new SolrConnection(server);
                Startup.Init<SolrIndex>(mSolr);

                //TODO: Should we update the database here or have it in an external cron job

                IsInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("The app parameter Solr Server string has not been defined.");
            }
        }

        public SolrService()
        {
        }
    }

    public class SolrIndex
    {
        public int Id { get; set; }

    }
}
