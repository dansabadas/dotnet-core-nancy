using System.Threading.Tasks;

namespace ShoppingCart.Library.Stores
{
  public interface IShoppingCartStore
  {
    Task<DomainModels.ShoppingCart> Get(int userId);

    Task Save(DomainModels.ShoppingCart shoppingCart);
  }
}
