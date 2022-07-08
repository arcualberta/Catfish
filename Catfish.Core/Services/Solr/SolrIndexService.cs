using System;
using System.Collections.Generic;
using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using ElmahCore;
using SolrNet;
using SolrNet.Exceptions;

namespace Catfish.Core.Services.Solr
{
    public class SolrIndexService<T, TSolrOperations> : ISolrIndexService<T>
    where TSolrOperations : ISolrOperations<T>
    {
		private readonly TSolrOperations _solr;
		private readonly ErrorLog _errorLog;
		public SolrIndexService(ISolrOperations<T> solr, ErrorLog errorLog)
		{
			_solr = (TSolrOperations)solr;
			_errorLog = errorLog;
		}

		public bool AddUpdate(T document)
		{
			try
			{
				// If the id already exists, the record is updated, otherwise added                         
				_solr.Add(document);
				_solr.Commit();
				_solr.Optimize();
				return true;
			}
			catch ( Exception ex)
			{
				//Log exception
				_errorLog.Log(new Error(ex));
				return false;

			}
		}

		public bool Delete(T document)
		{
			try
			{
				//Can also delete by id                
				_solr.Delete(document);
				_solr.Commit();
				_solr.Optimize();
				return true;
			}
			catch (Exception ex)
			{
				//Log exception
				_errorLog.Log(new Error(ex));
				return false;
			}
		}
	}
}
