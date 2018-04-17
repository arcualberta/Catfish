using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestClass]
    public class SubmissionServiceTest
    {
        private Form CreateFormTemplate(SubmissionService subSrv, string name, string description, string questionPrefix)
        {
            FormField field;
            Form f = new Form();

            f.Name = name;
            f.Description = description;

            List<FormField> fields = new List<FormField>();

            field = new TextField()
            {
                Name = questionPrefix + " short text",
                Description = "This is a short text field"
            };
            fields.Add(field);

            field = new RadioButtonSet()
            {
                Name = questionPrefix + " radio button set",
                Description = "This is a radio button field",
                Options = new List<Option>
                {
                    new Option(){ Guid = Guid.NewGuid().ToString(), Value = new List<TextValue> (){ new TextValue("en", "One", "One") } },
                    new Option(){ Guid = Guid.NewGuid().ToString(), Value = new List<TextValue> (){ new TextValue("en", "Two", "Two") } },
                    new Option(){ Guid = Guid.NewGuid().ToString(), Value = new List<TextValue> (){ new TextValue("en", "Three", "Three") } },
                }
            };
            fields.Add(field);

            f.Fields = fields;

            f.Serialize();
            subSrv.SaveForm(f);

            return f;
        }

        [TestMethod]
        public void CreateSubmissionFormTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            SubmissionService SubSrv = new SubmissionService(Dh.Db);
            string testName = "Test 1";
            string testDescription = "This is a form for the first test.";
            string testPrefix = "Test 1 ";
            Form form1 = CreateFormTemplate(SubSrv, testName, testDescription, testPrefix);
            Dh.Db.SaveChanges();

            Form form2 = SubSrv.GetForm<Form>(form1.Id);

            Assert.AreEqual(form2.Name, testName);
            Assert.AreEqual(form2.Description, testDescription);
            Assert.AreNotEqual(0, form2.Fields.Count);

            foreach (FormField field in form2.Fields)
            {
                Assert.IsTrue(field.Name.StartsWith(testPrefix));
            }

            Assert.AreEqual(form1.Fields.Count, form2.Fields.Count);
        }

        [Ignore]
        [TestMethod]
        public void SaveSubmissionTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(true);
            SubmissionService SubSrv = new SubmissionService(Dh.Db);
            ItemService ItemSrv = new ItemService(Dh.Db);

            string testName = "Test 2";
            string testDescription = "This is a form for the second test.";
            string testPrefix = "Test 2 ";
            Form form1 = CreateFormTemplate(SubSrv, testName, testDescription, testPrefix);
            Dh.Db.SaveChanges();

            Form submission = SubSrv.CreateSubmissionForm(form1.Id);

            Assert.AreEqual(submission.Name, testName);
            Assert.AreEqual(submission.Description, testDescription);
            Assert.AreNotEqual(0, submission.Fields.Count);

            foreach (FormField field in submission.Fields)
            {
                Assert.IsTrue(field.Name.StartsWith(testPrefix));
                Type fieldType = field.GetType();

                if (typeof(RadioButtonSet).IsAssignableFrom(fieldType))
                {
                    var options = ((RadioButtonSet)field).Options;
                    options[0].Selected = true;
                    field.SetValues(new string[] { options[0].Value[0].Value });
                }
                else
                {
                    field.SetValues(new string[]{ "TESTING RESULT" });
                }
                
            }

            submission.Serialize();
            Item itemCreated = SubSrv.SaveSubmission(submission, null, 0, Dh.Db.EntityTypes.FirstOrDefault().Id, form1.Id, Dh.Db.Collections.FirstOrDefault().Id);
            Dh.Db.SaveChanges();

            Item item = ItemSrv.GetItem(itemCreated.Id);

            Assert.IsTrue(item.Id > 0);
            Assert.AreNotEqual(0, item.FormSubmissions.FirstOrDefault().FormData.Fields.Count);

            foreach (FormField field in item.FormSubmissions.FirstOrDefault().FormData.Fields)
            {
                Assert.IsTrue(field.Name.StartsWith(testPrefix));
                Type fieldType = field.GetType();

                if (typeof(RadioButtonSet).IsAssignableFrom(fieldType))
                {
                    Assert.IsTrue(((RadioButtonSet)field).Options[0].Selected);
                }
                else
                {
                    Assert.AreEqual("TESTING RESULT", field.GetValues().FirstOrDefault().Value);
                }

            }
        }
    }
}
