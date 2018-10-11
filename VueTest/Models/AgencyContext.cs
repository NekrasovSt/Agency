using System.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agency.Web.Models
{
  public class AgencyContext : IdentityDbContext<User, Role, int>
  {
    public AgencyContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<RealEstateObject> RealEstateObject { get; set; }
  }
}
