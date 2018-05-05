using System.Collections.Generic;
using System.Security.Claims;

namespace ShoppingCart.Library.DomainModels.LoginConfiguration
{
  public class CustomUser
  {
    public string Subject { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public IEnumerable<Claim> Claims { get; set; }
  }
}