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
  }
}
