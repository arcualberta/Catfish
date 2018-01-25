using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityGroupViewModel //: KoBaseViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Entity Group Name")]
        public string EntityGroupName { get; set; }

        public string userName { get; set; } /*user to be added -- used on edit view */

        public List<Piranha.Entities.User> SelectedUsers { get; set; }

        public List<Piranha.Entities.User> AllUsers{ get; set; }

        public List<Piranha.Entities.User> UsersToRemove { get; set; }

        public EntityGroupViewModel()
        {
            SelectedUsers = new List<Piranha.Entities.User>();
            AllUsers = new List<Piranha.Entities.User>();
        }

        public void AddUser()
        {
            if(!string.IsNullOrEmpty(userName) && SelectedUsers.Where(u=>u.Login == userName).Count() <= 0)
            {
                Piranha.Entities.User user = AllUsers.Where(u => u.Login == userName).SingleOrDefault();
                SelectedUsers.Add(user);
                userName = "";
            }
        }
        /// <summary>
        /// removed selected user 
        /// </summary>
        public void RemoveSelected()
        {
            SelectedUsers.RemoveAll(u => UsersToRemove.Any(y=>y.Id == u.Id));
            UsersToRemove.Clear();
        }
       
        //public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        //{
        //    throw new NotImplementedException();
        //}

    }
}