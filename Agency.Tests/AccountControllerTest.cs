using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Agency.Tests.Helpers;
using Agency.Web.Controllers;
using Agency.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Agency.Tests
{
    [TestFixture]
    public class AccountControllerTest
    {
        private Mock<IConfiguration> _configuration;

        [SetUp]
        public void Init()
        {
            _configuration = new Mock<IConfiguration>();
        }


        [Test]
        public async Task TokenTestBadRequest()
        {
            var fakeUserManager = new Mock<FakeUserManager>();


            var controller = new AccountController(fakeUserManager.Object, _configuration.Object);

            var model = new UserAuthenticate();

            controller.ValidateViewModel(model);

            var result = await controller.Token(model);

            Assert.IsInstanceOf<BadRequestResult>(result);

            result = await controller.Token(new UserAuthenticate() {Login = "mail"});

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task TokenTestNotFoundBadRequest()
        {
            var fakeUserManager = new Mock<FakeUserManager>();

            User user = null;

            fakeUserManager.Setup(i => i.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            var controller = new AccountController(fakeUserManager.Object, _configuration.Object);


            var result = await controller.Token(new UserAuthenticate() {Login = "mail", Password = "pass"});

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task TokenTestOk()
        {
            var fakeUserManager = new Mock<FakeUserManager>();

            User user = new User
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@inmolko.ru",
                NormalizedEmail = "ADMIN@INMOLOKO.RU",
                PasswordHash = "AQAAAAEAACcQAAAAEJDFPxHcK/TpfOQyuHQjbiwGLOJlCaNHmZBCuLAmyNK5pGCjEK1el2FLxpB/DvaDgw==",
                Id = 1,
            };

            fakeUserManager.Setup(i => i.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            fakeUserManager.Setup(i => i.GetClaimsAsync(It.IsAny<User>())).ReturnsAsync(new List<Claim>()
            {
                new Claim("", "", "", "", "")
            });
            fakeUserManager.Setup(i => i.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            _configuration.SetupGet(i => i[It.IsAny<string>()]).Returns("1234567890qwerty");

            var controller = new AccountController(fakeUserManager.Object, _configuration.Object);


            var result = await controller.Token(new UserAuthenticate()
                {Login = "admin@inmolko.ru", Password = "admin"});

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            var token = ok.Value as string;
            Assert.IsNotNull(token);
        }
    }
}