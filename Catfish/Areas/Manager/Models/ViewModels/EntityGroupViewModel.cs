using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityGroupViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Entity Group Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string EntityGroupName { get; set; }

        public string userName { get; set; } /*user's loginName to be added -- used on edit view */

        
        public List<string> SelectedUsers { get; set; }
       
        public List<string> AllUsers { get; set; }
       
        public List<string> UsersToRemove { get; set; }

        public EntityGroupViewModel()
        {
            SelectedUsers = new List<string>();
            AllUsers = new List<string>();// List<Piranha.Entities.User>();
            UsersToRemove = new List<string>();// List<Piranha.Entities.User>();            
        }

        public void AddUser()
        {
            if(!string.IsNullOrEmpty(userName) && !SelectedUsers.Contains(userName))//SelectedUsers.Where(u=>u.Login == userName).Count() <= 0)
            {
                if(AllUsers.Contains(userName)) //make sure if the name is legit -- un our system
                     SelectedUsers.Add(userName);
                userName = "";
            }
        }
        /// <summary>
        /// removed selected user 
        /// </summary>
        public void RemoveSelected()
        {
            SelectedUsers.RemoveAll(u => UsersToRemove.Contains(u));//(u => UsersToRemove.Any(y=>y.Id == u.Id));
            UsersToRemove.Clear();
        }
       
        
       
    }
}