using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agency.Web.Controllers
{
  [Route("api/[controller]")]
  public class UploadController : Controller
  {
    public async Task<IActionResult> AddFile(IFormCollection collection)
    {
//        if (uploadedFile != null)
//        {
//          // путь к папке Files
//          string path = "/Files/" + uploadedFile.FileName;
//          // сохраняем файл в папку Files в каталоге wwwroot
//          using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
//          {
//            await uploadedFile.CopyToAsync(fileStream);
//          }
//        }
      var frm = Request.Form;
      return Ok(Guid.NewGuid());
    }
  }
}
