using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Agency.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Agency.Web.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<User> userManager, IConfiguration configuration)
    {
      _userManager = userManager;
      _configuration = configuration;
    }

    [HttpPost("Token")]
    public async Task<IActionResult> Token([FromBody] UserAuthenticate model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var user = await _userManager.FindByNameAsync(model.Login);

      if (user == null || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, model.Password) !=
          PasswordVerificationResult.Success)
      {
        return BadRequest();
      }

      var token = await GetJwtSecurityToken(user);

      return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private async Task<JwtSecurityToken> GetJwtSecurityToken(User user)
    {
      var userClaims = await _userManager.GetClaimsAsync(user);
      var roles = await _userManager.GetRolesAsync(user);

      var roleClaims = roles.Select(i => new Claim(ClaimTypes.Role, i));

      var now = DateTime.UtcNow;

      return new JwtSecurityToken(
        issuer: "Stock exchange",
        claims: GetTokenClaims(user).Union(userClaims).Union(roleClaims),
#if DEBUG
        expires: DateTime.UtcNow.AddYears(1),
#else
                expires: DateTime.UtcNow.AddHours(1),
#endif
        notBefore: now,
        signingCredentials: new SigningCredentials(
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
          SecurityAlgorithms.HmacSha256)
      );
    }

    private IEnumerable<Claim> GetTokenClaims(User user)
    {
      return new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
          ClaimValueTypes.Integer64)
      };
    }
  }
}
