using System.Linq;
using Agency.Web.Controllers;
using Agency.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        private AgencyContext InitContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<AgencyContext>()
                .UseInMemoryDatabase("agency")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            return new AgencyContext(options);
        }

        [SetUp]
        public void Init()
        {
            _context = InitContext();

            _environment = new Mock<IHostingEnvironment>();
            _controller = new RealEstateObjectController(_context, _environment.Object);
        }

        [Test]
        public void GetCollection()
        {
            _context.RealEstateObject.Add(new RealEstateObject()
            {
                Id = 1,
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
            _context.RealEstateObject.Add(new RealEstateObject()
            {
                Id = 2,
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
            _context.SaveChanges();

            var result = _controller.Get();

            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}