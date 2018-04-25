using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using ShoppingCart.Library.DomainModels;

namespace ShoppingCart.Library.Stores
{
  public interface IEventStore
  {
    Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);

    Task Raise(string eventName, object content);
  }

  public class EventStoreInMemory : IEventStore
  {
    private static long _currentSequenceNumber = 0;
    private static readonly IList<Event> _database = new List<Event>();

    public Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
      return Task.FromResult(_database
        .Where(e =>
          e.SequenceNumber >= firstEventSequenceNumber &&
          e.SequenceNumber <= lastEventSequenceNumber)
        .OrderBy(e => e.SequenceNumber)
        .AsEnumerable()
      );
    }

    public Task Raise(string eventName, object content)
    {
      var seqNumber = Interlocked.Increment(ref _currentSequenceNumber);
      _database.Add(
        new Event(
          seqNumber,
          DateTimeOffset.UtcNow,
          eventName,
          content));

      return Task.CompletedTask;
    }
  }

  public class EventStore : IEventStore
  {
    private string connectionString = @"Data Source=s12.winhost.com;Initial Catalog=DB_100072_eastwest;Persist Security Info=True;User ID=DB_100072_eastwest_user;Password=batran11";

    private const string writeEventSql =
      @"insert into EventStore(Name, OccurredAt, Content) values (@Name, @OccurredAt, @Content)";

    public Task Raise(string eventName, object content)
    {
      var jsonContent = JsonConvert.SerializeObject(content);
      using (var conn = new SqlConnection(connectionString))
      {
        return
          conn.ExecuteAsync(
            writeEventSql,
            new
            {
              Name = eventName,
              OccurredAt = DateTimeOffset.Now,
              Content = jsonContent
            });
      }
    }

    private const string readEventsSql =
      @"select * from dotnetcore.EventStore where ID >= @Start and ID <= @End";

    public async Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
      using (var conn = new SqlConnection(connectionString))
      {
        return (await conn.QueryAsync<dynamic>(
            readEventsSql,
            new
            {
              Start = firstEventSequenceNumber,
              End = lastEventSequenceNumber
            })
            .ConfigureAwait(false))
            .Select(row =>
            {
              var content = JsonConvert.DeserializeObject(row.Content);
              return new Event(row.ID, row.OccurredAt, row.Name, content);
            });
      }
    }
  }
}
