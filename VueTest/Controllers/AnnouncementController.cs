using System;
using System.Collections.Generic;
using System.Linq;
using Agency.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers
{

  public class AnnouncementController : ODataController
  {
    // GET
    [EnableQuery]
    public IQueryable<Announcement> Get()
    {
      var list = new List<Announcement>();

      list.Add(new Announcement()
      {
        Id = 1,
        Price = 100,
        CreationDate = DateTimeOffset.Now,
        RealEstateObject = new RealEstateObject()
        {
          Id = 1,
          RealEstateType = RealEstateType.Apartment,
          Building = "1",
          City = "Пермь",
          Description = "бла",
          Floor = "3/5",
          Region = "Пермский край",
          Rooms = 3,
          Square = 23.4,
          Street = "Ленина",
          FIAS = "59000000123"
        }
      });

      return list.AsQueryable();
    }
  }
}
