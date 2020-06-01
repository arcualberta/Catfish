using System;
using SolrNet;
using SolrNet.Exceptions;

namespace Catfish.Core.Services.Solr
{
    public class SolrIndexService<T, TSolrOperations> : ISolrIndexService<T>
    where TSolrOperations : ISolrOperations<T>
    {
		private readonly TSolrOperations _solr;
		public SolrIndexService(ISolrOperations<T> solr)
		{
			_solr = (TSolrOperations)solr;
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
			catch (SolrNetException ex)
			{
				//Log exception
				throw ex;
				//return false;

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
			catch (SolrNetException ex)
			{
				//Log exception
				throw ex;
			}
		}
	}
}
