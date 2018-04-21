using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ShoppingCart.Library.DomainModels;

namespace ShoppingCart.Library.Stores
{
  public interface IEventStore
  {
    IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);

    void Raise(string eventName, object content);
  }

  public class EventStore : IEventStore
  {
    private static long _currentSequenceNumber = 0;
    private static readonly IList<Event> _database = new List<Event>();

    public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber) =>
      _database
          .Where(e =>
            e.SequenceNumber >= firstEventSequenceNumber &&
            e.SequenceNumber <= lastEventSequenceNumber)
          .OrderBy(e => e.SequenceNumber);

    public void Raise(string eventName, object content)
    {
      var seqNumber = Interlocked.Increment(ref _currentSequenceNumber);
      _database.Add(
        new Event(
          seqNumber,
          DateTimeOffset.UtcNow,
          eventName,
          content));
    }
  }
}
