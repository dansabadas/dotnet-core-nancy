using Nancy;
using Nancy.Bootstrapper;
using System;

namespace Hello_Microservices
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    //  protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    //  {
    //    container.Register<IShoppingCartStore, ShoppingCartStore>();
    //  }

    protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
      => NancyInternalConfiguration.WithOverrides(builder => builder.StatusCodeHandlers.Clear()); // when 500 or 404, no default HTML message will be generated!
  }
}
