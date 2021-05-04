using Catfish.Core.Models;
using Catfish.Core.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Fields.ViewModels;
using Newtonsoft.Json;
using Catfish.Test.Helpers;

namespace Catfish.UnitTests
{
    class FieldViewModelTest
    {
        private AppDbContext _db;
        private TestHelper _testHelper;
        private IAuthorizationService _auth;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
            IAuthorizationService _auth = _testHelper.AuthorizationService;
        }

        public DataItem CreateSampleForm()
        {
            string lang = "en";
            DataItem form = new DataItem();
            form.CreateField<TextField>("First Name", lang, true);
            form.CreateField<TextField>("LastName Name", lang, true);
            form.CreateField<TextArea>("Address", lang, true);

            return form;
        }

        [Test]
        public void ViewModelTest1()
        {
            //Pick the first item from the database
            DataItem form = CreateSampleForm();

            //Test creating a list view models for all fields in the form
            Assert.IsNotNull(form);

            var tmpList = new List<FieldVM>();
            foreach(TextField f in form.Fields)
            {
                tmpList.Add(new FieldVM(f));
            }

            var json = JsonConvert.SerializeObject(tmpList);
            //Test re-instantiating data fields in the form using the view models
            foreach(FieldVM f in tmpList)
            {
                //value groups/ids? i dont understand what this test is meaning
                //f.UpdateTextValues(f);
            }
        }
    }
}
