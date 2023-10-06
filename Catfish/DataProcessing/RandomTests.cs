using Catfish.API.Repository.Models.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class RandomTests
    {
        [Fact]
        public void ParseJson()
        {
            string file = "c:\\temp\\formData.json";
            Assert.True(File.Exists(file));

            string txt = File.ReadAllText(file);
            try
            {
                var obj = JsonConvert.DeserializeObject<FormData>(txt);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
