using System;
using System.IO;
using System.Text;
using Agency.Tests.Helpers;
using Agency.Tests.Models;
using Agency.Web.Utils;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Agency.Tests
{
    [TestFixture]
    public class FormDataHelper
    {
        private Mock<IFormFile> _formFile;

        [SetUp]
        public void Init()
        {
            _formFile = new Mock<IFormFile>();
        }


        [Test]
        public void GetObjectFromForm()
        {
            using (var stream = "{\"name\":\"foo\"}".ToStream())
            {
                _formFile.Setup(i => i.OpenReadStream()).Returns(stream);

                var model = _formFile.Object.GetObjectFromForm<NameModel>();
                Assert.That(model.Name, Is.EqualTo("foo"));
            }
        }

        [Test]
        public void SaveFromForm()
        {
            using (var stream = "{\"name\":\"foo\"}".ToStream())
            {
                _formFile.Setup(i => i.OpenReadStream()).Returns(stream);
                _formFile.SetupGet(i => i.FileName).Returns("tmp.json");

                string tmp = Path.GetTempPath();
                var fileName = _formFile.Object.SaveFromForm(tmp);

                var model = JsonConvert.DeserializeObject<NameModel>(File.ReadAllText(fileName));

                Assert.That(model.Name, Is.EqualTo("foo"));
                File.Delete(fileName);
            }
        }
    }
}