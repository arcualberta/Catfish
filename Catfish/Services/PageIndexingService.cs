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
        protected ISolrIndexService<SolrPageContentModel> _solrIndexService;
        public PageIndexingService(ISolrIndexService<SolrPageContentModel> srv, IApi api)
        {
            _api = api;
            _solrIndexService = srv;
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
                Id = page.Id,
                ParentId = page.ParentId,
                ContenType = SolrPageContentModel.eContentType.Page
            };

            //Indexing the page title
            if (!string.IsNullOrWhiteSpace(page.Title))
                model.Title = page.Title;

            //Indexing the page excerpt
            if (!string.IsNullOrWhiteSpace(page.Excerpt))
                model.Excerpt = page.Excerpt;

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
                Id = post.Id,
                ParentId = post.BlogId,
                ContenType = SolrPageContentModel.eContentType.Post
            };

            //Indexing the page title
            if (!string.IsNullOrWhiteSpace(post.Title))
                model.Title = post.Title;

            //Indexing the page excerpt
            if (!string.IsNullOrWhiteSpace(post.Excerpt))
                model.Excerpt = post.Excerpt;

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
