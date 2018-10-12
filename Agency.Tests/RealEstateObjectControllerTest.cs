using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agency.Tests.Helpers;
using Agency.Web.Controllers;
using Agency.Web.Models;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Agency.Tests
{
    [TestFixture]
    public class RealEstateObjectControllerTest
    {
        private AgencyContext _context;
        private Mock<IHostingEnvironment> _environment;
        private RealEstateObjectController _controller;
        private Mock<IObjectModelValidator> _objectValidator;

        private AgencyContext InitContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<AgencyContext>()
                .UseInMemoryDatabase("agency")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var agencyContext = new AgencyContext(options);

            agencyContext.RealEstateObject.Add(new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "1",
                City = "Пермь",
                Description =
                    "Малоквартирный дом бизнес-класса. Все квартиры в нашем доме имеют свободную планировку. \nКвартира расположена на 3 этаже 12 этажного дома. Общая площадь квартиры 115,4 кв.м., жилая 66 кв.м., просторная прихожая, высокие потолки 2,9м. \nВ нашей квартире имеется: большая кухня-столовая, большая ванная комната с отдельной душевой кабиной и санузлом, гостиная, спальня/детская, спальня, 2 гардеробных комнаты, лоджия. На кухне, в ванной комнате и коридоре установлен теплый пол. В квартире сделан ремонт из качественных европейских материалов. \nДоступ на территорию дома ограничен для посторонних лиц. Ограждение дома оборудовано автоматическими воротами. На всей территории установлено видеонаблюдение. В подъезде, для Вашей безопасности, работает охрана.",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 1,
                Square = 23.4,
                Street = "Ленина",
                Code = "59000000123"
            });
            agencyContext.RealEstateObject.Add(new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "1",
                City = "Пермь",
                Description =
                    "Продам 2-комн квартиру, в хорошем состоянии, комн изолированные, просторный коридор, стеклопакеты деревянные, металлическая входная дверь, Инфраструктура развита, в шаговой доступности ост. Давыдова, Леонова, супермаркеты, детские сады, привязка к физико-математ школе №102, документы готовы. Любая форма оплаты, помогу оформить ипотеку по заниженной процентной ставки, звоните отвечу на любой интересующий вопрос.",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 2,
                Square = 30.4,
                Street = "Ленина",
                Code = "59000000123"
            });
            agencyContext.RealEstateObject.Add(new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "1",
                City = "Пермь",
                Description =
                    "Продам квартиру в отличном состоянии. Все комнаты изолированы, две лоджии застеклены. Установлены окна пвх, пол ламинат, трубы металлопластик, счетчики воды, имеется закрываемая перегородка на площадке на 4 квартиры (можно ставить коляски не украдут и т.д.), в связи с тем, что квартира на 1-м этаже, не предусмотрена оплата за лифт, также есть домофон. \nВозможен торг.\nили\nобменяю на 2-х комнатную в аналогичном кирпичном доме с вашей доплатой.",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 3,
                Square = 33.4,
                Street = "Ленина",
                Code = "59000000123",
                RealEstateObjectFile = new HashSet<RealEstateObjectFile>()
                {
                    new RealEstateObjectFile()
                    {
                        Name = "test-tmp.txt"
                    },
                    new RealEstateObjectFile()
                    {
                        Name = "test-tmp-1.txt"
                    }
                }
            });
            agencyContext.SaveChanges();
            return agencyContext;
        }

        [SetUp]
        public void Init()
        {
            _context = InitContext();

            _environment = new Mock<IHostingEnvironment>();
            _controller = new RealEstateObjectController(_context, _environment.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _objectValidator = new Mock<IObjectModelValidator>();
            _objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _controller.ObjectValidator = _objectValidator.Object;
            _environment.SetupGet(i => i.WebRootPath).Returns(Path.GetTempPath());
        }

        [Test]
        public void GetCollection()
        {
            var result = _controller.Get();

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetByIdNotFound()
        {
            var actual = await _controller.Get(100);
            var result = actual as NotFoundResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByIdOk()
        {
            var actual = await _controller.Get(1);
            var result = actual as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(((RealEstateObject) result.Value).Id, Is.EqualTo(1));
        }

        [Test]
        public void PostModelInvalid()
        {
            var modelMock = new Mock<IFormFile>();
            var model = new RealEstateObject();
            modelMock.Setup(i => i.OpenReadStream()).Returns(model.ToJsonStream());
            _controller.ValidateViewModel(model);

            var actual = _controller.Post(modelMock.Object, null);
            var result = actual as BadRequestObjectResult;
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void PostModelEmptyFiles()
        {
            var before = _context.RealEstateObject.Count();

            var modelMock = new Mock<IFormFile>();
            var model = new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "1",
                City = "Пермь",
                Description =
                    "Малоквартирный дом бизнес-класса. Все квартиры в нашем доме имеют свободную планировку. \nКвартира расположена на 3 этаже 12 этажного дома. Общая площадь квартиры 115,4 кв.м., жилая 66 кв.м., просторная прихожая, высокие потолки 2,9м. \nВ нашей квартире имеется: большая кухня-столовая, большая ванная комната с отдельной душевой кабиной и санузлом, гостиная, спальня/детская, спальня, 2 гардеробных комнаты, лоджия. На кухне, в ванной комнате и коридоре установлен теплый пол. В квартире сделан ремонт из качественных европейских материалов. \nДоступ на территорию дома ограничен для посторонних лиц. Ограждение дома оборудовано автоматическими воротами. На всей территории установлено видеонаблюдение. В подъезде, для Вашей безопасности, работает охрана.",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 1,
                Square = 23.4,
                Street = "Ленина",
                Code = "59000000123"
            };
            modelMock.Setup(i => i.OpenReadStream()).Returns(model.ToJsonStream());
            var actual = _controller.Post(modelMock.Object, null);
            var result = actual as CreatedODataResult<RealEstateObject>;
            Assert.That(result, Is.Not.Null);
            var after = _context.RealEstateObject.Count();

            Assert.That(after, Is.EqualTo(before + 1));
        }

        [Test]
        public void PostModelWithFiles()
        {
            var before = _context.RealEstateObject.Count();

            //Model
            var modelMock = new Mock<IFormFile>();
            var model = new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "1",
                City = "Пермь",
                Description =
                    "Малоквартирный дом бизнес-класса. Все квартиры в нашем доме имеют свободную планировку. \nКвартира расположена на 3 этаже 12 этажного дома. Общая площадь квартиры 115,4 кв.м., жилая 66 кв.м., просторная прихожая, высокие потолки 2,9м. \nВ нашей квартире имеется: большая кухня-столовая, большая ванная комната с отдельной душевой кабиной и санузлом, гостиная, спальня/детская, спальня, 2 гардеробных комнаты, лоджия. На кухне, в ванной комнате и коридоре установлен теплый пол. В квартире сделан ремонт из качественных европейских материалов. \nДоступ на территорию дома ограничен для посторонних лиц. Ограждение дома оборудовано автоматическими воротами. На всей территории установлено видеонаблюдение. В подъезде, для Вашей безопасности, работает охрана.",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 1,
                Square = 23.4,
                Street = "Ленина",
                Code = "59000000123"
            };
            modelMock.Setup(i => i.OpenReadStream()).Returns(model.ToJsonStream());

            //File
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(i => i.OpenReadStream()).Returns("Some text".ToStream());
            fileMock.SetupGet(i => i.FileName).Returns("some.txt");


            var actual = _controller.Post(modelMock.Object, new List<IFormFile>() {fileMock.Object});
            var result = actual as CreatedODataResult<RealEstateObject>;
            Assert.That(result, Is.Not.Null);
            var after = _context.RealEstateObject.Count();

            Assert.That(after, Is.EqualTo(before + 1));

            var newObject = _context.RealEstateObject.OrderBy(i => i.Id).Last();
            Assert.That(newObject.RealEstateObjectFile.Count(), Is.EqualTo(1));
            var fileName = newObject.RealEstateObjectFile.FirstOrDefault().Name;

            var path = Path.Combine(Path.GetTempPath(), "photos", fileName);
            var text = File.ReadAllText(path);
            Assert.That(text, Is.EqualTo("Some text"));

            File.Delete(path);
        }

        [Test]
        public void PutNotFound()
        {
            var actual = _controller.Put(null, null, 100);
            Assert.That(actual, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void PutModelInvalid()
        {
            var modelMock = new Mock<IFormFile>();
            var model = new RealEstateObject();
            modelMock.Setup(i => i.OpenReadStream()).Returns(model.ToJsonStream());
            _controller.ValidateViewModel(model);

            var actual = _controller.Put(modelMock.Object, null, 100);
            Assert.That(actual, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void PutEmptyFiles()
        {
            var before = _context.RealEstateObject.Count();

            var modelMock = new Mock<IFormFile>();
            var model = new RealEstateObject()
            {
                RealEstateType = RealEstateType.House,
                Building = "Building is changed",
                City = "City is changed",
                Description = "Description is changed",
                Floor = "Floor is changed",
                Region = "Region is changed",
                Rooms = 2,
                Square = 100,
                Street = "Street is changed",
                Code = "Code is changed"
            };


            modelMock.Setup(i => i.OpenReadStream()).Returns(model.ToJsonStream());
            var actual = _controller.Put(modelMock.Object, null, 1);
            var result = actual as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            var after = _context.RealEstateObject.Count();

            Assert.That(after, Is.EqualTo(before));

            var changed = _context.RealEstateObject.Find(1);
            Assert.That(changed.Description, Is.EqualTo("Description is changed"));
            Assert.That(changed.Street, Is.EqualTo("Street is changed"));
            Assert.That(changed.RealEstateType, Is.EqualTo(RealEstateType.House));
            Assert.That(changed.Building, Is.EqualTo("Building is changed"));
            Assert.That(changed.City, Is.EqualTo("City is changed"));
            Assert.That(changed.Floor, Is.EqualTo("Floor is changed"));
            Assert.That(changed.Region, Is.EqualTo("Region is changed"));
            Assert.That(changed.Rooms, Is.EqualTo(2));
            Assert.That(changed.Square, Is.EqualTo(100));
            Assert.That(changed.Code, Is.EqualTo("Code is changed"));
        }

        [Test]
        public void PutDeleteFile()
        {
            var before = _context.RealEstateObject.Count();
            //Model
            var modelMock = new Mock<IFormFile>();
            var model = new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "1",
                City = "Пермь",
                Description =
                    "Малоквартирный дом бизнес-класса. Все квартиры в нашем доме имеют свободную планировку. \nКвартира расположена на 3 этаже 12 этажного дома. Общая площадь квартиры 115,4 кв.м., жилая 66 кв.м., просторная прихожая, высокие потолки 2,9м. \nВ нашей квартире имеется: большая кухня-столовая, большая ванная комната с отдельной душевой кабиной и санузлом, гостиная, спальня/детская, спальня, 2 гардеробных комнаты, лоджия. На кухне, в ванной комнате и коридоре установлен теплый пол. В квартире сделан ремонт из качественных европейских материалов. \nДоступ на территорию дома ограничен для посторонних лиц. Ограждение дома оборудовано автоматическими воротами. На всей территории установлено видеонаблюдение. В подъезде, для Вашей безопасности, работает охрана.",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 1,
                Square = 23.4,
                Street = "Ленина",
                Code = "59000000123"
            };
            modelMock.Setup(i => i.OpenReadStream()).Returns(model.ToJsonStream());
            //Set test file
            var path = Path.Combine(Path.GetTempPath(), "photos", "test-tmp.txt");
            File.WriteAllText(path, "Some text");


            var actual = _controller.Put(modelMock.Object, null, 3);
            var result = actual as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            var after = _context.RealEstateObject.Count();

            Assert.That(after, Is.EqualTo(before));
            var changed = _context.RealEstateObject.Find(3);
            Assert.That(changed.RealEstateObjectFile.Count, Is.EqualTo(0));

            Assert.That(File.Exists(path), Is.False);
        }
    }
}