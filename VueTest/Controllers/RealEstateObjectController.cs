using System.Collections.Generic;
using System.IO;
using System.Linq;
using Agency.Web.Models;
using Agency.Web.Utils;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers
{
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
    public RealEstateObject Get(int key)
    {
      return _context.RealEstateObject.Find(key);
    }

    [EnableQuery]
    public IQueryable<RealEstateObject> Get()
    {
      return _context.RealEstateObject;
    }

    public IActionResult Post(IFormFile document, List<IFormFile> file)
    {
      RealEstateObject model = document.GetObjectFromForm<RealEstateObject>();

      if (!TryValidateModel(model) && !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      foreach (IFormFile formFile in file)
      {
        formFile.SaveFromForm(Path.Combine(_appEnvironment.WebRootPath, "photos"));
      }

      _context.RealEstateObject.Add(model);
      return Created(model);
    }
  }
}
