using ShoppingCart.Library.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Library.Stores
{
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
}
