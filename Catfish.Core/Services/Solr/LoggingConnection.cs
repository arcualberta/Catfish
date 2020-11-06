using ElmahCore;
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
        private readonly ErrorLog _errorLog;

        public LoggingConnection(ISolrConnection connection, ErrorLog errorLog)
        {
            this.connection = connection;
            _errorLog = errorLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public string Post(string relativeUrl, string s)
        {
            try
            {
                logger.DebugFormat("POSTing '{0}' to '{1}'", s, relativeUrl);
                return connection.Post(relativeUrl, s);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <param name="getParameters"></param>
        /// <returns></returns>
        public string PostStream(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            try
            {
                logger.DebugFormat("POSTing to '{0}'", relativeUrl);
                return connection.PostStream(relativeUrl, contentType, content, getParameters);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string Get(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            try
            {
                var stringParams = string.Join(", ", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)).ToArray());
                logger.DebugFormat("GETting '{0}' from '{1}'", stringParams, relativeUrl);
                return connection.Get(relativeUrl, parameters);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public Task<string> PostAsync(string relativeUrl, string s)
        {
            try
            {
                return connection.PostAsync(relativeUrl, s);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <param name="getParameters"></param>
        /// <returns></returns>
        public Task<string> PostStreamAsync(string relativeUrl, string contentType, Stream content, IEnumerable<KeyValuePair<string, string>> getParameters)
        {
            try
            {
                return connection.PostStreamAsync(relativeUrl, contentType, content, getParameters);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetAsync(string relativeUrl, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                return connection.GetAsync(relativeUrl, parameters, cancellationToken);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(LoggingConnection));
    }
    
}
