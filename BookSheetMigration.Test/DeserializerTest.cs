using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class DeserializerTest
    {
        [TestMethod]
        [Ignore]
        public void WhenGivenAnXelement_AnObjectIsDeserialized()
        {
            Deserializer<List<int>> deserializer = new Deserializer<List<int>>(new XElement(XName.Get("MyContent", "MyNamespace"), new object[]{2}));
            Assert.IsInstanceOfType(deserializer.deserializeResponse().GetType(), typeof(List<int>));
        }
    }
}
