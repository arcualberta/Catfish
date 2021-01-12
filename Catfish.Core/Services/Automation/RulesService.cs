using Catfish.Core.Models;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Automation
{
    public class RulesService : DbEntityService
    {
        public RulesService(AppDbContext db, ErrorLog errorLog)
        : base(db, errorLog)
        {

        }

        public bool ProcessItem(Guid id)
        {
            try
            {

                return true;
            }
            catch(Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }
    }
}
