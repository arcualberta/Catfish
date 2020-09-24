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

            //TODO after completing the following. Check whether the page save
            //represents whether the save was related to just "saved" or "published".
            //We should not call the following indexing code if the page was not actually
            //published.

            //Create an index entry for the page tile

            // test both title for page is there and if the page is published
            if (!string.IsNullOrWhiteSpace(page.Title) && page.IsPublished)
            {
                SolrPageContentModel pagecontent = new SolrPageContentModel()
                {
                    ContenType = SolrPageContentModel.eContentType.Page,
                    PageId = page.Id,
                    Title = page.Title,
                    ParentId = page.ParentId,
                    Excerpt = page.Excerpt
                };

                //TODO: call the solr indexing service to index this entry.
                IndexInSolr(pagecontent);

                // go through blocks and submit content for indexing
                // check for type of block to use  
                if (true)
                {
                    var  i = 0;
                    foreach (var blockpiece in page.Blocks)
                    {
                        string blocktype = blockpiece.Type;
                        var blockcontent = new SolrPageContentModel()
                        {
                            ContenType = SolrPageContentModel.eContentType.Block,
                            BlockId = blockpiece.Id,
                            Title = "block  " + i + " from " + page.Title,
                            ParentId = page.Id



                        };
                        //blockcontent.Content = page.Blocks[i].ToString();
                        //blockcontent.Content = page.Blocks[i].GetTitle();
                        blockcontent.Content = blockpiece.ToString();
                        IndexInSolr(blockcontent);
                        i++;
                    }
                }
             }
                    


                
                
         }



 

        public void IndexPost(PostBase post)
        {
            //Implement this method similar to the IndexPage method to add the post
            //title, exerpt and contents of all "Content" type blocks to the solr index.
        }

        private void IndexInSolr(SolrPageContentModel data)
        {
            Console.WriteLine(data.Content);

        }
    }
}
