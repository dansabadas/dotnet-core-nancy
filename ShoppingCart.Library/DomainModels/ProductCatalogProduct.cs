namespace ShoppingCart.Library.DomainModels
{
  public class ProductCatalogProduct
  {
    public ProductCatalogProduct(int productId, string productName, string description, Money price)
    {
      ProductId = productId.ToString();
      ProductName = productName;
      ProductDescription = description;
      Price = price;
    }

    public string ProductId { get; private set; }

    public string ProductName { get; private set; }

    public string ProductDescription { get; private set; }

    public Money Price { get; private set; }
  }
}
