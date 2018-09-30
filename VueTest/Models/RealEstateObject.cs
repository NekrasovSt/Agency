namespace Agency.Web.Models
{
  public class RealEstateObject
  {
    public int Id { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Этаж / Этажей
    /// </summary>
    public string Floor { get; set; }

    /// <summary>
    /// Площадь
    /// </summary>
    public double Square { get; set; }

    public int Rooms { get; set; }

    /// <summary>
    /// область
    /// </summary>
    public string Region { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string Building { get; set; }

    public string FIAS { get; set; }

    public RealEstateType RealEstateType { get; set; }
  }
}
