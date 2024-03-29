﻿using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Entities;
using Catfish.API.Repository.Models.Forms;
using Catfish.API.Repository.Services;
using Catfish.API.Repository.Solr;
using Catfish.API.Repository.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.API.Repository.Tests.UnitTests
{
    public class SolrDocTests
    {
        public readonly TestHelper _testHelper;

        public SolrDocTests()
        {
            _testHelper = new TestHelper();
        }

        [Fact]
        public void BuildSolrDoc()
        {
            //Load the contents of an entity and its template form example files and then
            //reconstruct the entity object manally for testing

            ISolrService solr = _testHelper.Solr;

            string entitDataFile = @"..\..\..\Data\test_entity.json";
            string entitTemplateFile = @"..\..\..\Data\test_entity_template.json";
            string dataForm = @"..\..\..\Data\form_5f792ac1-3284-1fa7-704b-437ff8ca8540.json";
            string metadataForm = @"..\..\..\Data\form_1492520d-a082-272e-ade0-a9f7513d678f.json";
            
            Assert.True(File.Exists(entitDataFile));
            Assert.True(File.Exists(entitTemplateFile));
            Assert.True(File.Exists(dataForm));
            Assert.True(File.Exists(metadataForm));

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

            List<FormTemplate> forms = new List<FormTemplate>();

            FormTemplate form_01 = new FormTemplate();
            form_01.Id = Guid.Parse("1492520d-a082-272e-ade0-a9f7513d678f");
            form_01.Name = "Test metadata form";
            form_01.Status = 0;
            form_01.SerializedFields = File.ReadAllText(metadataForm);
            form_01.Created = new DateTime(2022, 11, 28);
            form_01.Updated = DateTime.Now;
            forms.Add(form_01);

            FormTemplate form_02 = new FormTemplate();
            form_02.Id = Guid.Parse("5f792ac1-3284-1fa7-704b-437ff8ca8540");
            form_02.Name = "Test data form";
            form_02.Status = 0;
            form_02.SerializedFields = File.ReadAllText(dataForm);
            form_02.Created = new DateTime(2022, 11, 26);
            form_02.Updated = DateTime.Now;
            forms.Add(form_02);
            SolrDoc doc = new SolrDoc(entityData, forms, true);
            List<SolrDoc> docs= new List<SolrDoc>();
            docs.Add(doc);
            solr.Index(docs);
            int x = 10;


        }
        [Fact]
        public void TestIndexingUsingEntity()
        {
            //Load the contents of an entity and its template form example files and then
            //reconstruct the entity object manally for testing

            ISolrService solr = _testHelper.Solr;

            string entitDataFile = @"..\..\..\Data\test_entity2.json";
            string entitTemplateFile = @"..\..\..\Data\test_entity2_template.json";
            string dataForm = @"..\..\..\Data\form_95FC71A0-3363-FDEE-9802-E0CABC98607A.json";
            string metadataForm = @"..\..\..\Data\form_861FBA83-C3AB-62AA-2177-2D579AC5BD02.json";

            Assert.True(File.Exists(entitDataFile));
            Assert.True(File.Exists(entitTemplateFile));
            Assert.True(File.Exists(dataForm));
            Assert.True(File.Exists(metadataForm));

            EntityData entityData = new EntityData();
            entityData.Title = "Test entity item title";
            entityData.Description = "Test entity item title";
            entityData.Created = new DateTime(2022, 11, 28);
            entityData.Updated = DateTime.Now;
            entityData.Id = Guid.Parse("D416139D-744D-3867-9815-132E82B37BE3");
            entityData.TemplateId = Guid.Parse("C465AA8E-3D3D-848A-CF16-D96491EC2CC7");
            entityData.SerializedData = File.ReadAllText(entitDataFile);

            EntityTemplate template = new EntityTemplate();
            template.Id = Guid.Parse("C465AA8E-3D3D-848A-CF16-D96491EC2CC7");
            template.SerializedEntityTemplateSettings = File.ReadAllText(entitTemplateFile);
            template.Name = "Test New Entity Template";

            entityData.Template = template;

            List<FormTemplate> forms = new List<FormTemplate>();

            FormTemplate form_01 = new FormTemplate();
            form_01.Id = Guid.Parse("861FBA83-C3AB-62AA-2177-2D579AC5BD02");
            form_01.Name = "Test new metadata form";
            form_01.Status = 0;
            form_01.SerializedFields = File.ReadAllText(metadataForm);
            form_01.Created = new DateTime(2022, 11, 28);
            form_01.Updated = DateTime.Now;
            forms.Add(form_01);

            FormTemplate form_02 = new FormTemplate();
            form_02.Id = Guid.Parse("95FC71A0-3363-FDEE-9802-E0CABC98607A");
            form_02.Name = "Test new data form";
            form_02.Status = 0;
            form_02.SerializedFields = File.ReadAllText(dataForm);
            form_02.Created = new DateTime(2022, 11, 26);
            form_02.Updated = DateTime.Now;
            forms.Add(form_02);
            solr.Index(entityData, forms);
            int x = 10;


        }
    }
}
