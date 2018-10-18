using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Agency.Web.Controllers
{
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class AddressController : Controller

  {
    private readonly IMemoryCache _memoryCache;

    public AddressController(IMemoryCache memoryCache)
    {
      _memoryCache = memoryCache;
    }

    [HttpGet("GetRegion")]
    public async Task<IActionResult> GetRegion(string query)
    {
      string url = $"http://kladr-api.ru/api.php?contentType=region&query={query}";
      return await SendRequest(url);
    }

    private async Task<IActionResult> SendRequest(string url)
    {
      if (_memoryCache.TryGetValue(url, out string content))
      {
        return Content(content, "application/json");
      }

      using (HttpClient client = new HttpClient())
      {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        _memoryCache.Set(url, responseBody, TimeSpan.FromHours(1));
        return Content(responseBody, "application/json");
      }
    }

    [HttpGet("GetCity")]
    public async Task<IActionResult> GetCity(string query, string parentId)
    {
      string url = $"http://kladr-api.ru/api.php?contentType=city&query={query}&regionId={parentId}";
      return await SendRequest(url);
    }

    [HttpGet("GetStreet")]
    public async Task<IActionResult> GetStreet(string query, string parentId)
    {
      string url = $"http://kladr-api.ru/api.php?contentType=street&query={query}&cityId={parentId}";
      return await SendRequest(url);
    }

    [HttpGet("GetBuilding")]
    public async Task<IActionResult> GetBuilding(string query, string parentId)
    {
      string url = $"http://kladr-api.ru/api.php?contentType=building&query={query}&streetId={parentId}";
      return await SendRequest(url);
    }
    [HttpGet("GetParents")]
    public async Task<IActionResult> GetParents(string code, string building)
    {
      string url = $"http://kladr-api.ru/api.php?contentType=building&buildingId={code}&withParent=1";
      using (HttpClient client = new HttpClient())
      {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        JObject obj = JObject.Parse(responseBody);
        return Json(obj["result"].FirstOrDefault(i => i["name"].ToString() == building));
      }
    }
  }
}
