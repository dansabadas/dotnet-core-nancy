using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibOwin;

namespace ShoppingCart.Library.OwinMiddleware
{
  using AppFunc = Func<IDictionary<string, object>, Task>;

  public class MonitoringMiddleware
  {
    private AppFunc _next;
    private Func<Task<bool>> _healthCheck;

    private static readonly PathString monitorPath = new PathString("/_monitor");
    private static readonly PathString monitorShallowPath = new PathString("/_monitor/shallow");
    private static readonly PathString monitorDeepPath = new PathString("/_monitor/deep");

    public MonitoringMiddleware(AppFunc next, Func<Task<bool>> healthCheck)
    {
      _next = next;
      _healthCheck = healthCheck;
    }

    public Task Invoke(IDictionary<string, object> env)
    {
      var context = new OwinContext(env);
      if (context.Request.Path.StartsWithSegments(monitorPath))
      {
        return HandleMonitorEndpoint(context);
      }

      return _next(env);
    }

    private Task HandleMonitorEndpoint(OwinContext context)
    {
      if (context.Request.Path.StartsWithSegments(monitorShallowPath))
      {
        return ShallowEndpoint(context);
      }

      if (context.Request.Path.StartsWithSegments(monitorDeepPath))
      {
        return DeepEndpoint(context);
      }

      return Task.FromResult(0);
    }

    private async Task DeepEndpoint(OwinContext context)
    {
      context.Response.StatusCode = await _healthCheck()
        ? 204
        : 503;
    }

    private Task ShallowEndpoint(OwinContext context)
    {
      context.Response.StatusCode = 204;
      return Task.FromResult(0);
    }
  }
}
