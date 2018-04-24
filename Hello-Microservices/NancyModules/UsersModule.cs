using Nancy;
using Nancy.ModelBinding;
using ShoppingCart.Library.DomainModels;
using System.Collections.Generic;

namespace Hello_Microservices.NancyModules
{
  public class UsersModule : NancyModule
  {
    private static IDictionary<int, LoyaltyProgramUser> __registeredUsers = new Dictionary<int, LoyaltyProgramUser>();

    public UsersModule() : base("/users")
    {
      Get("/", _ => __registeredUsers.Values);

      Get("/{userId:int}", parameters =>
      {
        int userId = parameters.userId;
        if (__registeredUsers.ContainsKey(userId))
          return __registeredUsers[userId];
        else
          return HttpStatusCode.NotFound;
      });

      Post("/", _ =>
      {
        var newUser = this.Bind<LoyaltyProgramUser>();
        AddRegisteredUser(newUser);
        return CreatedResponse(newUser);
      });

      Put("/{userId:int}", parameters =>
      {
        int userId = parameters.userId;
        var updatedUser = this.Bind<LoyaltyProgramUser>();
        __registeredUsers[userId] = updatedUser;
        return updatedUser;
      });
    }

    private dynamic CreatedResponse(LoyaltyProgramUser newUser)
    {
      var response =
        Negotiate
          .WithStatusCode(HttpStatusCode.Created)
          .WithHeader("Location", Request.Url.SiteBase + "/users/" + newUser.Id)
          .WithModel(newUser);

      return response;
    }

    private void AddRegisteredUser(LoyaltyProgramUser newUser)
    {
      var userId = __registeredUsers.Count;
      newUser.Id = userId;
      __registeredUsers[userId] = newUser;
    }
  }
}
