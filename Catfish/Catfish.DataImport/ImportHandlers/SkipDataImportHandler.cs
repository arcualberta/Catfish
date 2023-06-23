using Catfish.DataImport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.DataImport.ImportHandlers
{
    public class SkipDataImportHandler : IImportHnadler
    {
        private readonly Guid NameId = Guid.Parse("xxx-xxx-xxx");
        private readonly Guid EmailId = Guid.Parse("xxx-xxx-xxx");
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
