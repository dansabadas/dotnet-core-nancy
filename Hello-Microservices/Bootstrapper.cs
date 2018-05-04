using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.TinyIoc;
using Serilog;
using ShoppingCart.Library;
using ShoppingCart.Library.Stores;
using System;

namespace Hello_Microservices
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    private readonly ILogger _log;

    public Bootstrapper(ILogger log)
    {
      _log = log;
    }

    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      container.Register<IEventStore, ShoppingCart.Library.Stores.EventStore>();
      container.Register(_log);

      pipelines.OnError += (ctx, ex) =>
      {
        // write to central log store
        return null;
      };
    }

    protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
      => NancyInternalConfiguration.WithOverrides(builder => builder.StatusCodeHandlers.Clear()); // when 500 or 404, no default HTML message will be generated!

    protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
    {
      base.RequestStartup(container, pipelines, context);
      var correlationToken = context.GetOwinEnvironment()["correlationToken"] as string;
      container.Register<IHttpClientFactory>(new HttpClientFactory(correlationToken));
    }
  }
}
