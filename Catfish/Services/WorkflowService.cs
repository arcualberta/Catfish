using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Models;
using Catfish.Models.Blocks;
using Catfish.Models.SiteTypes;
using Piranha.AspNetCore.Services;
using Piranha.Models;
using Piranha.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class WorkflowService : IWorkflowService
    {
        public static readonly string DefaultLanguage = "en";

        private AppDbContext _db { get; set; }
        private ISiteService _siteService { get; set; }
        private IPageService _pageService { get; set; }

        private EntityTemplate mEntityTemplate;

        private Item mItem;

        public WorkflowService(AppDbContext db, ISiteService siteService, IPageService pageService)
        {
            _db = db;
            _siteService = siteService;
            _pageService = pageService;
        }

        public EntityTemplate GetModel()
        {
            return mEntityTemplate;
        }
        
        public EntityTemplate GetTemplate()
        {
            return mEntityTemplate;
        }

        public void SetModel(EntityTemplate entityTemplate)
        {
            mEntityTemplate = entityTemplate;
        }

        public void SetModel(Item item)
        {
            mItem = item;
            mEntityTemplate = _db.EntityTemplates.Where(et => et.Id == item.TemplateId).FirstOrDefault();
        }

        public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists)
        {
            MetadataSet ms = GetMetadataSet(templateName, createIfNotExists, true);
            return ms == null ? null : new EmailTemplate(ms.Data);
        }

        protected MetadataSet GetMetadataSet(string metadataSetName, bool createIfNotExists, bool markAsTemplateMetadataSetIfCreated)
        {
            MetadataSet ms = mEntityTemplate.MetadataSets
                .Where(ms => ms.GetName(DefaultLanguage) == metadataSetName)
                .FirstOrDefault();

            if(ms == null && createIfNotExists)
            {
                ms = new MetadataSet();
                ms.SetName(metadataSetName, DefaultLanguage);
                mEntityTemplate.MetadataSets.Add(ms);
                if (markAsTemplateMetadataSetIfCreated)
                    ms.IsTemplate = true;
            }
            return ms;
        }

        //public DataItem GetDataItem(string dataItemName, bool createIfNotExists)
        //{
        //    DataItem dataItem = mEntityTemplate.DataContainer
        //        .Where(di => di.GetName(DefaultLanguage) == dataItemName)
        //        .FirstOrDefault();

        //    if (dataItem == null && createIfNotExists)
        //    {
        //        dataItem = new DataItem();
        //        dataItem.SetName(dataItemName, DefaultLanguage);
        //        mEntityTemplate.DataContainer.Add(dataItem);
        //    }
        //    return dataItem;
        //}

        public Workflow GetWorkflow(bool createIfNotExists)
        {
            XmlModel xml = new XmlModel(mEntityTemplate.Data);
            XElement element = xml.GetElement(Workflow.TagName, createIfNotExists);
            Workflow workflow = new Workflow(element);
            return workflow;
        }

        public List<string> GetEmailAddresses(EmailTrigger trigger)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method goes through all entity templates in the database and select all of the roles
        /// in these templates. The method should return a unique list of roles.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserRoles()
        {
            throw new NotImplementedException();
        }

        public async Task InitSiteStructureAsync(Guid siteId, string siteTypeId)
        {
            if(siteTypeId == typeof(WorkflowPortal).Name)
            {
                var site = await _siteService.GetByIdAsync(siteId).ConfigureAwait(false);

                var siteContent = await _siteService.GetContentByIdAsync(siteId).ConfigureAwait(false);
                var workflowPageSettings = siteContent.Regions.WorkflowPagesContent;
                if(workflowPageSettings.CreateSubmissionEntryPage == true)
                {
                    //Making sure the submission-entry page exists
                    Guid? submissionEntryPageId = GetSystemPageId(siteId, "SubmissionEntryPage", true,
                        string.IsNullOrEmpty(workflowPageSettings.SubmissionEntryPage) ? "Start a Submission" : workflowPageSettings.SubmissionEntryPage);

                    //Making sure this page has at least one block of SubmissionEntryList
                    var page = await _pageService.GetByIdAsync(submissionEntryPageId.Value).ConfigureAwait(false);
                    if(page.Blocks.Where(b => typeof(SubmissionEntryList).IsAssignableFrom(Type.GetType(b.Type))).Any() == false)
                    {
                        page.Blocks.Add(new SubmissionEntryList());
                        await _pageService.SaveAsync<DynamicPage>(page).ConfigureAwait(false);
                    }

                    ////SystemPage pageInfo = _db.SystemPages
                    ////    .Where(pg => pg.PageKey == "SubmissionEntryPage" && pg.SiteId == siteId)
                    ////    .FirstOrDefault();

                    ////bool createPage = false;
                    ////if (pageInfo == null)
                    ////{
                    ////    pageInfo = new SystemPage() { PageKey = "SubmissionEntryPage", SiteId = siteId };
                    ////    _db.SystemPages.Add(pageInfo);
                    ////    createPage = true;
                    ////}
                    ////else
                    ////{
                    ////    var page = await _pageService.GetByIdAsync(pageInfo.PageId).ConfigureAwait(false);
                    ////    if (page == null)
                    ////        createPage = true;
                    ////}

                    ////if (createPage)
                    ////{
                    ////    var page = await _pageService.CreateAsync<StandardPage>().ConfigureAwait(false);
                    ////    page.SiteId = siteId;
                    ////    page.Published = DateTime.Now;
                    ////    page.Title = workflowPageSettings.SubmissionEntryPage;
                    ////    if (string.IsNullOrEmpty(page.Title))
                    ////        page.Title = "Start a Submission";
                    ////    await _pageService.SaveAsync<StandardPage>(page).ConfigureAwait(false);
                    ////    pageInfo.PageId = page.Id;
                    ////    _db.SaveChanges();
                    ////}
                }
            }
        }

        protected Guid? GetSystemPageId(Guid siteId, string pageKey, bool createIfNotExist, string pageTitleIfShouldCreate)
        {
            SystemPage pageInfo = _db.SystemPages
                .Where(pg => pg.PageKey == "SubmissionEntryPage" && pg.SiteId == siteId)
                .FirstOrDefault();

            if (createIfNotExist == false)
            {
                if (pageInfo == null)
                    return null;
                else
                {
                    var t = _pageService.GetByIdAsync(pageInfo.PageId);
                    t.Wait();
                    return t.Result.Id;
                }
            }
            else
            {
                if (pageInfo == null)
                    pageInfo = new SystemPage() { PageKey = pageKey, SiteId = siteId };
                else
                {
                    var t = _pageService.GetByIdAsync(pageInfo.PageId);
                    t.Wait();
                    var loadedPage = t.Result;

                    if (loadedPage != null)
                        return loadedPage.Id;
                }

                //If the execution comes here, then we need to create a new page and update the page info entry
                var task = _pageService.CreateAsync<StandardPage>();
                task.Wait();

                StandardPage newPage = task.Result;
                newPage.SiteId = siteId;
                newPage.Published = DateTime.Now;
                newPage.Title = pageTitleIfShouldCreate;

                Task savePageTask = _pageService.SaveAsync<StandardPage>(newPage);
                savePageTask.Wait();

                pageInfo.PageId = newPage.Id;
                _db.SaveChanges();

                return newPage.Id;
            }
        }
    }
}
