using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using ShoppingCart.Library.Stores;
using System;

namespace Hello_Microservices
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      pipelines.OnError += (ctx, ex) =>
      {
        // write to central log store
        return null;
      };

      container.Register<IEventStore, ShoppingCart.Library.Stores.EventStore>();
    }

    protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
      => NancyInternalConfiguration.WithOverrides(builder => builder.StatusCodeHandlers.Clear()); // when 500 or 404, no default HTML message will be generated!
  }
}
