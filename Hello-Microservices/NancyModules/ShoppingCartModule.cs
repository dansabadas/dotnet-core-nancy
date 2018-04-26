using Nancy;
using Nancy.ModelBinding;
using ShoppingCart.Library;
using ShoppingCart.Library.Stores;
using System;
using System.Diagnostics;

namespace Hello_Microservices.NancyModules
{
  public class ShoppingCartModule : NancyModule
  {
    public ShoppingCartModule(IShoppingCartStore shoppingCartStore, IProductCatalogueClient productCatalog, IEventStore eventStore) 
      : base("/shoppingcart")
    {
      Get("/{userid:int}", async (parameters) =>
      {
        var userId = (int)parameters.userid;
        try
        {
          await eventStore.Raise("ShoppingCartQueried", new { UserId = userId });
        }
        catch (Exception exc)
        {
          Debug.WriteLine(exc.Message);
        }

        return await shoppingCartStore.Get(userId);
      });

      Post("/{userid:int}/items", async (parameters, _) =>
      {
        var productCatalogIds = this.Bind<int[]>(); // binds from the request body
        var userId = (int)parameters.userid;        // binds from URL

        var shoppingCart = await shoppingCartStore.Get(userId);
        var shoppingCartItems = await productCatalog
          .GetShoppingCartItems(productCatalogIds)
          .ConfigureAwait(false); // The ConfigureAwait(false) call tells the Task not to save the current thread context (we are not interested in it)

        shoppingCart.AddItems(shoppingCartItems, eventStore);
        await shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
      });

      Delete("/{userid:int}/items", async (parameters, _) =>
      {
        var productCatalogIds = this.Bind<int[]>(); 
        var userId = (int)parameters.userid;        

        var shoppingCart = await shoppingCartStore.Get(userId);

        shoppingCart.RemoveItems(productCatalogIds, eventStore);
        await shoppingCartStore.Save(shoppingCart);

        return System.Threading.Tasks.Task.FromResult(shoppingCart);
      });
    }
  }
}
