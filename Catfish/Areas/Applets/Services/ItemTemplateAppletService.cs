using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Catfish.Areas.Applets.Services
{
    public class ItemTemplateAppletService : IItemTemplateAppletService
    {
        public readonly AppDbContext _appDb;
        private readonly ErrorLog _errorLog;
        public ItemTemplateAppletService(AppDbContext db, ErrorLog errorLog)
        {
            _appDb = db;
            _errorLog = errorLog;
        }

        public ItemTemplate GetItemTemplate(Guid id, ClaimsPrincipal user)
        {
            ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == id);

            //TODO #1: If the "user" does not have permission to edit the template, throw a Catfish.Core.Exceptions.AuthorizationException

            return template;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">EntityTemplate Id</param>
        /// <returns></returns>
        public List<Group> GetTemplateGroups(Guid? id)
        {
            return _appDb.GroupTemplates.Where(g => g.EntityTemplateId == id.Value).Select(g=>g.Group).ToList();
        }
       
        public DataItem GetDataItem(Guid itemTemplate, Guid ChildFormId)
        {
            ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == itemTemplate);
            DataItem dataItem = template.GetDataItem(ChildFormId);

            return dataItem;

        }

        public List<DataItem> GetDataItems(Guid itemTemplate, bool isRoot = false)
        {
            ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == itemTemplate);
            var dataItems = template.DataContainer.Where(di => di.IsRoot == isRoot).ToList();
            return dataItems;
        }
        public List<DataItem> GetAllDataItems(Guid itemTemplate)
        {
            ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == itemTemplate);
            var dataItems = template.DataContainer.ToList();
            return dataItems;
        }


        /// <summary>
        /// This method will return all collectios related to a group template.
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ICollection<Collection> GetAllGroupTemplateCollections(Guid templateId, Guid groupId)
        {
            try
            {
                var collections = _appDb.GroupTemplates
                                    .Where(gt => gt.EntityTemplateId == templateId && gt.GroupId == groupId)
                                    .Select(gt => gt.Collections)
                                    .FirstOrDefault();
                return collections;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return new List<Collection>();
            }
            
        }

    }
}
