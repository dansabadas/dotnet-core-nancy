namespace ShoppingCart.Library.DomainModels
{
  public class ShoppingCartItem
  {
    public int ProductCatalogueId { get; }
    public string ProductName { get; }
    public string Desscription { get; }
    public Money Price { get; }

    public ShoppingCartItem(
      int productCatalogueId,
      string productName,
      string description,
      Money price)
    {
      ProductCatalogueId = productCatalogueId;
      ProductName = productName;
      Desscription = description;
      Price = price;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      var that = obj as ShoppingCartItem;
      return ProductCatalogueId.Equals(that.ProductCatalogueId);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
      return ProductCatalogueId.GetHashCode();
    }
  }
}
