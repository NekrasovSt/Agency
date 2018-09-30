using System.Collections.Generic;
using Agency.Web.Models;

namespace Agency.Web.Interfaces
{
  public interface IRealEstateRepository
  {
    IEnumerable<Announcement> GetAnnouncements();
  }
}
