using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models
{
  public class RealEstateObjectFile
  {
    /// <summary>
    /// Объект недвижимости ид
    /// </summary>
    public int RealEstateObjectId { get; set; }

    /// <summary>
    /// Объект недвижимости
    /// </summary>
    public RealEstateObject RealEstateObject { get; set; }

    /// <summary>
    /// Имя файла
    /// </summary>
    [Required]
    public string Name { get; set; }
  }
}
