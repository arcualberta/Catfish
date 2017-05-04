using System;
using Catfish.Core.Models.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Catfish.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var option_filed_types = typeof(OptionsField).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(OptionsField))).ToList();

            var n = option_filed_types.Count;
        }
    }
}
