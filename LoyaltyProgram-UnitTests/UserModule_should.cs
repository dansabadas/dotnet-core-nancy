using System.Threading.Tasks;
using Nancy;
using Nancy.Testing;
using Xunit;
using ShoppingCart.Library.DomainModels;
using Microsoft.Extensions.Logging;

namespace LoyaltyProgram_UnitTests
{
  public class UserModule_should
  {
    private Browser _sut;

    public UserModule_should()
    {
      var log = ShoppingCart.Library.ILoggerFactory.ConfigureLogger();

      _sut = new Browser(
        new Hello_Microservices.Bootstrapper(log), // this Bootstrapper class is everything, it plugs the routes!
        defaultsTo => defaultsTo.Accept("application/json")
      );
    }

    [Fact]
    public async Task respond_not_when_queried_for_unregistered_user()
    {
      var actual = await _sut.Get("/users/1000");
      Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task allow_to_register_new_user()
    {
      var expected = new LoyaltyProgramUser { Name = "Chr" };
      var registrationResponse = await _sut.Post("/users", with => with.JsonBody(expected));
      var newUser = registrationResponse.Body.DeserializeJson<LoyaltyProgramUser>();

      var actual = await _sut.Get($"/users/{newUser.Id}");

      Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
      Assert.Equal(expected.Name, actual.Body.DeserializeJson<LoyaltyProgramUser>().Name);
    }

    [Fact]
    public async Task allow_modifying_users()
    {
      var expected = "jane";
      var user = new LoyaltyProgramUser { Name = "Chr" };
      var registrationResponse = await _sut.Post("/users", with => with.JsonBody(user));
      var newUser = registrationResponse.Body.DeserializeJson<LoyaltyProgramUser>();

      newUser.Name = expected;
      var actual = await _sut.Put($"/users/{newUser.Id}", with => with.JsonBody(newUser));

      Assert.Equal(expected, actual.Body.DeserializeJson<LoyaltyProgramUser>().Name);
    }
  }
}
