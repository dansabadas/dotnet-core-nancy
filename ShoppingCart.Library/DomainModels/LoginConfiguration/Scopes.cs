using System.Collections.Generic;
using IdentityServer4.Models;

namespace ShoppingCart.Library.DomainModels.LoginConfiguration
{
  public class Scopes
  {
    public static IEnumerable<ApiResource> Get() =>
      new[]
      {
        // standard OpenID Connect scopes
        //IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
        //StandardScopes.Profile,
        //StandardScopes.Email,
        new ApiResource {
          Scopes = new[] {
            new Scope
              {
                Name = "openid",
                DisplayName = "openid"
              },
              new Scope
              {
                Name = "email",
                DisplayName = "email"
              },
              new Scope
              {
                Name = "profile",
                DisplayName = "profile"
              }
            }
          },
        new ApiResource {
          Scopes = new[] {
            new Scope
            {
              Name = "loyalty_program_write",
              DisplayName = "Loyalty Program write access",         
              //Type = ScopeType.Resource,
            }
          }
        }
      };
  }
}
