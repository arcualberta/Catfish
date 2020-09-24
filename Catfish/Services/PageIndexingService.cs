using Catfish.Core.Models.Solr;
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
        public PageIndexingService(IApi api)
        {
            _api = api;
        }

        private IList<SolrPageContentModel> GetContents(Guid pageId)
        {
            throw new NotImplementedException();
        }

        public void IndexPage(PageBase page)
        {
            if (page == null || !page.IsPublished)
                return;

            //Indexing the page title
            if (!string.IsNullOrWhiteSpace(page.Title))
            {
                IndexInSolr(new SolrPageContentModel()
                {
                    ContenType = SolrPageContentModel.eContentType.Page,
                    PageId = page.Id,
                    Title = page.Title,
                    ParentId = page.ParentId
                });
            }

            //Indexing the page excerpt
            if (!string.IsNullOrWhiteSpace(page.Excerpt))
            {
                IndexInSolr(new SolrPageContentModel()
                {
                    ContenType = SolrPageContentModel.eContentType.Page,
                    PageId = page.Id,
                    Excerpt = page.Excerpt,
                    ParentId = page.ParentId
                });
            }

            //Indexing all content blocks
            foreach (var block in page.Blocks)
                IndexBlock(block, page.Id);
        }

        public void IndexBlock(Block block, Guid ParentId)
        {
            if (block == null)
                return;

            //If the given block is an HtmlBlock or any specialization of it,
            //then we index its body content
            if (typeof(HtmlBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as HtmlBlock).Body.Value;
                IndexInSolr(new SolrPageContentModel()
                {
                    ContenType = SolrPageContentModel.eContentType.Block,
                    BlockId = block.Id,
                    ParentId = ParentId,
                    Content = text
                });
            }

            //If the given block is an TextBlock or any specialization of it,
            //then we index its body content
            if (typeof(TextBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as TextBlock).Body.Value;
                IndexInSolr(new SolrPageContentModel()
                {
                    ContenType = SolrPageContentModel.eContentType.Block,
                    BlockId = block.Id,
                    ParentId = ParentId,
                    Content = text
                });
            }
            //If the given block is an ColumnBlock or any specialization of it,
            //then we index each block inside it

            if (typeof(ColumnBlock).IsAssignableFrom(block.GetType()))
            {
                var children = (block as ColumnBlock).Items;
                foreach (var child in children)
                    IndexBlock(child, block.Id);
            }

        }






        public void IndexPost(PostBase post)
        {
            //Implement this method similar to the IndexPage method to add the post
            //title, exerpt and contents of all "Content" type blocks to the solr index.
        }

        private void IndexInSolr(SolrPageContentModel data)
        {
            

        }
    }
}
