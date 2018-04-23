using System.Collections.Generic;

namespace ShoppingCart.Library.Stores
{
  public interface IShoppingCartStore
  {
    DomainModels.ShoppingCart Get(int userId);
    void Save(DomainModels.ShoppingCart shoppingCart);
  }

  public class ShoppingCartStore : IShoppingCartStore
  {
    private static readonly Dictionary<int, DomainModels.ShoppingCart> _database = new Dictionary<int, DomainModels.ShoppingCart>();

    public DomainModels.ShoppingCart Get(int userId)
    {
      if (!_database.ContainsKey(userId))
      {
        _database[userId] = new DomainModels.ShoppingCart(userId);
      }

      return _database[userId];
    }

    public void Save(DomainModels.ShoppingCart shoppingCart)
    {
      // Nothing needed. Saving would be needed with a real DB
    }
  }
}
