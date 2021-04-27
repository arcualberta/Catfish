using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Catfish.Test.Helpers;

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

               
                var item = JsonConvert.DeserializeObject<Item>(jsonString);
                var jsonContentString = new StringContent(JsonConvert.SerializeObject(item));
                
                var driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://localhost:44385/manager/");
                var usernameField = driver.FindElement(By.Id("username"));
                usernameField.SendKeys("admin");

                var passwordField = driver.FindElement(By.Id("password"));
                passwordField.SendKeys("password");

                var loginButton = driver.FindElement(By.Id("submit"));
                loginButton.Click();



                var response = await client.PostAsync("manager/items/edit/{0}", jsonContentString);
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
