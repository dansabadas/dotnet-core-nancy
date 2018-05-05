using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Hello_Microservices
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildCustomWebHost(args).Run();
    }

    public static IWebHost BuildDefaultWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();

    public static IWebHost BuildCustomWebHost(string[] args) =>
      new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseIISIntegration()
        .UseStartup<Startup>()
        .UseUrls("http://localhost:5000")
        .Build();
  }
}
