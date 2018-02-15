using Catfish.Core.Models;
using Catfish.Core.Models.Ingestion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Services
{
    public class IngestionService : ServiceBase
    {
        public IngestionService(CatfishDbContext db) : base(db)
        {

        }

        public IngestionService Parse(XElement ingestion)
        {
            throw new NotImplementedException();
        }

        public void Import(Ingestion ingestion)
        {
            throw new NotImplementedException();
        }

        public void Import(XElement ingestion)
        {
            Ingestion input = new Ingestion();
            input.Deserialize(ingestion);

            Import(input);
        }

        public void Import(Stream ingestion)
        {
            XElement result = XElement.Load(ingestion);
            Import(result);
        }

        public Ingestion Export()
        {
            throw new NotImplementedException();
        }

        public void Export(Stream stream, Ingestion ingestion = null)
        {
            if (ingestion == null)
            {
                ingestion = Export();
            }

            XElement element = ingestion.Serialize();
            element.Save(stream);
        }
    }
}
