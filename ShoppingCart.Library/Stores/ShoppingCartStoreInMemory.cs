using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Library.Stores
{
  public class ShoppingCartStoreInMemory
  {
    private static readonly Dictionary<int, DomainModels.ShoppingCart> __database = new Dictionary<int, DomainModels.ShoppingCart>();

    public Task<DomainModels.ShoppingCart> Get(int userId)
    {
      if (!__database.ContainsKey(userId))
      {
        __database[userId] = new DomainModels.ShoppingCart(userId);
      }

      return Task.FromResult(__database[userId]);
    }

    public Task Save(DomainModels.ShoppingCart shoppingCart)
    {
      // Nothing needed. Saving would be needed with a real DB
      __database[shoppingCart.UserId] = shoppingCart;
      return Task.CompletedTask;
    }
  }
}
