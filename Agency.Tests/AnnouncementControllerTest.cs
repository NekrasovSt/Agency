using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agency.Web.Controllers;
using Agency.Web.Models;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Agency.Tests
{
    [TestFixture]
    public class AnnouncementControllerTest
    {
        private AgencyContext _context;
        private AnnouncementController _controller;
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

            agencyContext.Announcement.Add(new Announcement()
            {
                RealEstateObject = new RealEstateObject()
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
                },
                Price = 1000000,
                AnnouncementType = AnnouncementType.Buy
            });

            agencyContext.SaveChanges();
            return agencyContext;
        }

        [SetUp]
        public void Init()
        {
            _context = InitContext();
            _controller = new AnnouncementController(_context);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _objectValidator = new Mock<IObjectModelValidator>();
            _objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            _controller.ObjectValidator = _objectValidator.Object;
        }

        [Test]
        public void GetCollection()
        {
            var before = _context.Announcement.Count();

            var result = _controller.Get();

            Assert.That(result.Count(), Is.EqualTo(before));
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
            Assert.That(((Announcement) result.Value).Id, Is.EqualTo(1));
        }

        [Test]
        public void Post()
        {
            var before = _context.Announcement.Count();
            var obj = new RealEstateObject()
            {
                RealEstateType = RealEstateType.Apartment,
                Building = "53",
                City = "Пермь",
                Description = "Description",
                Floor = "3/5",
                Region = "Пермский край",
                Rooms = 2,
                Square = 45,
                Street = "Мильчакова",
                Code = "59000000123"
            };
            _context.RealEstateObject.Add(obj);
            var beforeObjects = _context.RealEstateObject.Count();

            var actual = _controller.Post(new Announcement()
            {
                RealEstateObject = obj,
                Price = 100000,
                AnnouncementType = AnnouncementType.Lease
            });

            var result = actual as CreatedODataResult<Announcement>;
            Assert.That(result, Is.Not.Null);

            var after = _context.Announcement.Count();
            var afterObjects = _context.RealEstateObject.Count();

            Assert.That(after, Is.EqualTo(before + 1));
            Assert.That(afterObjects, Is.EqualTo(beforeObjects));
        }

        [Test]
        public void PutNotFound()
        {
            var announcement = _context.Announcement.FirstOrDefault();
            announcement.AnnouncementType = AnnouncementType.Rent;
            announcement.Price = 9999;
            var actual = _controller.Put(announcement, 10000);
            Assert.That(actual, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Put()
        {
            var before = _context.Announcement.Count();
            var beforeObjects = _context.RealEstateObject.Count();

            var announcement = _context.Announcement.FirstOrDefault();
            announcement.AnnouncementType = AnnouncementType.Rent;
            announcement.Price = 9999;
            var actual = _controller.Put(announcement, announcement.Id);

            var result = actual as CreatedODataResult<Announcement>;
            Assert.That(result, Is.Not.Null);

            var after = _context.Announcement.Count();
            var afterObjects = _context.RealEstateObject.Count();

            Assert.That(after, Is.EqualTo(before));
            Assert.That(afterObjects, Is.EqualTo(beforeObjects));

            announcement = _context.Announcement.FirstOrDefault();

            Assert.That(announcement.AnnouncementType, Is.EqualTo(AnnouncementType.Rent));
            Assert.That(announcement.Price, Is.EqualTo(9999));
        }
    }
}