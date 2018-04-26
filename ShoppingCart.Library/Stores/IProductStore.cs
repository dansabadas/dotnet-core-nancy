using ShoppingCart.Library.DomainModels;
using System.Collections.Generic;

namespace ShoppingCart.Library.Stores
{
  public interface IProductStore
  {
    IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds);
  }
}
