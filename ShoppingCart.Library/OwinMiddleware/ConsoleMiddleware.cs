using LibOwin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Library.OwinMiddleware
{
  using AppFunc = System.Func<IDictionary<string, object>, Task>;

  public class ConsoleMiddleware
  {
    private AppFunc _next;

    public ConsoleMiddleware(AppFunc next)
    {
      _next = next;
    }

    public Task Invoke(IDictionary<string, object> env)
    {
      var context = new OwinContext(env);
      var method = context.Request.Method;
      var path = context.Request.Path;
      System.Console.WriteLine($"Got request class: {method} {path}");
      return _next(env);
    }
  }
}
