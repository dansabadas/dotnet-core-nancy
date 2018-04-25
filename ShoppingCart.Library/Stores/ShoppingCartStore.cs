using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ShoppingCart.Library.Stores
{
  public class ShoppingCartStore : IShoppingCartStore
  {
    private const string __connectionString = @"Data Source=s12.winhost.com;Initial Catalog=DB_100072_eastwest;Persist Security Info=True;User ID=DB_100072_eastwest_user;Password=batran11";

    private const string readItemsSql =
      @"select item.* from dotnetcore.ShoppingCart AS cart, dotnetcore.ShoppingCartItems as item
      WHERE item.ShoppingCartId = cart.ID
      and cart.UserId=@UserId";

    public async Task<DomainModels.ShoppingCart> Get(int userId)
    {
      using (var conn = new SqlConnection(__connectionString))
      {
        IEnumerable<DomainModels.ShoppingCartItem> items = null;
        try
        {
          items = await
            conn.QueryAsync<DomainModels.ShoppingCartItem>(readItemsSql, new { UserId = userId });
        }
        catch (System.Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        return new DomainModels.ShoppingCart(userId, items);
      }
    }

    private const string deleteAllForShoppingCartSql =
      @"delete item from dotnetcore.ShoppingCartItems item
      inner join dotnetcore.ShoppingCart cart on item.ShoppingCartId = cart.ID
      and cart.UserId=@UserId";

    private const string addAllForShoppingCartSql =
      @"insert into dotnetcore.ShoppingCartItems 
      (ShoppingCartId, ProductCatalogId, ProductName, ProductDescription, Amount, Currency)
      values 
      (@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";

    public async Task Save(DomainModels.ShoppingCart shoppingCart)
    {
      using (var conn = new SqlConnection(__connectionString))
      {
        using (var tx = conn.BeginTransaction())
        {
          await conn
            .ExecuteAsync(deleteAllForShoppingCartSql, new { shoppingCart.UserId }, tx)
            .ConfigureAwait(false);
          foreach (var item in shoppingCart.Items)
          {
            await conn
              .ExecuteAsync(
                addAllForShoppingCartSql,
                item,
                tx)
            .ConfigureAwait(false);
          }
        }
      }
    }
  }
}
