using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using ShoppingCart.Library.Stores;

namespace Hello_Microservices.NancyModules
{
  public class NancyManualRegistrationBootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      container.Register<IShoppingCartStore, ShoppingCartStore>();
    }
  }
}
