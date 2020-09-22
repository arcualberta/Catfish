using Catfish.Core.Models.Solr;
using Piranha;
using Piranha.Models;
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

            //TODO after completing the following. Check whether the page save
            //represents whether the save was related to just "saved" or "published".
            //We should not call the following indexing code if the page was not actually
            //published.

            //Create an index entry for the page tile
            if (!string.IsNullOrWhiteSpace(page.Title))
            {
                SolrPageContentModel content = new SolrPageContentModel()
                {
                    ContenType = SolrPageContentModel.eContentType.Page,
                    PageId = page.Id,
                    Title = page.Title,
                    ParentId = page.ParentId
                };

                //TODO: call the solr indexing service to index this entry.
            }

            //Add the page exerpt to the solr index


            //iterate through all blocks. If a block is of type ContentBlock, then
            //add the content of that block to the solr index.

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
