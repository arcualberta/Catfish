using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class EntityTemplateService : IEntityTemplateService
    {
        public EntityTemplate GetAvailableTemplates()
        {
            EntityTemplate availableTemplates = new EntityTemplate();
            return availableTemplates;
        }
    }
}
