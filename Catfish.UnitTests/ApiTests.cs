using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Tests.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Catfish.UnitTests
{
    public class ApiTests
    {
        protected AppDbContext _db;
        protected TestHelper _testHelper;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

        [Test]
        public async System.Threading.Tasks.Task ItemSaveTestAsync()
        {
            //Use api/items/ID GET call to get a json object 
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44385/");

            try
            {
                //Use api/item POST method to post the json object back
                var srcDataResponse = await client.GetAsync(client.BaseAddress + "manager/api/Items/d6c5dfe4-946d-4f58-972e-00056eb6af1b");
                string jsonString = await srcDataResponse.Content.ReadAsStringAsync();

                var jsonContentString = new StringContent(jsonString);
                var viewModel = new Item() { Id = Guid.Parse("d6c5dfe4-946d-4f58-972e-00056eb6af1b"), Updated = DateTime.Now, };
                var response = await client.PostAsync("manager/items/edit/", jsonContentString);
                var responseJson = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<ItemSaveResponseVM>(responseJson);

                Assert.AreEqual("OK", obj.Status);
            }
            catch (Exception e)
            {
                throw new Exception("error" + e);
            }


        }




    }
}
