using Nancy;
using ShoppingCart.Library.Stores;
using System.Collections.Generic;
using System.Linq;

namespace Hello_Microservices.NancyModules
{
  public class ProductsModule : NancyModule
  {
    public ProductsModule(IProductStore productStore) : base("/products")
    {
      Get("", _ =>
      {
        string productIdsString = Request.Query.productIds;
        var productIds = ParseProductIdsFromQueryString(productIdsString);
        var products = productStore.GetProductsByIds(productIds);

        return
          Negotiate
           .WithModel(products)
           .WithHeader("cache-control", "max-age:86400");
      });
    }

    private static IEnumerable<int> ParseProductIdsFromQueryString(string productIdsString)
    {
      return productIdsString.Split(',').Select(s => s.Replace("[", "").Replace("]", "")).Select(int.Parse);
    }
  }
}
