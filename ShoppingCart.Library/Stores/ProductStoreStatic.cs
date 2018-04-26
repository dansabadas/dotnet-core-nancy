using ShoppingCart.Library.DomainModels;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Library.Stores
{
  public class ProductStoreStatic : IProductStore
  {
    public IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds)
    {
      return productIds.Select(id => new ProductCatalogProduct(id, "foo" + id, "bar" + id, new Money()));
    }
  }
}
