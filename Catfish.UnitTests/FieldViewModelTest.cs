﻿using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;

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



            //Test re-instantiating data fields in the form using the view models


        }

    }
}