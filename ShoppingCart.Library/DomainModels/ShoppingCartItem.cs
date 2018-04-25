namespace ShoppingCart.Library.DomainModels
{
  public class ShoppingCartItem
  {
    public int ProductCatalogId { get; }

    public string ProductName { get; }

    public string Description { get; }

    public Money Price { get; }

    public ShoppingCartItem()
    {
    }

    public ShoppingCartItem(int productCatalogueId, string productName, string description, Money price)
    {
      ProductCatalogId = productCatalogueId;
      ProductName = productName;
      Description = description;
      Price = price;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      var that = obj as ShoppingCartItem;
      return ProductCatalogId.Equals(that.ProductCatalogId);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
      return ProductCatalogId.GetHashCode();
    }
  }
}
