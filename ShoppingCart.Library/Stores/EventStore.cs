using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using ShoppingCart.Library.DomainModels;

namespace ShoppingCart.Library.Stores
{
  public class EventStore : IEventStore
  {
    private const string __connectionString = @"Data Source=s12.winhost.com;Initial Catalog=DB_100072_eastwest;Persist Security Info=True;User ID=DB_100072_eastwest_user;Password=batran11";

    private const string writeEventSql = @"insert into dotnetcore.EventStore(Name, OccurredAt, Content) values (@Name, @OccurredAt, @Content)";

    public async Task Raise(string eventName, object content)
    {
      var jsonContent = JsonConvert.SerializeObject(content);
      using (var conn = new SqlConnection(__connectionString))
      {
        await
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

    private const string readEventsSql = @"select * from dotnetcore.EventStore where ID >= @Start and ID <= @End";

    public async Task<IEnumerable<Event>> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
      using (var conn = new SqlConnection(__connectionString))
      {
        return (
          await conn.QueryAsync<dynamic>(
            readEventsSql,
            new
            {
              Start = firstEventSequenceNumber,
              End = lastEventSequenceNumber
            })
            .ConfigureAwait(false)
          )
          .Select(row =>
          {
            var content = JsonConvert.DeserializeObject(row.Content);
            return new Event(row.ID, row.OccurredAt, row.Name, content);
          });
      }
    }
  }
}
