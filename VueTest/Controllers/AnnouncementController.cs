using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agency.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agency.Web.Controllers
{
  public class AnnouncementController : ODataController
  {
    private readonly AgencyContext _context;

    public AnnouncementController(AgencyContext context)
    {
      _context = context;
    }

    // GET
    [EnableQuery]
    public IQueryable<Announcement> Get()
    {
      return _context.Announcement.Include(i => i.RealEstateObject.RealEstateObjectFile);
    }

    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
      var obj = await _context.Announcement
        .Include(i => i.RealEstateObject)
        .Include(i => i.RealEstateObject.RealEstateObjectFile)
        .FirstOrDefaultAsync(i => i.Id == key);
      if (obj == null)
        return NotFound();
      return Ok(obj);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Post([FromBody]Announcement model)
    {
      if (!TryValidateModel(model) || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _context.Attach(model.RealEstateObject);
      _context.Announcement.Add(model);
      _context.SaveChanges();
      return Created(model);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Put([FromBody]Announcement model, int key)
    {
      var old = _context.Announcement.Find(key);
      if (old == null)
      {
        return NotFound();
      }

      if (!TryValidateModel(model) || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _context.Attach(model.RealEstateObject);

      old.RealEstateObject = model.RealEstateObject;
      old.Price = model.Price;
      old.AnnouncementType = model.AnnouncementType;
      old.RealEstateObjectId = model.RealEstateObjectId;

      _context.SaveChanges();
      return Created(model);
    }
  }
}
