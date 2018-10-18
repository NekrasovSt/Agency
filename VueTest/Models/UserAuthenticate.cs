using System.ComponentModel.DataAnnotations;

namespace Agency.Web.Models
{
  public class UserAuthenticate
  {
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
  }
}
