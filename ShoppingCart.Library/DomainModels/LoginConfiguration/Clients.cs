﻿using System.Collections.Generic;
using IdentityServer4.Models;

namespace ShoppingCart.Library.DomainModels.LoginConfiguration
{
  public class Clients
  {
    public static IEnumerable<Client> Get() =>
      new List<Client>
      {
        new Client
        {
          ClientName = "API Gateway",
          ClientId = "api_gateway",
          ClientSecrets = new List<Secret>
          {
            new Secret("secret".Sha256())
          },
          AllowedScopes = new List<string>
          {
            "loyalty_program_write",
          },
          AllowedGrantTypes = GrantTypes.ClientCredentials
        },

        new Client
        {
          ClientName = "Web Client",
          ClientId = "web",

          RedirectUris = new List<string>
          {
            "http://localhost:5000/signin-oidc",
          },
          PostLogoutRedirectUris = new List<string>
          {
            "http://localhost:5000/",
          },

          AllowedScopes = new List<string>
          {
            "openid",
            "email",
            "profile",
          }
        }
      };
  }
}
