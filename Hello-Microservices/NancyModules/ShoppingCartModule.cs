using Nancy;
using Nancy.ModelBinding;
using ShoppingCart.Library;
using ShoppingCart.Library.Stores;

namespace Hello_Microservices.NancyModules
{
  public class ShoppingCartModule : NancyModule
  {
    public ShoppingCartModule(IShoppingCartStore shoppingCartStore, IProductCatalogueClient productCatalog, IEventStore eventStore) 
      : base("/shoppingcart")
    {
      Get("/{userid:int}", parameters =>
      {
        var userId = (int)parameters.userid;
        return shoppingCartStore.Get(userId);
      });

      Post("/{userid:int}/items", async (parameters, _) =>
      {
        var productCatalogIds = this.Bind<int[]>(); // binds from the request body
        var userId = (int)parameters.userid;        // binds from URL

        var shoppingCart = shoppingCartStore.Get(userId);
        var shoppingCartItems = await productCatalog
          .GetShoppingCartItems(productCatalogIds)
          .ConfigureAwait(false); // The ConfigureAwait(false) call tells the Task not to save the current thread context (we are not interested in it)

        shoppingCart.AddItems(shoppingCartItems, eventStore);
        shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
      });

      Delete("/{userid:int}/items", (parameters, _) =>
      {
        var productCatalogIds = this.Bind<int[]>(); 
        var userId = (int)parameters.userid;        

        var shoppingCart = shoppingCartStore.Get(userId);

        shoppingCart.RemoveItems(productCatalogIds, eventStore);
        shoppingCartStore.Save(shoppingCart);

        return System.Threading.Tasks.Task.FromResult(shoppingCart);
      });
    }
  }
}
