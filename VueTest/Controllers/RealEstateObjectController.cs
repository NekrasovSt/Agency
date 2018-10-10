using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Agency.Web.Models;
using Agency.Web.Utils;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Agency.Web.Controllers
{

  public class RealEstateObjectController : ODataController
  {
    public IActionResult Post(IFormFile document, List<IFormFile> file)
    {
      RealEstateObject result = document.GetObjectFromForm<RealEstateObject>();

      return Ok(result);
    }
  }
}
