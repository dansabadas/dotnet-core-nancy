using LibOwin;
using Serilog;

namespace ShoppingCart.Library.OwinMiddleware
{
  using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

  public class RequestLogging
  {
    public static AppFunc Middleware(AppFunc next, ILogger log)
    {
      return async env =>
      {
        var owinContext = new OwinContext(env);
        log.Information(
          "Incoming request: {@Method}, {@Path}, {@Headers}",
          owinContext.Request.Method,
          owinContext.Request.Path,
          owinContext.Request.Headers);
        await next(env);
        log.Information(
          "Outgoing response: {@StatucCode}, {@Headers}",
           owinContext.Response.StatusCode,
           owinContext.Response.Headers);
      };
    }
  }
}
