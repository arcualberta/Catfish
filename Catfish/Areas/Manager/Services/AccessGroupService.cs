using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Services;
using Catfish.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Services
{
    public class AccessGroupService : ServiceBase
    {
        public AccessGroupService(CatfishDbContext db) : base(db) { }

        
        public EntityAccessDefinitionsViewModel UpdateViewModel(CFEntity entity)
        {
            UserService us = new UserService();
            UserListService uListService = new UserListService(Db);

            EntityAccessDefinitionsViewModel entityAccessVM = new EntityAccessDefinitionsViewModel();
            entityAccessVM.Id = entity.Id;
            entityAccessVM.EntityName = entity.Name;
            entityAccessVM.BlockInheritance = entity.BlockInheritance;

            entityAccessVM.AvailableUsers2 = us.GetUserIdAndLoginName();
            Dictionary<string, string> allUserLists = uListService.GetDictionaryUserLists();

            allUserLists.ToList().ForEach(x => entityAccessVM.AvailableUsers2.Add(x.Key, x.Value));

            AccessDefinitionService accessDefinitionService = new AccessDefinitionService(Db);
            SelectList accessDefs = new SelectList(accessDefinitionService.GetSelectListAccessDefinitions()
                .GroupBy(a => a.Name)
                .Select(a => a.FirstOrDefault())
                .Select(i => new SelectListItem() {
                    Value = ((int)i.AccessModes).ToString(),
                    Text = i.StringAccessModesList }), "Value", "Text");
            entityAccessVM.AvailableAccessDefinitions = accessDefs;

            entityAccessVM.AvailableAccessDefinitions2 = accessDefs.ToList();

            if (entity.AccessGroups.Count > 0)
            {

                //update SelectedAccessGroups
                foreach (CFAccessGroup gr in entity.AccessGroups)
                {
                    AccessGroup accGrp = new Models.ViewModels.AccessGroup();
                    accGrp.userId = gr.AccessGuid.ToString(); //FirstOrDefault().ToString();
                    var user = us.GetUserById(accGrp.userId);
                    string name = string.Empty;
                    if (user == null)
                    {
                        name = uListService.GetEntityGroup(accGrp.userId).Name;
                    }
                    else
                    {
                        name = user.Login;
                    }
                    accGrp.User = name;
                    accGrp.AccessMode = gr.AccessDefinition.StringAccessModesList;
                    accGrp.AccessModesNum = (int)gr.AccessDefinition.AccessModes;

                    entityAccessVM.SelectedAccessGroups.Add(accGrp);
                }
            }

            
            return entityAccessVM;

        }

        public CFEntity UpdateEntityAccessGroups(CFEntity entity, EntityAccessDefinitionsViewModel entityAccessVM)
        {
            // XXX Here is where you need to update the access group on solr
            List<CFAccessGroup> accessGroups = new List<CFAccessGroup>();
            foreach (var ag in entityAccessVM.SelectedAccessGroups)
            {
                CFAccessGroup group = new CFAccessGroup();
                //group.AccessGuids = new List<Guid>() { Guid.Parse(ag.userId) };
                group.AccessGuid = Guid.Parse(ag.userId);
                group.AccessDefinition.AccessModes = (AccessMode)ag.AccessModesNum;
                if(ag.AccessMode != null)
                {
                    group.AccessDefinition.Name = ag.AccessMode.Substring(0, ag.AccessMode.LastIndexOf("-"));
                }
                accessGroups.Add(group);
            }

            entity.AccessGroups = accessGroups;           
            entity.BlockInheritance = entityAccessVM.BlockInheritance;

            //entity.MappedGuid
            // fetch solr entry by cfentity mapped guid
            SolrNet.SolrQueryResults<Dictionary<string, object>> mapedEntity;
            mapedEntity = SolrService.solrOperations.Query("id:" + entity.MappedGuid);

            mapedEntity.First();
            // remove all access entries

            return entity;

        }

        public List<SelectListItem> GetAccessCodesList()
        {
            List<SelectListItem> accessCodesList = new List<SelectListItem>();
            
            foreach (AccessMode am in Enum.GetValues(typeof(AccessMode)))
            {

                accessCodesList.Add(new SelectListItem { Text = am.ToString(), Value = ((int)am).ToString() });

            }
            return accessCodesList;
        }

    }
}