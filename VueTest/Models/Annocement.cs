using System;

namespace Agency.Web.Models
{
  public class Announcement
  {
    public int Id { get; set; }
    public RealEstateObject RealEstateObject { get; set; }
    public int RealEstateObjectId { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset CreationDate { get; set; } = new DateTimeOffset();
    public AnnouncementType AnnouncementType { get; set; }
  }
}
