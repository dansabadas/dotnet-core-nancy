using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Microsoft.AspNetCore.Hosting;

namespace LoyaltyProgram_IntegrationTest
{


  public class RegisterUserAndGetNotification : IDisposable
  {
    private IWebHost hostForFakeEndpoints;
    private Process eventConsumer;
    private Process api;
    private Thread thread;

    public RegisterUserAndGetNotification()
    {
      StartLoyaltyProgram();
      StartFakeEndpoints();
    }

    private void StartFakeEndpoints()
    {
      hostForFakeEndpoints = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<FakeStartup>()
        .UseUrls("http://localhost:5001")
        .Build();

      thread = new Thread(() => hostForFakeEndpoints.Run());
      thread.Start();
    }

    private void StartLoyaltyProgram()
    {
      StartEventConsumer();
      StartLoyaltyProgramApi();
    }

    private void StartLoyaltyProgramApi()
    {
      var apiInfo = new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "run",
        WorkingDirectory = "../Hello-Microservices"
      };
      api = Process.Start(apiInfo);
    }

    private void StartEventConsumer()
    {
      var eventConsumerInfo = new ProcessStartInfo("dotnet.exe")
      {
        Arguments = "run localhost:5001",
        WorkingDirectory = "../ApiGateway-Console"
      };
      eventConsumer = Process.Start(eventConsumerInfo);
    }

    [Fact]
    public void Scenario()
    {
      RegisterNewUser().Wait();
      WaitForConsumerToReadSpeciallOffersEvents();
      AssertNotificationWassent();
    }

    private async Task RegisterNewUser()
    {
      using (var httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri("http://localhost:5000");
        var response = await
          httpClient
            .PostAsync(
              "/users/",
              new StringContent(
                JsonConvert.SerializeObject(new LoyaltyProgramUser()),
                Encoding.UTF8,
                "application/json"))
            .ConfigureAwait(false);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Console.WriteLine("registered users");
      }
    }

    private static void AssertNotificationWassent()
    {
      Assert.True(FakeNotifications.NotificationWasSent);
    }

    private static void WaitForConsumerToReadSpeciallOffersEvents()
    {
      Console.WriteLine("waiting for event poll ....");
      Assert.True(FakeEventFeed.polled.WaitOne(30000));
      Console.WriteLine("got poll ....");
      Thread.Sleep(1000);
      Console.WriteLine("waited for notification ....");
    }

    public void Dispose()
    {
      Console.WriteLine("disposing...");
      eventConsumer.Dispose();
      api.Dispose();
      //this.hostForFakeEndpoints.Dispose();
      Console.WriteLine("disposed...");
    }
  }

  public class LoyaltyProgramUser
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int LoyaltyPoints { get; set; }
    public LoyaltyProgramSettings Settings { get; set; }
  }

  public class LoyaltyProgramSettings
  {
    public string[] Interests { get; set; }
  }
}
