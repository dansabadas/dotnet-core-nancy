﻿using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

namespace ShoppingCart.Library.DomainModels.LoginConfiguration
{
  static class Users
  {
    public static List<CustomUser> Get()
      =>
        new List<CustomUser>
        {
          new CustomUser{Subject = "818727", Username = "alice", Password = "alice",
            Claims = new[]
            {
              new Claim(JwtClaimTypes.Name, "Alice Smith"),
              new Claim(JwtClaimTypes.GivenName, "Alice"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "User"),
              new Claim(JwtClaimTypes.Id, "1", ClaimValueTypes.Integer64)
            }
          },
          new CustomUser{Subject = "88421113", Username = "bob", Password = "bob",
            Claims = new[]
            {
              new Claim(JwtClaimTypes.Name, "Bob Smith"),
              new Claim(JwtClaimTypes.GivenName, "Bob"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "User"),
              new Claim(JwtClaimTypes.Id, "2", ClaimValueTypes.Integer64)
            }
          }
        };
  }
}
