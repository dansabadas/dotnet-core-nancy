﻿using Newtonsoft.Json;
using ShoppingCart.Library.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using static System.Console;

namespace ApiGateway_Console
{
  public class EventSubscriber
  {
    private readonly string _loyaltyProgramHost;
    private long _start = 0, _chunkSize = 100;
    private readonly Timer _timer;

    public EventSubscriber(string loyaltyProgramHost)
    {
      WriteLine("created");
      _loyaltyProgramHost = loyaltyProgramHost;
      _timer = new Timer(10 * 1000)
      {
        AutoReset = false
      };
      _timer.Elapsed += (_, __) => SubscriptionCycleCallback().Wait();
    }

    private async Task SubscriptionCycleCallback()
    {
      var response = await ReadEvents();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        await HandleEvents(await response.Content.ReadAsStringAsync());
      }
      _timer.Start();
    }

    private async Task<HttpResponseMessage> ReadEvents()
    {
      long startNumber = await ReadStartNumber().ConfigureAwait(false);
      using (var httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri($"http://{_loyaltyProgramHost}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var response = await httpClient.GetAsync($"/events/?start={startNumber}&end={_start + _chunkSize}");
        PrettyPrintResponse(response);
        return response;
      }
    }

    private long _startDbNumber = 0;
    private Task<long> ReadStartNumber()
    {
      return Task.FromResult(_startDbNumber);
    }

    private async Task HandleEvents(string content)
    {
      WriteLine("Handling events");
      var lastSucceededEvent = 0L;
      var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(content);
      WriteLine(events);
      WriteLine(events.Count());
      foreach (var ev in events)
      {
        WriteLine(ev.Content);
        dynamic eventData = ev.Content;
        WriteLine("product name from data: " + eventData?.item?.productName ?? "NULL");
        _start = Math.Max(_start, ev.SequenceNumber + 1);
        lastSucceededEvent = ev.SequenceNumber + 1;
      }

      await WriteStartNumber(lastSucceededEvent);
    }

    private Task WriteStartNumber(long lastSucceededEvent)
    {
      _startDbNumber = lastSucceededEvent;  // TODO: write to some DB
      return Task.CompletedTask;
    }

    public void Start()
    {
      _timer.Start();
    }

    public void Stop()
    {
      _timer.Stop();
    }

    private static async void PrettyPrintResponse(HttpResponseMessage response)
    {
      WriteLine("Status code: " + response?.StatusCode.ToString() ?? "command failed");
      WriteLine("Headers: " + response?.Headers.Aggregate("", (acc, h) => acc + "\n\t" + h.Key + ": " + h.Value) ?? "");
      WriteLine("Body: " + await response?.Content.ReadAsStringAsync() ?? "");
    }
  }
}
