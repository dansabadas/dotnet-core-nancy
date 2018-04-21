using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using ShoppingCart.Library.Stores;

namespace Hello_Microservices.NancyModules
{
  public class ShoppingCartModule : NancyModule
  {
    public ShoppingCartModule(IShoppingCartStore shoppingCartStore) : base("/shoppingcart")
    {
      Get("/{userid:int}", parameters =>
      {
        var userId = (int)parameters.userid;
        return shoppingCartStore.Get(userId);
      });
    }
  }
}
