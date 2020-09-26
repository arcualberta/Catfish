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
        private readonly ISolrIndexService<SolrPageContentModel> _solrIndexService;
        private readonly IQueryService _solrQueryService;
        public PageIndexingService(ISolrIndexService<SolrPageContentModel> iSrv, IQueryService qSrv, IApi api)
        {
            _api = api;
            _solrIndexService = iSrv;
            _solrQueryService = qSrv;
        }

        private IList<SolrPageContentModel> GetContents(Guid pageId)
        {
            throw new NotImplementedException();
        }

        public void IndexBlock(Block block, Guid ParentId, SolrPageContentModel model)
        {
            if (block == null)
                return;

            //If the given block is an HtmlBlock or any specialization of it,
            //then we index its body content
            if (typeof(HtmlBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as HtmlBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    model.AddBlockContent(block.Id, ParentId, text);
            }

            //If the given block is an TextBlock or any specialization of it,
            //then we index its body content
            if (typeof(TextBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as TextBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    model.AddBlockContent(block.Id, ParentId, text);
            }

            //If the given block is an QuoteBlock or any specialization of it,
            //then we index its body content
            if (typeof(QuoteBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as QuoteBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    model.AddBlockContent(block.Id, ParentId, text);
            }

            //If the given block is an ColumnBlock or any specialization of it,
            //then we index each block inside it
            if (typeof(ColumnBlock).IsAssignableFrom(block.GetType()))
            {
                var children = (block as ColumnBlock).Items;
                foreach (var child in children)
                    IndexBlock(child, block.Id, model);
            }
        }

        public void IndexPage(PageBase page)
        {
            if (page == null || !page.IsPublished)
                return;

            SolrPageContentModel model = new SolrPageContentModel()
            {
                Id = page.Id
            };
            model.ContenType.Add(SolrEntry.eEntryType.Page);
            model.ParentId.Add(page.ParentId);

            //Indexing the page title
            if (!string.IsNullOrWhiteSpace(page.Title))
                model.Title.Add(page.Title);

            //Indexing the page excerpt
            if (!string.IsNullOrWhiteSpace(page.Excerpt))
                model.Excerpt.Add(page.Excerpt);

            //Indexing the page permalink
            if (!string.IsNullOrWhiteSpace(page.Permalink))
                model.Excerpt.Add(page.Permalink);

            //Indexing all content blocks
            foreach (var block in page.Blocks)
                IndexBlock(block, page.Id, model);

            IndexInSolr(model);
        }

        public void IndexPost(PostBase post)
        {
            if (post == null || !post.IsPublished)
                return;

            SolrPageContentModel model = new SolrPageContentModel()
            {
                Id = post.Id
            };
            model.ContenType.Add(SolrEntry.eEntryType.Post);
            model.ParentId.Add(post.BlogId);

            //Indexing the page title
            if (!string.IsNullOrWhiteSpace(post.Title))
                model.Title.Add(post.Title);

            //Indexing the page excerpt
            if (!string.IsNullOrWhiteSpace(post.Excerpt))
                model.Excerpt.Add(post.Excerpt);

            //Indexing the page permalink
            if (!string.IsNullOrWhiteSpace(post.Permalink))
                model.Excerpt.Add(post.Permalink);

            //Indexing all content blocks
            foreach (var block in post.Blocks)
                IndexBlock(block, post.Id, model);

            IndexInSolr(model);
        }

        private void IndexInSolr(SolrPageContentModel model)
        {
             _solrIndexService.AddUpdate(model);
        }
    }
}
