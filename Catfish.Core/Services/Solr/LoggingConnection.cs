using log4net;
using SolrNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catfish.Core.Services.Solr
{
    public class LoggingConnection : ISolrConnection
    {
        private readonly ISolrConnection connection;

        public LoggingConnection(ISolrConnection connection)
        {
            this.connection = connection;
        }

        public string Post(string relativeUrl, string s)
        {
            logger.DebugFormat("POSTing '{0}' to '{1}'", s, relativeUrl);
            return connection.Post(relativeUrl, s);
        }

        public string PostStream(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            logger.DebugFormat("POSTing to '{0}'", relativeUrl);
            return connection.PostStream(relativeUrl, contentType, content, getParameters);
        }

        public string Get(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var stringParams = string.Join(", ", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)).ToArray());
            logger.DebugFormat("GETting '{0}' from '{1}'", stringParams, relativeUrl);
            return connection.Get(relativeUrl, parameters);
        }

        public Task<string> PostAsync(string relativeUrl, string s)
        {
            return connection.PostAsync(relativeUrl, s);
        }

        public Task<string> PostStreamAsync(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            return connection.PostStreamAsync(relativeUrl, contentType, content, getParameters);
        }

        public Task<string> GetAsync(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken = default)
        {
            return connection.GetAsync(relativeUrl, parameters, cancellationToken);
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(LoggingConnection));
    }
    
}
