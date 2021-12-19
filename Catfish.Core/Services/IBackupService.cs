using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public interface IBackupService
    {
        public void Backup(EntityTemplate template);
    }
}
