using System;
using Agency.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Agency.Web.Utils
{
  public class DbInitializer
  {
    public static void Initialize(AgencyContext context) //SchoolContext is EF context
    {
      context.Database.EnsureCreated();
      context.Announcement.Add(new Announcement()
      {
        Price = 1000000,
        CreationDate = DateTimeOffset.Now,
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
        }
      });
      context.Announcement.Add(new Announcement()
      {
        Price = 1500000,
        CreationDate = DateTimeOffset.Now,
        RealEstateObject = new RealEstateObject()
        {
          RealEstateType = RealEstateType.Apartment,
          Building = "2",
          City = "Пермь",
          Description =
            "Продам 2-комн квартиру, в хорошем состоянии, комн изолированные, просторный коридор, стеклопакеты деревянные, металлическая входная дверь, Инфраструктура развита, в шаговой доступности ост. Давыдова, Леонова, супермаркеты, детские сады, привязка к физико-математ школе №102, документы готовы. Любая форма оплаты, помогу оформить ипотеку по заниженной процентной ставки, звоните отвечу на любой интересующий вопрос.",
          Floor = "3/5",
          Region = "Пермский край",
          Rooms = 2,
          Square = 30.4,
          Street = "Карла Маркса",
          Code = "59000000123"
        }
      });
      context.Announcement.Add(new Announcement()
      {
        Price = 2000000,
        CreationDate = DateTimeOffset.Now,
        RealEstateObject = new RealEstateObject()
        {
          RealEstateType = RealEstateType.Apartment,
          Building = "3",
          City = "Пермь",
          Description =
            "Продам квартиру в отличном состоянии. Все комнаты изолированы, две лоджии застеклены. Установлены окна пвх, пол ламинат, трубы металлопластик, счетчики воды, имеется закрываемая перегородка на площадке на 4 квартиры (можно ставить коляски не украдут и т.д.), в связи с тем, что квартира на 1-м этаже, не предусмотрена оплата за лифт, также есть домофон. \nВозможен торг.\nили\nобменяю на 2-х комнатную в аналогичном кирпичном доме с вашей доплатой.",
          Floor = "3/5",
          Region = "Пермский край",
          Rooms = 3,
          Square = 33.4,
          Street = "Пушкина",
          Code = "59000000123"
        }
      });
      context.Announcement.Add(new Announcement()
      {
        Price = 5000000,
        CreationDate = DateTimeOffset.Now,
        RealEstateObject = new RealEstateObject()
        {
          RealEstateType = RealEstateType.House,
          Building = "4",
          City = "Пермь",
          Description =
            "Продаём светлую, тёплую, просторную квартиру с хорошим ремонтом! С отличным расположением дома! С мебелью, техникой) У дома большая парковка) в шаговой доступности школа, лицей, д/сады, торговые центры, остановки! Покажем в удобное для вас время!",
          Floor = "3/5",
          Region = "Пермский край",
          Rooms = 3,
          Square = 33.4,
          Street = "Советская",
          Code = "59000000123"
        }
      });
      context.Announcement.Add(new Announcement()
      {
        Price = 5500000,
        CreationDate = DateTimeOffset.Now,
        RealEstateObject = new RealEstateObject()
        {
          RealEstateType = RealEstateType.House,
          Building = "4",
          City = "Пермь",
          Description =
            "Общая площадь однокомнатной квартиры 34,18 кв.м., жилая 13,93 кв.м., кухня 9,99 кв.м., лоджия 2,2 кв.м. с выходом из кухни, совмещенный санузел 3,98 кв.м., холл 5,18 кв.м., окна квартиры выходят на южную сторону (ул.Цимлянская)",
          Floor = "3/5",
          Region = "Пермский край",
          Rooms = 4,
          Square = 33.4,
          Street = "Ленина",
          Code = "59000000123"
        }
      });
      context.Announcement.Add(new Announcement()
      {
        Price = 2000000,
        CreationDate = DateTimeOffset.Now,
        RealEstateObject = new RealEstateObject()
        {
          RealEstateType = RealEstateType.NewBuilding,
          Building = "5",
          City = "Пермь",
          Description =
            "СРОЧНО!!! продам двухкомнатную квартиру в п.Горный на втором этаже двухэтажного дома, площадь комнат 15.5 и 7.5, коридор 2.8, санузел 2.4, на окнах стеклопакеты, натяжной потолок. Тихое и спокойное место. п.Горный активно застраивается, сеть магазинов, регулярное автобусное сообщение. Условие, возможно снижение цены. Гараж в подарок.",
          Floor = "3/5",
          Region = "Пермский край",
          Rooms = 3,
          Square = 33.4,
          Street = "Ленина",
          Code = "59000000123"
        }
      });
      context.SaveChanges();

      var admin = new Role
      {
        Name = "Admin",
        NormalizedName = "ADMIN"
      };
      context.Roles.AddRange(admin);
      context.SaveChanges();
      var hasher = new PasswordHasher<User>();
      var user = new User
      {
        UserName = "1",
        NormalizedUserName = "1",
        Email = "admin@inmoloko.ru",
        NormalizedEmail = "ADMIN@INMOLOKO.RU",
        SecurityStamp = Guid.NewGuid().ToString()
      };
      user.PasswordHash = hasher.HashPassword(user, "1");
      context.Add(user);
      context.SaveChanges();
      var ur = new IdentityUserRole<int>()
      {
        UserId = user.Id,
        RoleId = admin.Id
      };
      context.UserRoles.Add(ur);
      context.SaveChanges();
    }
  }
}
