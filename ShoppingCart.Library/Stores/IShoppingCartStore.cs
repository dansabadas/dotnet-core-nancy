using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShoppingCart.Library.Stores
{
  public interface IShoppingCartStore
  {
    Task<DomainModels.ShoppingCart> Get(int userId);

    Task Save(DomainModels.ShoppingCart shoppingCart);
  }
}
