using LibOwin;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Hello_Microservices
{
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
