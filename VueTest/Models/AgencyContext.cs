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

    public DbSet<Announcement> Announcement { get; set; }
    public DbSet<RealEstateObject> RealEstateObject { get; set; }
    public DbSet<RealEstateObjectFile> RealEstateObjectFile { get; set; }


    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder builder)
    {
      builder.Entity<RealEstateObjectFile>()
        .HasKey(t => new {t.RealEstateObjectId, t.Name});
      base.OnModelCreating(builder);
    }
  }
}
