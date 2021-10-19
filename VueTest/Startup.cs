using System.IO;
using System.Text;
using Agency.Web.Models;
using Agency.Web.Utils;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Agency.Web
{
  public class Startup
  {
    private IConfigurationRoot Configuration;

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "settings"))
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

      IsDevelopment = env.IsDevelopment();
      if (IsDevelopment)
      {
        builder.AddUserSecrets<Startup>();
      }
      Configuration = builder.Build();
    }

    private bool IsDevelopment { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMemoryCache();

      if (IsDevelopment)
        services.AddDbContext<AgencyContext>(opt => opt.UseInMemoryDatabase("agency"));
      else
        services.AddDbContext<AgencyContext>(
          options => { options.UseNpgsql(Configuration["ConnectionString:Agency"]); });

      services.AddAuthentication(o =>
        {
          o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = false;
          options.TokenValidationParameters = new TokenValidationParameters
          {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = false,
            // строка, представляющая издателя
            ValidIssuer = "Stock exchange",

            // будет ли валидироваться потребитель токена
            ValidateAudience = false,
            // установка потребителя токена
            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
          };
        });
      services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<AgencyContext>()
        .AddDefaultTokenProviders();
      services.AddOData();
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, AgencyContext context)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        var options = new WebpackDevMiddlewareOptions() {HotModuleReplacement = true};
        app.UseWebpackDevMiddleware(options);

        DbInitializer.InitializeDevelop(context);
      }
      else
      {
        DbInitializer.InitializaProduction(context);
      }

      app.UseAuthentication();
      app.UseDefaultFiles();
      app.UseStaticFiles();


      app.UseMvc(routes =>
      {
        routes.Select().Expand().Filter().OrderBy().Count().MaxTop(1000);
        routes.MapODataServiceRoute("odata", "odata", ModelBuilder.GetEdmModel(app.ApplicationServices));
        routes.MapRoute(
          name: "default",
          template: "{controller}/{action}/{id?}");
      });
    }
  }
}
