﻿using Catfish.Core.Models;
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
            var dataItems = template.GetAllNonRootDataItems();
            return dataItems;
        }
    }
}
