using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public class BackupService : IBackupService
    {
        private readonly AppDbContext _db;
        private readonly IAuthorizationService _authorizationService;
        public BackupService(AppDbContext db, IAuthorizationService authorizationService)
        {
            _db = db;
            _authorizationService = authorizationService;
        }
        public void Backup(EntityTemplate template)
        {
            var user = _authorizationService.GetLoggedUser();

            //Making a back-up of the template
            Backup backup = new Backup(template.Id,
                template.GetType().ToString(),
                template.Content,
                user.Id,
                user.UserName);

            _db.Backups.Add(backup);
        }
    }
}
