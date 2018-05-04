using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ShoppingCart.Library.Stores
{
  public class ShoppingCartStore : IShoppingCartStore
  {
    private const string __connectionString = @"Data Source=s12.winhost.com;Initial Catalog=DB_100072_eastwest;Persist Security Info=True;User ID=DB_100072_eastwest_user;Password=batran11";

    private static readonly int __threshold = 0;

    public static async Task<bool> HealthCheck()
    {
      using (var conn = new SqlConnection(__connectionString))
      {
        var count = (await conn.QueryAsync<int>("select count(ID) from dotnetcore.ShoppingCart")).Single();
        return count > __threshold;
      }
    }

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
        await conn.OpenAsync();
        using (var tx = conn.BeginTransaction())
        {
          await conn
            .ExecuteAsync(deleteAllForShoppingCartSql, new { shoppingCart.UserId }, tx)
            .ConfigureAwait(false);
          foreach (var item in shoppingCart.Items)
          {
            var dbPrice = item.Price?.Amount;
            await conn
              .ExecuteAsync(
                addAllForShoppingCartSql,
                new {
                  ShoppingCartId = 1, // this is hardcoded so it needs to have a 1 in the corresponding ShoppingCart parent table
                  item.ProductCatalogId,
                  item.ProductName,
                  ProductDescription = item.Description,
                  Amount = dbPrice.GetValueOrDefault(),
                  Currency = item.Price?.Currency ?? string.Empty
                },
                tx)
            .ConfigureAwait(false);
          }

          tx.Commit();
        }
      }
    }
  }
}
