using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agency.Web.Models;
using Agency.Web.Utils;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class RealEstateObjectController : ODataController
  {
    private readonly AgencyContext _context;
    private readonly IHostingEnvironment _appEnvironment;

    public RealEstateObjectController(AgencyContext context, IHostingEnvironment appEnvironment)
    {
      _context = context;
      _appEnvironment = appEnvironment;
    }

    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
      var obj = await _context.RealEstateObject.FindAsync(key);
      if (obj == null)
        return NotFound();
      return Ok(obj);
    }

    [EnableQuery]
    public IQueryable<RealEstateObject> Get()
    {
      return _context.RealEstateObject;
    }

    public IActionResult Post(IFormFile document, List<IFormFile> file)
    {
      RealEstateObject model = document.GetObjectFromForm<RealEstateObject>();
      if (!TryValidateModel(model) || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (file != null)
      {
        var root = Path.Combine(_appEnvironment.WebRootPath, "photos");
        Directory.CreateDirectory(root);
        foreach (IFormFile formFile in file)
        {
          model.RealEstateObjectFile.Add(new RealEstateObjectFile()
          {
            Name = Path.GetFileName(formFile.SaveFromForm(root)),
          });
        }
      }

      _context.RealEstateObject.Add(model);
      _context.SaveChanges();
      return Created(model);
    }

    public IActionResult Put(IFormFile document, List<IFormFile> file, int key)
    {
      var oldModel = _context.RealEstateObject.Find(key);
      if (oldModel == null)
      {
        return NotFound();
      }

      RealEstateObject model = document.GetObjectFromForm<RealEstateObject>();
      if (!TryValidateModel(model) || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var root = Path.Combine(_appEnvironment.WebRootPath, "photos");
      if (file != null)
      {
        Directory.CreateDirectory(root);
        foreach (IFormFile formFile in file)
        {
          model.RealEstateObjectFile.Add(new RealEstateObjectFile()
          {
            Name = Path.GetFileName(formFile.SaveFromForm(root))
          });
        }
      }

      var deleted = oldModel.RealEstateObjectFile.Select(i => i.Name)
        .Except(model.RealEstateObjectFile.Select(i => i.Name));
      foreach (string deletedFile in deleted)
      {
        System.IO.File.Delete(Path.Combine(root, deletedFile));
      }

      oldModel.RealEstateObjectFile = model.RealEstateObjectFile;

      oldModel.Building = model.Building;
      oldModel.City = model.City;
      oldModel.Code = model.Code;
      oldModel.Description = model.Description;
      oldModel.Floor = model.Floor;
      oldModel.Region = model.Region;
      oldModel.Street = model.Street;
      oldModel.Rooms = model.Rooms;
      oldModel.Square = model.Square;
      oldModel.RealEstateType = model.RealEstateType;

      _context.SaveChanges();
      return Ok(model);
    }
  }
}
