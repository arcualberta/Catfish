using Catfish.Core.Contexts;
using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Helpers
{
    public abstract class BaseUnitTest
    {
        [SetUp]
        public virtual void SetUp()
        {
            OnSetup();
        }

        [TearDown]
        public virtual void TearDown()
        {
            OnTearDown();
        }

        protected abstract void OnSetup();

        protected abstract void OnTearDown();

        protected CFMetadataSet CreateMetadataSet(DatabaseHelper dh, string name, string description, IEnumerable<FormField> fields)
        {
            CFMetadataSet metadata = new CFMetadataSet();
            metadata.Name = name;
            metadata.Description = description;

            metadata.Fields = new List<FormField>(fields);

            metadata.Serialize();

            return dh.Ms.UpdateMetadataSet(metadata);
        }

        protected CFEntityType CreateEntityType(DatabaseHelper dh, string name, string description, string targetTypes, params CFMetadataSet[] metadataSets)
        {
            CFEntityType type = new CFEntityType()
            {
                Name = name,
                Description = description,
                MetadataSets = metadataSets,
                TargetTypes = targetTypes
            };

            return dh.Ets.CreateEntityType(type);
        }
    }    
}
