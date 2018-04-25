using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Library.DomainModels
{
  public class ShoppingCart
  {
    private HashSet<ShoppingCartItem> _items = new HashSet<ShoppingCartItem>();

    public int UserId { get; }
    public IEnumerable<ShoppingCartItem> Items { get { return _items; } }

    public ShoppingCart(int userId)
    {
      UserId = userId;
    }

    public ShoppingCart(int userId, IEnumerable<ShoppingCartItem> items)
    {
      UserId = userId;
      foreach (var item in items)
      {
        _items.Add(item);
      }
    }

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, Stores.IEventStore eventStore)
    {
      foreach (var item in shoppingCartItems)
      {
        if (_items.Add(item))
        {
          eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });
        }
      }
    }

    public void RemoveItems(int[] productCatalogueIds, Stores.IEventStore eventStore)
    {
      _items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogId));
    }
  }
}
