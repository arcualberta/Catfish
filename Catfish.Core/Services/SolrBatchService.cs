using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services
{
    public class SolrBatchService : ISolrBatchService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;
        private readonly ISolrService _solr;
        private readonly int _indexBatchSize;
        private readonly bool _indexFieldNames;
        public SolrBatchService(IConfiguration config, AppDbContext db, ISolrService solr)
        {
            _config = config;
            _db = db;
            _solr = solr;
            var batchSize = _config.GetSection("SolarConfiguration:IndexBatchSize").Value;
            _indexBatchSize = string.IsNullOrEmpty(batchSize) ? 1000 : int.Parse(batchSize);
         
            _indexFieldNames = false;
            _ = bool.TryParse(_config.GetSection("SolarConfiguration:IndexFieldNames").Value, out _indexFieldNames);
        }

        public void IndexItems(bool forceReindexingAll)
        {
            if (forceReindexingAll)
            {
                foreach (var h in _db.IndexingHistory.ToList())
                    _db.IndexingHistory.Remove(h);

                _db.SaveChanges();
            }

            List<SolrDoc> docsToIndex = new List<SolrDoc>();
            List<IndexingHistory> indexingHistoriesToUpdate = new List<IndexingHistory>();
            List<Item> items;
            int offset = 0;
            do
            {
                //Maximum number of items to be taken in this iteration
                int take = _indexBatchSize - docsToIndex.Count;

                //Loading items
                items = _db.Items.Skip(offset).Take(take).ToList();

                //Updating the offset for the next iteration
                offset += items.Count;

                //Iterating through items and 
                foreach (var item in items)
                {
                    var indexHistory = forceReindexingAll ? null : _db.IndexingHistory.Where(h => h.ObjectId == item.Id).FirstOrDefault();
                    if (indexHistory == null)
                    {
                        indexHistory = new IndexingHistory()
                        {
                            ObjectId = item.Id,
                            Created = DateTime.Now,
                            LastIndexedAt = new DateTime(1, 1, 1)
                        };
                        _db.IndexingHistory.Add(indexHistory);
                    }

                    if (indexHistory.LastIndexedAt < item.Updated)
                    {
                        docsToIndex.Add(new SolrDoc(item, _indexFieldNames));
                        indexingHistoriesToUpdate.Add(indexHistory);
                    }
                }

                //If a sufficient number of documents were collected for indexing or if there are no more
                //document for indexing (i.e. the current count loaded from the database is smaller than
                //the specified "take", then we index the current list of items prepared for indexing.
                if (docsToIndex.Count >= _indexBatchSize || items.Count < take)
                {
                    if (docsToIndex.Count > 0)
                    {
                        _solr.Index(docsToIndex);

                        foreach (var h in indexingHistoriesToUpdate)
                            h.LastIndexedAt = DateTime.Now;
                        _db.SaveChanges();

                        docsToIndex.Clear();
                        indexingHistoriesToUpdate.Clear();
                    }
                }
            }
            while (items.Count > 0);
        }
    }
}
