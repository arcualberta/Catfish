using Catfish.API.Repository.Models.Entities;
using Catfish.API.Repository.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.API.Repository.Tests.UnitTests
{
    public class SolrDocTests
    {
        [Fact]
        public void BuildSolrDoc()
        {
            //Load the contents of an entity and its template form example files and then
            //reconstruct the entity object manally for testing

            string entitDataFile = @"..\..\..\Data\test_entity.json";
            string entitTemplateFile = @"..\..\..\Data\test_entity_template.json";
            Assert.True(File.Exists(entitDataFile));
            Assert.True(File.Exists(entitTemplateFile));

            EntityData entityData = new EntityData();
            entityData.Title = "Test item title";
            entityData.Description = "Test item title";
            entityData.Created = new DateTime(2022, 11, 28);
            entityData.Updated = new DateTime(2022, 11, 29);
            entityData.Id = Guid.Parse("6b94c2cd-363a-4088-84dc-3a993ae6a055");
            entityData.TemplateId = Guid.Parse("142C3DB0-AA41-1F64-4F35-CABB636F2A66");
            entityData.SerializedData = File.ReadAllText(entitDataFile);

            EntityTemplate template = new EntityTemplate();
            template.Id = Guid.Parse("142C3DB0-AA41-1F64-4F35-CABB636F2A66");
            template.SerializedEntityTemplateSettings = File.ReadAllText(entitTemplateFile);
            template.Name = "Test Entity Template";

            entityData.Template = template;

            SolrDoc doc = new SolrDoc(entityData, true);

            int x = 10;


        }
    }
}
