using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;

namespace Agency.Web
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMemoryCache();
      services.AddOData();
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        var options = new WebpackDevMiddlewareOptions() {HotModuleReplacement = true};
        app.UseWebpackDevMiddleware(options);
      }

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseMvc(routes =>
      {
        routes.Select().Expand().Filter().OrderBy().Count();
        routes.MapODataServiceRoute("odata", "odata", ModelBuilder.GetEdmModel(app.ApplicationServices));
        routes.MapRoute(
          name: "default",
          template: "{controller}/{action}/{id?}");
      });
    }
  }
}
