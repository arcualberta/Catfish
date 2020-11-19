using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Models;
using Catfish.Models.Blocks;
using Catfish.Models.SiteTypes;
using ElmahCore;
using Piranha;
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
        private readonly ErrorLog _errorLog;
        private AppDbContext _db { get; set; }
        private IApi _api;

        private EntityTemplate mEntityTemplate;

        private Item mItem;

        public WorkflowService(AppDbContext db, IApi api, ErrorLog errorLog)
        {
            _db = db;
            _api = api;
            _errorLog = errorLog;
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
            try
            {
                mItem = item;
                mEntityTemplate = _db.EntityTemplates.Where(et => et.Id == item.TemplateId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }

        }

        public EmailTemplate GetEmailTemplate(string templateName, bool createIfNotExists)
        {
            try
            {
                MetadataSet ms = GetMetadataSet(templateName, createIfNotExists, true);
                return ms == null ? null : new EmailTemplate(ms.Data);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        protected MetadataSet GetMetadataSet(string metadataSetName, bool createIfNotExists, bool markAsTemplateMetadataSetIfCreated)
        {
            try
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
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
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
            try
            {
                XmlModel xml = new XmlModel(mEntityTemplate.Data);
                XElement element = xml.GetElement(Workflow.TagName, createIfNotExists);
                Workflow workflow = new Workflow(element);
                return workflow;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
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
            try 
            {
                if(siteTypeId == typeof(WorkflowPortal).Name)
                {
                    var site = await _api.Sites.GetByIdAsync(siteId).ConfigureAwait(false);

                    var siteContent = await _api.Sites.GetContentByIdAsync(siteId).ConfigureAwait(false);
                    var workflowPageSettings = siteContent.Regions.WorkflowPagesContent;
                    if (workflowPageSettings.CreateSubmissionEntryPage == true)
                    {
                        try
                        {
                            //Making sure the submission-entry page exists
                            Guid? submissionEntryPageId = GetSystemPageId(siteId, "SubmissionEntryPage", true,
                            string.IsNullOrEmpty(workflowPageSettings.SubmissionEntryPage) ? "Start a Submission" : workflowPageSettings.SubmissionEntryPage);

                            //Making sure this page has at least one block of SubmissionEntryList
                            var page = await _api.Pages.GetByIdAsync(submissionEntryPageId.Value).ConfigureAwait(false);
                            if (page.Blocks.Where(b => typeof(SubmissionEntryPointList).IsAssignableFrom(Type.GetType(b.Type))).Any() == false)
                            {
                                page.Blocks.Add(new SubmissionEntryPointList());
                                await _api.Pages.SaveAsync<DynamicPage>(page).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        protected Guid? GetSystemPageId(Guid siteId, string pageKey, bool createIfNotExist, string pageTitleIfShouldCreate)
        {
            try
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
                        try
                        {
                            var t = _api.Pages.GetByIdAsync(pageInfo.PageId);
                            t.Wait();
                            return t.Result.Id;
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                            return null;
                        }

                    }
                }
                else
                {
                    if (pageInfo == null)
                        pageInfo = new SystemPage() { PageKey = pageKey, SiteId = siteId };
                    else
                    {
                        try
                        {
                            var t = _api.Pages.GetByIdAsync(pageInfo.PageId);
                            t.Wait();
                            var loadedPage = t.Result;

                            if (loadedPage != null)
                                return loadedPage.Id;
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                            return null;
                        }
                    }

                    //If the execution comes here, then we need to create a new page and update the page info entry
                    var task = _api.Pages.CreateAsync<StandardPage>();
                    task.Wait();

                    StandardPage newPage = task.Result;
                    newPage.SiteId = siteId;
                    newPage.Published = DateTime.Now;
                    newPage.Title = pageTitleIfShouldCreate;

                    Task savePageTask = _api.Pages.SaveAsync<StandardPage>(newPage);
                    savePageTask.Wait();

                    pageInfo.PageId = newPage.Id;
                    _db.SaveChanges();

                    return newPage.Id;
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public string GetStatus(Guid entityTemplateId, string status, bool createIfNotExist)
        {
            try
            {
                SystemStatus systemStatus = _db.SystemStatuses.Where(ss => ss.NormalizedStatus == status.ToUpper() && ss.EntityTemplateId == entityTemplateId).FirstOrDefault();
                if (systemStatus == null && createIfNotExist)
                {
                    systemStatus = new SystemStatus() { Status = status, NormalizedStatus = status.ToUpper(), Id = Guid.NewGuid(), EntityTemplateId = entityTemplateId };
                    _db.SystemStatuses.Add(systemStatus);
                    _db.SaveChanges();
                }
                return systemStatus.Status;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return "";
            }

        }
        
        /// <summary>
        /// This method returns list of post actions which are belongs to the given function and group.
        /// </summary>
        /// <param name="entityTemplate"></param>
        /// <param name="function"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<PostAction> GetPostActions(EntityTemplate entityTemplate, string function, string group)
        {
            try
            {
                SetModel(entityTemplate);
                var workflow = GetWorkflow(false);
                if (workflow != null)
                {
                    var getAction = workflow.Actions.Where(ac => ac.Function == function && ac.Group == group).FirstOrDefault();
                    return getAction.PostActions.ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

    }
}
