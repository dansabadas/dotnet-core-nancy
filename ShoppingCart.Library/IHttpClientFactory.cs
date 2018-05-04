using System;
using System.Net.Http;

namespace ShoppingCart.Library
{
  public interface IHttpClientFactory
  {
    HttpClient Create(Uri uri);
  }

  public class HttpClientFactory : IHttpClientFactory
  {
    private readonly string _correlationToken;

    public HttpClientFactory(string correlationToken)
    {
      _correlationToken = correlationToken;
    }

    public HttpClientFactory()
    {
    }

    public HttpClient Create(Uri uri)
    {
      var client = new HttpClient() { BaseAddress = uri };
      client.DefaultRequestHeaders.Add("Correlation-Token", _correlationToken);
      return client;
    }
  }
}
