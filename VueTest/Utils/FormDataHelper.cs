using System;
using System.IO;
using Agency.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace Agency.Web.Utils
{
  public static class FormDataHelper
  {
    public static T GetObjectFromForm<T>(this IFormFile data)
    {
      var js = new JsonSerializer();
      using (var reader = new StreamReader(data.OpenReadStream()))
      {
        using (var jtr = new JsonTextReader(reader))
        {
          return js.Deserialize<T>(jtr);
        }
      }
    }

    public static string SaveFromForm(this IFormFile data, string rootDirectory)
    {
      var extension = Path.GetExtension(data.FileName);

      var newFileName = Path.Combine(rootDirectory, $"{Guid.NewGuid()}{extension}");
      using (var reader = data.OpenReadStream())
      using (var fileStream =
        new FileStream(newFileName, FileMode.Create))
      {
        reader.CopyTo(fileStream);
      }

      return newFileName;
    }
  }
}
