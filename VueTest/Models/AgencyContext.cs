using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Agency.Web.Models
{
  public class AgencyContext : IdentityDbContext<User, Role, int>
  {
  }
}
