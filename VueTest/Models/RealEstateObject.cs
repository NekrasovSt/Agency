using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models
{
  public class RealEstateObject
  {
    /// <summary>
    /// Ид
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Этаж / Этажей
    /// </summary>
    [Required]
    public string Floor { get; set; }

    /// <summary>
    /// Площадь
    /// </summary>
    public double Square { get; set; }

    /// <summary>
    /// Количество комнат
    /// </summary>
    public int Rooms { get; set; }

    /// <summary>
    /// область
    /// </summary>
    [Required]
    public string Region { get; set; }

    /// <summary>
    /// Город
    /// </summary>
    [Required]
    public string City { get; set; }

    /// <summary>
    /// Улица
    /// </summary>
    [Required]
    public string Street { get; set; }

    /// <summary>
    /// Строение
    /// </summary>
    [Required]
    public string Building { get; set; }

    /// <summary>
    /// ФИАС код
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Тип недвижимости
    /// </summary>
    public RealEstateType RealEstateType { get; set; }
  }
}
