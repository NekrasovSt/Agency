using System.Runtime.InteropServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.Web;

namespace Agency.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var configuration = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config");

      if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        configuration.Configuration.Variables["dir"] = "/var/log/agency";
      }
      else
      {
        configuration.Configuration.Variables["dir"] = @"C:\logs\agency";
      }
      configuration.ReconfigExistingLoggers();
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseNLog();
  }
}
