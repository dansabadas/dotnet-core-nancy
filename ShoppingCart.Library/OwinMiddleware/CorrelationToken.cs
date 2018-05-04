using System;
using LibOwin;
using Serilog.Context;

namespace ShoppingCart.Library.OwinMiddleware
{
  using AppFunc = Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

  public class CorrelationToken
  {
    public static AppFunc Middleware(AppFunc next)
    {
      return async env =>
      {
        Guid correlationToken;
        var owinContext = new OwinContext(env);
        if (!(owinContext.Request.Headers["Correlation-Token"] != null
            && Guid.TryParse(owinContext.Request.Headers["Correlation-Token"], out correlationToken)))
        {
          correlationToken = Guid.NewGuid();
        }

        owinContext.Set("correlationToken", correlationToken.ToString());
        using (LogContext.PushProperty("CorrelationToken", correlationToken))
        {
          await next(env);
        }
      };
    }
  }
}
