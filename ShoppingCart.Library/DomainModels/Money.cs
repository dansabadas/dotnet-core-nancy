namespace ShoppingCart.Library.DomainModels
{
  public class Money
  {
    public string Currency { get; }

    public decimal Amount { get; }

    public Money(string currency, decimal amount)
    {
      Currency = currency;
      Amount = amount;
    }

    public Money()
    {
      Currency = "USD";
      Amount = 0;
    }
  }
}
