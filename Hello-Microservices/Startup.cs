using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using LibOwin;
using ShoppingCart.Library;
using ShoppingCart.Library.OwinMiddleware;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Library.DomainModels.LoginConfiguration;

namespace Hello_Microservices
{
  public class Startup
  {
    private readonly IHostingEnvironment environment;

    public Startup(IHostingEnvironment env)
    {
      environment = env;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddTransient<IShoppingCartStore>
      //var cert = new X509Certificate2(Path.Combine(this.environment.ContentRootPath, "idsrv3test.pfx"), "idsrv3test");

      //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      //var builder = services.AddIdentityServer().AddSigningCredential(cert);

      //builder.AddInMemoryClients(Clients.Get());
      //builder.AddInMemoryApiResources(Scopes.Get());
      //builder.AddInMemoryIdentityResources(Resources.GetIdentityResources());//Users.Get()

      //services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment envi)
    {
      var log = ILoggerFactory.ConfigureLogger();

      app.UseOwin(buildFunc =>  // let's you use OWIN with ASP.NET Core
      {
        buildFunc(next => CorrelationToken.Middleware(next));
        buildFunc(next =>       // buildFunc builds an OWIN pipeline from MidFunc
          env => 
          {
            var context = new OwinContext(env);
            var method = context.Request.Method;
            var path = context.Request.Path;
            System.Console.WriteLine($"Got Request lambdas: {method} {path}");
            return next(env);
          });
        buildFunc(next => new ConsoleMiddleware(next).Invoke);
        buildFunc(next => RequestLogging.Middleware(next, log));
        buildFunc(next => PerformanceLogging.Middleware(next, log));
        buildFunc(next => new MonitoringMiddleware(next, ShoppingCart.Library.Stores.ShoppingCartStore.HealthCheck).Invoke);
        //buildFunc.UseNancy(); // this uses the default parameterless Nancy bootstrapper
        buildFunc.UseNancy(opt => opt.Bootstrapper = new Bootstrapper(log));
      });
    }
  }
}
