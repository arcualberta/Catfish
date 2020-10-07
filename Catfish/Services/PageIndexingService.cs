using Catfish.Core.Models.Solr;
using Catfish.Core.Services.Solr;
using Piranha;
using Piranha.Models;
using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class PageIndexingService : IPageIndexingService
    {
        private readonly IApi _api;
        private readonly ISolrIndexService<SolrEntry> _solrIndexService;
        private readonly IQueryService _solrQueryService;
        public PageIndexingService(ISolrIndexService<SolrEntry> iSrv, IQueryService qSrv, IApi api)
        {
            _api = api;
            _solrIndexService = iSrv;
            _solrQueryService = qSrv;
        }

        public void IndexBlock(Block block, SolrEntry entry)
        {
            if (block == null || entry == null)
                return;

            //If the given block is an HtmlBlock or any specialization of it,
            //then we index its body content
            if (typeof(HtmlBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as HtmlBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddContent(block.Id, text);
            }

            //If the given block is an TextBlock or any specialization of it,
            //then we index its body content
            if (typeof(TextBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as TextBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddContent(block.Id, text);
            }

            //If the given block is an QuoteBlock or any specialization of it,
            //then we index its body content
            if (typeof(QuoteBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as QuoteBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddContent(block.Id, text);
            }

            //If the given block is an ColumnBlock or any specialization of it,
            //then we index each block inside it
            if (typeof(ColumnBlock).IsAssignableFrom(block.GetType()))
            {
                var children = (block as ColumnBlock).Items;
                foreach (var child in children)
                    IndexBlock(child, entry);
            }
        }

        public void IndexPage(PageBase doc)
        {
            if (doc == null || !doc.IsPublished)
                return;

            SolrEntry entry = new SolrEntry()
            {
                Id = doc.Id,
                ObjectType = SolrEntry.eEntryType.Page,
                Permalink = string.IsNullOrWhiteSpace(doc.Permalink) ? null : doc.Permalink,
            };

            entry.Title.Add(doc.Title);

            if (!string.IsNullOrEmpty(doc.Excerpt))
                entry.AddContent(doc.Id, doc.Excerpt);

            //Indexing all content blocks
            foreach (var block in doc.Blocks)
                IndexBlock(block, entry);

            IndexInSolr(entry);
        }

        public void IndexPost(PostBase doc)
        {
            if (doc == null || !doc.IsPublished)
                return;

            SolrEntry entry = new SolrEntry()
            {
                Id = doc.Id,
                ObjectType = SolrEntry.eEntryType.Post,
                Permalink = string.IsNullOrWhiteSpace(doc.Permalink) ? null : doc.Permalink,
            };

            entry.Title.Add(doc.Title);

            if (!string.IsNullOrEmpty(doc.Excerpt))
                entry.AddContent(doc.Id, doc.Excerpt);

            //Indexing all content blocks
            foreach (var block in doc.Blocks)
                IndexBlock(block, entry);

            IndexInSolr(entry);
        }
        private void IndexInSolr(SolrEntry entry)
        {
             _solrIndexService.AddUpdate(entry);
        }
    }
}
